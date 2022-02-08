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

    // Start is called before the first frame update

    public void StartGame() {
        InvokeRepeating("DrawNewOrder", 0f, 20f);
        a.PlayOneShot(startGameSound);
        isActive = true;
        Destroy(startButton);
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
        Debug.Log("End Game");
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom != null) {
            Debug.Log("Final score: " + GetScore());
            ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
            ht["score"] = GetScore();
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
            PhotonNetwork.LoadLevel("Endgame");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient) {
            im.enabled = true;
        }
        if (isActive && PhotonNetwork.Time - startTime > Clock.GAME_LENGTH) {
            EndGame();
        }

        if (startGame && !isActive) {
            StartGame();
        }
    }

    public void DrawNewOrder()
    {
        if (canMakeMore())
        {
            orderNum++;
            Order newOrder = new Order(orderNum);
            openOrders.Add(orderNum, newOrder);
            a.PlayOneShot(startGameSound);
            createTicket(newOrder);
        }
    }

    public void DrawFeedback(int orderNum, string comments) {
        GameObject newTicket = PhotonNetwork.Instantiate(ticketPrefab.name, ticketSpawn.position, Quaternion.identity);

        newTicket.GetComponent<Printable>().orderNum = orderNum;
        newTicket.GetComponent<Printable>().orderString = comments;
        newTicket.tag = "feedback";
    }

    public void RedrawLastOrder()
    {
        Order order = (Order)openOrders[orderNum];
        Destroy(order.gObj);

        createTicket(order);
    }

    public void RedrawAllOrders() {
        foreach(Order ticket in openOrders) {
            Destroy(ticket.gObj);
            createTicket(ticket);
        }
    }

    public bool canMakeMore()
    {
        return openOrders.Count < MAX_ORDERS;
    }

    private void createTicket(Order newOrder)
    {
        GameObject newTicket = PhotonNetwork.Instantiate(ticketPrefab.name, ticketSpawn.position, Quaternion.identity);

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
                if (o is SteakFrites)
                {
                    (currScore, plateComments) = ((SteakFrites)o).Evaluate(p);
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
            if (rand < .1)
            {
                partySize = 1;
            }
            else if (rand < .5)
            {
                partySize = 2;
            }
            else if (rand < .75)
            {
                partySize = 3;
            }
            else if (rand < .95)
            {
                partySize = 4;
            }
            else
            {
                partySize = 6;
            }

            for (int i = 0; i < partySize; i++)
            {
                contents.Add(GenerateDish());
            }
        }

        private Orderable GenerateDish()
        {
            // expand when we add new dishes
            return new SteakFrites();
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

    private class SteakFrites : Orderable
    {
        SteakOrder s = null;
        FryOrder f = null;
        BearnaiseOrder b = null;
        bool hasSauce = true;

        public SteakFrites()
        {
            s = new SteakOrder();
            f = new FryOrder();
            b = new BearnaiseOrder();
            hasSauce = Random.Range(0f, 1f) > .20;
        }
        public (float, string) Evaluate(GameObject p) {
            float total = 0;
            string comments = "SteakFrites: ";
            Steak steak = null;
            bool hasFries = false;
            Fries currFry = null;
            Bearnaise sauce = null;
            bool hasSteak = false;
            float tempVal = 0;
            string tempString = "";
            foreach (Transform child in p.transform)
            {
                GameObject target = child.gameObject;
                steak = target.GetComponent<Steak>();
                if (steak != null)
                {
                    (tempVal, tempString) = s.Evaluate(steak);
                    total += tempVal;
                    comments += tempString;
                    hasSteak = true;
                    continue;
                }
                currFry = target.GetComponent<Fries>();
                if (currFry != null)
                {
                    (tempVal, tempString) = f.Evaluate(currFry);
                    total += tempVal;
                    comments += tempString;
                    hasFries = true;
                    continue;
                }
                sauce = target.GetComponent<Bearnaise>();
                if ((sauce != null && !hasSauce) || (sauce == null && hasSauce))
                {
                    total += 1f;
                    comments += hasSauce ? "forgot sauce, " : "added sauce when requested not to, ";
                }
                else if (sauce != null)
                {
                    (tempVal, tempString) = b.Evaluate(sauce);
                    total += tempVal;
                    comments += tempString;
                }
            }

            if (hasSteak == false)
            {
                total = 0;
                comments += "Missing steak";
            }

            if (!hasFries) {
                total = 0;
                comments += "Missing fries";
            }
            return ((total / 3), comments);
        }

        public override string ToString()
        {
            return "Steak Frites\n" + (hasSauce ? "" : "- NO BEARNAISE\n") + s.ToString() + f.ToString();
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
            string comments = "Steak: ";
            int doneness = s.GetDonenessValue();
            if (doneness == -1)
            {
                result = 0;
                comments = "raw steak inedible, ";
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
            string comments = "Fries: ";
            float total = 5;

            if (f.temp.maxTemp > 190 * 1.15f) {
                if(!extraCrispy) {
                    total -= 1;
                    comments += "Fries overcooked, ";
                }
            }
            if (f.temp.maxTemp < 190)
            {
                total -= 2;
                comments += "Undercooked fries, ";
            }
            if (f.seasoning.parsley < 5f && !noParsley)
            {
                total -= .5f;
                comments += "Not enough parsley, ";
            }
            else if (f.seasoning.parsley > 0f && noParsley)
            {
                total -= .5f;
                comments += "Order contained parsley, ";
            }
            if (f.seasoning.salt < 5f && !noSalt)
            {
                total -= .5f;
                comments += "Not enough salt, ";
            }
            else if (f.seasoning.salt > 8f && !noSalt)
            {
                total -= .5f;
                comments += "Too salty, ";
            }
            else if (f.seasoning.salt > 0f && noSalt)
            {
                total -= .5f;
                comments += "Order contained salt, ";
            }
            return (Mathf.Max(0f, total), comments);

        }

        public override string ToString()
        {
            return "FRIES\n" + (noSalt ? "-NO SALT\n" : "") + (noParsley ? "-NO PARSLEY\n" : "")
            + (extraCrispy ? "-EXTRA CRISPY (burn 'em!)\n" : "");
        }
    }

    private class BearnaiseOrder : Orderable
    {
        public (float, string) Evaluate(Bearnaise b)
        {
            float total = 5;
            string comments = "Bearnaise: ";
            if (b.container.currentVolume < 100)
            {
                comments += "Not enough, ";
                total -= 1;
            }
            if (b.temp.maxTemp > Bearnaise.SEPARATION_TEMP)
            {
                comments += "sauce separated, ";
                total -= 1;
            }
            if (b.temp.temp < 37)
            {
                comments += "cold, ";
                total -= 1;
            }
            return (total, comments);

        }
    }
}

