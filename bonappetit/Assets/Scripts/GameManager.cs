using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public int coversCompleted;
    public Transform ticketSpawn;
    public GameObject ticketPrefab;
    public GameObject startButton;
    public XRInteractionManager im = null;

    public Clock clock = null;
    private float timer;
    private float score;
    private bool isActive;
    private int orderNum = 0;
    private Hashtable openOrders = new Hashtable();
    private readonly int MAX_ORDERS = 3;

    private float startTime;

    public bool startGame = false;

    public AudioClip startGameSound;

    public AudioSource a = null;

    private bool endgame = false;

    // Start is called before the first frame update

    public void StartGame() {
        InvokeRepeating("createStartingOrders", 0f, 10f);
        a.PlayOneShot(startGameSound);
        isActive = true;
        PhotonNetwork.Destroy(startButton);
        startButton = null;
        startTime = (float) PhotonNetwork.Time;
        clock.startTime = startTime;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
            ht["startTime"] = startTime;
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
    }

    void EndGame() {
        // send everyone to the game over screen
        if(!endgame)
        {
            Debug.Log("End Game");
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom != null) {
                Debug.Log("Final score: " + GetScore());
                ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
		        ht["covers"] = coversCompleted;
                ht["score"] = GetScore();
                PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
                PhotonNetwork.LoadLevel("NewEndgame");
            }
            endgame = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient) {
            im.enabled = true;
        } else {
            im.enabled = false;
        }
        if (isActive && PhotonNetwork.Time - startTime > Clock.getGameLength()) {
            EndGame();
        }

        if (startGame && !isActive) {
            StartGame();
        }
    }

    public void createStartingOrders() {
        StartCoroutine(DrawNewOrder());
    }

    public IEnumerator DrawNewOrder()
    {
        if (canMakeMore())
        {
            if (openOrders.Count > 0) {
                moveTray();
                yield return new WaitForSeconds(5);
            }

            orderNum++;
            Order newOrder = new Order(orderNum);
            openOrders.Add(orderNum, newOrder);
            a.PlayOneShot(startGameSound);
            createTicket(newOrder);
        }
    }

    public void DrawFeedback(int orderNum, string comments) {
        Vector3 pos = new Vector3(
            ticketSpawn.position.x + .35f,
            ticketSpawn.position.y,
            ticketSpawn.position.z
        );
        GameObject newTicket = PhotonNetwork.Instantiate(ticketPrefab.name, pos, ticketSpawn.rotation);

        newTicket.GetComponent<Printable>().orderNum = orderNum;
        newTicket.GetComponent<Printable>().orderString = comments;
        newTicket.tag = "feedback";
    }

    public void RedrawLastOrder()
    {
        Order order = (Order)openOrders[orderNum];
        order.gObj.transform.position = ticketSpawn.position;
    }

    public void moveTray()
    {
        foreach(Order ticket in openOrders.Values) {
            Vector3 startPos = ticket.gObj.transform.position;

            Vector3 endPos = new Vector3(
                ticket.gObj.transform.position.x - .3f,
                ticket.gObj.transform.position.y,
                ticket.gObj.transform.position.z
            );

            // Update Order Receipt Transform Property
            ticket.gObj.GetComponent<OrderReceipt>().cachedPosition += 1;

            // animate
            if (ticket.gObj.GetComponent<OrderReceipt>().isStuck) {
                StartCoroutine(MoveObject(ticket.gObj.transform, startPos, endPos));
            }
        }
    }

    IEnumerator MoveObject(Transform t, Vector3 startPos, Vector3 endPos)
    {
        float rate = 1 / 5f;
        float i = 0f;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            t.position = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForSeconds(0);
        }
    }

    public void RedrawAllOrders() {
        List<int> arr = new List<int>();
        foreach (int num in openOrders.Keys) {
            arr.Add(num);
        }

        arr.Sort();
        int index = arr.Count;

        for (int i = index - 1; i >= 0; i--) {
            Order ticket = (Order) openOrders[arr[i]];

            ticket.gObj.transform.position = new Vector3(
                ticketSpawn.position.x - (.3f * (index - 1 - i)),
                ticketSpawn.position.y,
                ticketSpawn.position.z
            );

            ticket.gObj.transform.rotation = ticketSpawn.rotation;
        }
    }

    public bool canMakeMore()
    {
        return openOrders.Count < MAX_ORDERS;
    }

    private void createTicket(Order newOrder)
    {
        // TODO : Change back!!
        GameObject newTicket = PhotonNetwork.Instantiate(ticketPrefab.name, ticketSpawn.position, ticketSpawn.rotation);

        newTicket.GetComponent<Printable>().orderNum = newOrder.orderNum;
        newTicket.GetComponent<Printable>().orderString = newOrder.ToString();

        newOrder.gObj = newTicket;
    }

    public float GetScore()
    {
        return score;
    }

    public (float, string) EvaluateOrder(HashSet<GameObject> plates, int orderNum) {
        if (!openOrders.ContainsKey(orderNum)) {
            return (0f, "Comments: Old order sent, wasted food");
        }
        Order order = (Order)openOrders[orderNum];
        print("Received order" + order);
        float total = 0;
        string orderComments = "Comments: ";
        float maxScore;
        string matchComments = "";
        string plateComments = "";
        float currScore;
        HashSet<Orderable> remainingCovers = new HashSet<Orderable>(order.contents);
        Orderable closestMatch;
        foreach (GameObject p in plates) {
            closestMatch = null;
            maxScore = 0;
            foreach (Orderable o in remainingCovers)
            {
                if (o is SteakFritesOrder)
                {
                    (currScore, plateComments) = ((SteakFritesOrder)o).Evaluate(p);
                }
                else { currScore = 0; plateComments = "Bad type"; }
                maxScore = Mathf.Max(maxScore, currScore);
                if (maxScore == currScore)
                {
                    closestMatch = o;
                    matchComments = plateComments;
                }
                if (maxScore == 5f)
                {
                    break;
                }
            }
            if (closestMatch != null)
            {
                remainingCovers.Remove(closestMatch);
                total += maxScore;
                orderComments += matchComments;
            }
            else
            {
                total += 0;
                plateComments += "Don't know who one dish was meant for, ";
            }
        }
        total /= order.partySize;
        score = (score + total) / 2;
        coversCompleted += order.partySize;
        DrawFeedback(orderNum, orderComments);
        openOrders.Remove(orderNum);
        return (total, orderComments);
    }

    private class Order
    {
        public int orderNum;
        public int partySize;
        public List<Orderable> contents = new List<Orderable>();
        public GameObject gObj;
        public Order(int orderNum)
        {
            this.orderNum = orderNum;

            float rand = Random.Range(0f, 1f);
            if (rand < .2)
            {
                partySize = 1;
            }
            else if (rand < .7)
            {
                partySize = 2;
            }
            else
            {
                partySize = 3;
            }

            for (int i = 0; i < partySize; i++)
            {
                contents.Add(GenerateDish());
            }
        }

        private Orderable GenerateDish()
        {
            // expand when we add new dishes
            return new SteakFritesOrder();
        }

        public override string ToString()
        {
            string full = "";
            foreach (Orderable o in contents)
            {
                full += o.ToString();
            }
            return full;
        }


    }

    private class Orderable
    {
        public virtual (float, string) Evaluate(Object p) { return (0, ""); }
        public override string ToString() { return ""; }
    }

    private class OnionSoupOrder : Orderable {
        bool hasBread = true;
        public OnionSoupOrder() {
            hasBread = Random.Range(0f, 1f) > .10;
        }

        public override string ToString()
        {
            return "French Onion Soup\n" + (hasBread ? "" : "-MODIFICATION: NO BREAD\n");
        }

        public (float, string) Evaluate(GameObject p) {
            float total = 0;
            bool foundBread = false;
            string comments = "Onion soup: ";

            // evaluate bread
            foreach (Transform child in p.transform) {
                GameObject target = child.gameObject;
                if (target.tag == "bread") {
                    foundBread = true;
                    break;
                }
            }
            if ((foundBread && hasBread) || (!foundBread && !hasBread)) {
                total += 5;
            } else {
                total += 0;
                comments += foundBread ? "didn't leave out bread, " : "forgot bread, ";
            }

            // evaluate cheese and parsley
            Seasonable s = p.GetComponent<Seasonable>();
            if (s == null) {
                Debug.Log("Couldn't find seasonable on french onion soup");
            } else {
                if (s.gruyere >= 10) {
                    total += 5;
                } else {
                    total += 2;
                    comments += "not enough cheese, ";
                }
                if (s.parsley >= 2) {
                    total += 5;
                } else {
                    total += 2;
                    comments += "not enough parsley, ";
                }
            }

            // evaluate soup
            LiquidContainer l = p.GetComponent<LiquidContainer>();
            if (l = null) {
                Debug.Log("Couldn't find liquid container on onion soup");
            } else {
                if (p.tag == "frenchonionsoup" && l.currentVolume >= 500) {
                    total += 5;
                } else if (p.tag == "frenchonionsoup" && l.currentVolume < 500 && l.currentVolume > 0) {
                    total += 3;
                    comments += "not enough soup, ";
                } else {
                    total += 0;
                    comments += "wrong or missing soup, ";
                }
            }

            // evaluate toastiness
            Cheese c = p.GetComponentInChildren<Cheese>();
            if (c.toastingTime >= 10) {
                total += 5;
            } else {
                total += 2;
                comments += "not toasted enough, ";
            }

            return (total / 4, comments);
        }
    }
    private class SteakFritesOrder : Orderable
    {
        SteakOrder s = null;
        FryOrder f = null;
        BearnaiseOrder b = null;
        bool hasSauce = true;

        public SteakFritesOrder()
        {
            s = new SteakOrder();
            f = new FryOrder();
            b = new BearnaiseOrder();
            hasSauce = Random.Range(0f, 1f) > .20;
        }
        public (float, string) Evaluate(GameObject p) {
            float total = 0;
            string comments = "Steak Frites: ";
            Steak steak = null;
            Fries fry = null;
            LiquidContainer bearnaise = null;
        
            float tempVal = 0;
            string tempString = "";
            foreach (Transform child in p.transform)
            {
                GameObject target = child.gameObject;
                if (steak == null && target.tag == "steak") {
                    steak = target.GetComponent<Steak>();
                } else if (fry == null && target.tag == "fry") {
                    fry = target.GetComponent<Fries>();
                } else if (bearnaise == null && target.tag == "bearnaise") {
                    bearnaise = target.GetComponent<LiquidContainer>();
                }
            }

            if (steak != null)
            {
                (tempVal, tempString) = s.Evaluate(steak);
                total += tempVal;
                comments += tempString;
            } else {
                total += 0;
                comments += "missing steak, ";
            }

            if (fry != null) {
                (tempVal, tempString) = f.Evaluate(fry);
                total += tempVal;
                comments += tempString;
            } else {
                total += 0;
                comments += "missing fries, ";
            }

            if (bearnaise != null) {
                (tempVal, tempString) = b.Evaluate(bearnaise);
                total += tempVal;
                comments += tempString;
            } else if (!hasSauce) {
                total += 5;
            } else {
                total += 0;
                comments += "forgot sauce, ";
            }
            return ((total / 3), comments);
        }

        public override string ToString()
        {
            return "Steak Frites\n" + (hasSauce ? "- BEARNAISE ON SIDE\n" : "- NO BEARNAISE\n") + s.ToString() + f.ToString();
        }
    }

    private class SteakOrder : Orderable
    {
        int expectedDoneness;
        public SteakOrder()
        {
            expectedDoneness = (int)Random.Range(0, Steak.donenessLabels.Length - 1);
        }

        public (float, string) Evaluate(Steak s)
        {
            int result = 5;
            string comments = "Steak notes: ";
            int doneness = s.GetDonenessValue();
            if (doneness == -1)
            {
                result = 0;
                comments = "steak was still mooin ";
                return (result, comments);
            }
            if (doneness != expectedDoneness)
            {
                result -= 2;
                comments += "expected " + Steak.donenessLabels[expectedDoneness] + " received " + s.GetDonenessLabel() + ", ";
            }

            if (s.searTime < 120)
            {
                result -= 1;
                comments += "not seared enough, ";
            }
            if (s.searTime > 180)
            {
                result -= 1;
                comments += "burnt exterior, ";
            }
            if (s.restTime < 300f)
            {
                result -= 1;
                comments += "not rested long enough, ";
            }
            if (Mathf.Abs(s.seasoning.salt - 7) > 2)
            {
                result -= 1;
                comments += s.seasoning.salt > 7 ? "too much salt, " : "not enough salt, ";
            }

            if (Mathf.Abs(s.seasoning.pepper - 5) > 2)
            {
                result -= 1;
                comments += s.seasoning.pepper > 5 ? "too much pepper, " : "not enough pepper, ";
            }

            return (Mathf.Max(result, 0f), comments);


        }

        public override string ToString()
        {
            return "NY STRIP\n" + Steak.donenessLabels[expectedDoneness].ToUpper() + "\n";
        }

    }

    private class FryOrder : Orderable
    {
        public bool noSalt = false;
        public bool noParsley = false;
        public bool extraCrispy = false;

        public FryOrder()
        {
            float rand = Random.Range(0f, 1f);
            noSalt = rand < .80 ? false : true;
            noParsley = rand > .1 ? false : true;
            rand = Random.Range(0f, 1f);
            extraCrispy = rand > .8 ? true : false;
        }

        public (float, string) Evaluate(Fries f)
        {
            string comments = "Fries notes: ";
            float total = 5;

            if (f.temp.maxTemp > 190 * 1.15f) {
                if(!extraCrispy) {
                    total -= 1;
                    comments += "overcooked, ";
                }
            }
            if (f.temp.maxTemp < 190)
            {
                total -= 2;
                comments += "undercooked, ";
            }
            if (f.seasoning.parsley < 4f && !noParsley)
            {
                total -= .5f;
                comments += "not enough parsley, ";
            }
            else if (f.seasoning.parsley > 0f && noParsley)
            {
                total -= .5f;
                comments += "parsley allergy ignored, ";
            }
            if (f.seasoning.salt < 4f && !noSalt)
            {
                total -= .5f;
                comments += "under salted, ";
            }
            else if (f.seasoning.salt > 8f && !noSalt)
            {
                total -= .5f;
                comments += "too salty, ";
            }
            else if (f.seasoning.salt > 0f && noSalt)
            {
                total -= .5f;
                comments += "no salt request was ignored ";
            }
            return (Mathf.Max(0f, total), comments);

        }

        public override string ToString()
        {
            return "FRIES\n" + (noSalt ? "-NO SALT\n" : "") + (noParsley ? "-NO PARSLEY\n" : "")
            + (extraCrispy ? "-EXTRA CRISPY (burn 'em!)\n" : "");
        }
    }

    private class BearnaiseOrder : Orderable {
        public (float, string) Evaluate(LiquidContainer b) {
            float total = 5;
            string comments = "";
            if (b.currentVolume < 100) {
                total -= 3;
                comments += "Bearnaise notes: not enough sauce ";
            }
            return (total, comments);
        }
    }
}

