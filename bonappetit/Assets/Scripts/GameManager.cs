//using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DishManager dishManager;
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

    private string menu = "10000";

    private int ticketOption; // 0 is single, 1 is multiple

    public bool test = false;

    public bool tutorial = false;

    // Start is called before the first frame update

    void Start() {
        if (test) {
            PhotonNetwork.Destroy(startButton);
            CreateTestOrders();
        } else if (tutorial) {
            PhotonNetwork.Destroy(startButton);
            createTutorialOrder();
        }
    }
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
            ExitGames.Client.Photon.Hashtable currHt = PhotonNetwork.CurrentRoom.CustomProperties;
            ht["startTime"] = startTime;
            ticketOption = (int)currHt["ticketOption"];
            menu = (string)currHt["FoodDisplay"];
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

    public void createTutorialOrder() {
        Order newOrder = new Order(1, ticketOption, menu, false, true);
        openOrders.Add(1, newOrder);
        createTicket(newOrder);
    }

    private void CreateTestOrders() {
        for (int i = 0; i < 5; i++) {
            Order newOrder = new Order(i, ticketOption, menu, true, false);
            openOrders.Add(i, newOrder);
            createTicket(newOrder);
        }
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
            Order newOrder = new Order(orderNum, ticketOption, menu, false, false);
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
        GameObject newTicket = null;
        if (test) {
            newTicket = Instantiate(ticketPrefab, pos, ticketSpawn.rotation);
        } else {
            newTicket = PhotonNetwork.Instantiate(ticketPrefab.name, pos, ticketSpawn.rotation);
        }
        newTicket.GetComponent<Printable>().orderNum = orderNum;
        newTicket.GetComponent<Printable>().orderString = comments;
        GameObject desc = newTicket.transform.Find("order desc").gameObject;
        if (desc != null && desc.TryGetComponent(out TextMeshPro textMesh))
        {
            textMesh.alignment = TextAlignmentOptions.TopLeft;
        }
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
                ticket.gObj.transform.position.x + (.3f * Mathf.Cos(ticketSpawn.eulerAngles.y * (float)System.Math.PI / -180f)),
                ticket.gObj.transform.position.y,
                ticket.gObj.transform.position.z + (.3f * Mathf.Sin(ticketSpawn.eulerAngles.y * (float)System.Math.PI / -180f))
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
                ticketSpawn.position.x + (.3f * (index - 1 - i) * Mathf.Cos(ticketSpawn.eulerAngles.y * (float)System.Math.PI / -180f)),
                ticketSpawn.position.y,
                ticketSpawn.position.z + (.3f * (index - 1 - i) * Mathf.Sin(ticketSpawn.eulerAngles.y * (float)System.Math.PI / -180f))
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
        GameObject newTicket;
        if (test || tutorial) {
            newTicket = Instantiate(ticketPrefab, ticketSpawn.position, ticketSpawn.rotation);
        } else {
            newTicket = PhotonNetwork.Instantiate(ticketPrefab.name, ticketSpawn.position, ticketSpawn.rotation);
        }
        newTicket.GetComponent<Printable>().orderNum = newOrder.orderNum;
        newTicket.GetComponent<Printable>().orderString = newOrder.ToString();

        newOrder.gObj = newTicket;
    }

    public float GetScore()
    {
        return score;
    }

    public (float, string) EvaluateOrder(HashSet<Dish> plates, int orderNum) {
        if (!openOrders.ContainsKey(orderNum)) {
            return (0f, "Feedback: Old order sent, wasted food");
        }
        Order order = (Order)openOrders[orderNum];
        Debug.Log("Received order" + order);
        float total = 0;
        string orderComments = "Feedback: \n";
        float maxScore;
        string matchComments = "";
        string plateComments = "";
        float currScore;
        HashSet<Orderable> remainingCovers = new HashSet<Orderable>(order.contents);
        Orderable closestMatch;
        int extraPlates = 0;

        foreach (Dish p in plates) {
            closestMatch = null;
            maxScore = 0;
            foreach (Orderable o in remainingCovers)
            {
                if (o is SteakFritesOrder && p.dishID == "steakfrites")
                {
                    (currScore, plateComments) = ((SteakFritesOrder)o).Evaluate(p);
                } else if (o is OnionSoupOrder && p.dishID == "onionsoup") {
                    (currScore, plateComments) = ((OnionSoupOrder)o).Evaluate(p);
                } else if (o is TableBreadOrder && p.dishID == "tablebread") {
                    (currScore, plateComments) = ((TableBreadOrder)o).Evaluate(p);
                } else if (o is CrabCakeOrder && p.dishID == "crabcakes") {
                    (currScore, plateComments) = ((CrabCakeOrder)o).Evaluate(p);
                } else if (o is RoastChickenOrder && p.dishID == "roastchicken") {
                    (currScore, plateComments) = ((RoastChickenOrder)o).Evaluate(p);
                } else { 
                    currScore = 0; plateComments = "Wrong order :("; 
                }

                // maxScore = Mathf.Max(maxScore, currScore);
                if (currScore > maxScore)
                {
                    closestMatch = o;
                    matchComments = plateComments;
                    maxScore = currScore;
                }
                if (maxScore >= 5f)
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
                extraPlates++;
            }
        }
        if (extraPlates > 0)
        {
            orderComments += extraPlates + " extra dishes were sent in\n";
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
        public Order(int orderNum, int ticketOption, string menu, bool isTest, bool isTutorial)
        {
            this.orderNum = orderNum;
            partySize = 1;
            if (isTest) {
                switch(orderNum) {
                    case 0: contents.Add(new SteakFritesOrder(1, true, true, false, true)); break;
                    case 1: contents.Add(new OnionSoupOrder(true)); break;
                    case 2: contents.Add(new RoastChickenOrder()); break;
                    case 3: contents.Add(new TableBreadOrder()); break;
                    case 4: contents.Add(new CrabCakeOrder()); break;
                }
                return;
            }
            if (isTutorial) {
                partySize = 2;
                contents.Add(new OnionSoupOrder(false));
                contents.Add(new CrabCakeOrder());
                return;
            }
            if (ticketOption == 1) {
                float rand = Random.Range(0f, 1f);
                if (rand > .5)
                {
                    partySize = 2;
                } else if (rand > .8)
                {
                    partySize = 3;
                }
            }

            for (int i = 0; i < partySize; i++)
            {
                contents.Add(GenerateDish(menu));
            }
        }

        private Orderable GenerateDish(string menu)
        {
            List<Orderable> dishPool = new List<Orderable>();
            if (menu[0] == '1') {
                dishPool.Add(new SteakFritesOrder());
            }
            if (menu[1] == '1') {
                dishPool.Add(new CrabCakeOrder());
            }
            if (menu[2] == '1') {
                dishPool.Add(new OnionSoupOrder());
            }
            if (menu[3] == '1') {
                dishPool.Add(new RoastChickenOrder());
            }
            if (menu[4] == '1') {
                dishPool.Add(new TableBreadOrder());
            }
            if (dishPool.Count == 0) { return new TableBreadOrder();}
            return dishPool[Random.Range(0,dishPool.Count)];
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

        public OnionSoupOrder(bool hasBread) {
            this.hasBread = hasBread;
        }
        public OnionSoupOrder() {
            hasBread = Random.Range(0f, 1f) > .10;
        }

        public override string ToString()
        {
            return "French Onion Soup\n" + (hasBread ? "" : "-MODIFICATION: NO BREAD\n");
        }

        public (float, string) Evaluate(Dish p) {
            float total = 0;
            bool foundBread = false;
            string comments = "French onion soup: ";
            List<string> commentList = new List<string>();

            // evaluate bread
            foreach (int id in p.connectedItems) {
                GameObject target = PhotonView.Find(id).gameObject;
                if (target.tag == "bread") {
                    foundBread = true;
                    break;
                }
            }
            if ((foundBread && hasBread) || (!foundBread && !hasBread)) {
                total += 5;
            } else {
                total += 0;
                commentList.Add(foundBread ? "didn't leave out bread" : "forgot bread");
            }

            // evaluate cheese and parsley
            Seasonable s = p.GetComponent<Seasonable>();
            if (s == null) {
                Debug.Log("Couldn't find seasonable on french onion soup");
            } else {
                if (s.gruyere >= 5) {
                    total += 5;
                } else {
                    total += 2;
                    commentList.Add("not enough cheese");
                }
                if (s.parsley >= 2) {
                    total += 5;
                } else {
                    total += 2;
                    commentList.Add("not enough parsley");
                }
            }

            // evaluate soup
            LiquidContainer l = p.GetComponent<LiquidContainer>();
            if (l == null) {
                Debug.Log("Couldn't find liquid container on onion soup");
            } else {
                if (l.tag == "french onion soup" && l.currentVolume >= 500) {
                    total += 5;
                } else if (p.tag == "french onion soup" && l.currentVolume < 500 && l.currentVolume > 0) {
                    total += 3;
                    commentList.Add("not enough soup");
                } else {
                    total += 0;
                    commentList.Add("wrong or missing soup");
                }
            }

            // evaluate toastiness
            Cheese c = p.GetComponentInChildren<Cheese>();
            if (c.toastingTime >= 10) {
                total += 5;
            } else {
                total += 2;
                commentList.Add("not toasted enough");
            }

            comments += (commentList.Count > 0 ? string.Join(", ", commentList.ToArray()) : "no comments, perfectly cooked!") + '\n';
            return (total / 4, comments);
        }
    }
    private class SteakFritesOrder : Orderable
    {
        SteakOrder s = null;
        FryOrder f = null;
        BearnaiseOrder b = null;
        bool hasSauce = true;

        public SteakFritesOrder(int doneness, bool parsley, bool salt, bool crispy, bool bearnaise) {
            s = new SteakOrder(doneness);
            f = new FryOrder(salt, parsley, crispy);
            b = new BearnaiseOrder();
            hasSauce = bearnaise;
        }
        public SteakFritesOrder()
        {
            s = new SteakOrder();
            f = new FryOrder();
            b = new BearnaiseOrder();
            hasSauce = Random.Range(0f, 1f) > .20;
        }
        public (float, string) Evaluate(Dish p) {
            float total = 0;
            string comments = "Steak Frites: ";
            Steak steak = null;
            Fries fry = null;
            LiquidContainer bearnaise = null;
            List<string> commentList = new List<string>();
            List<string> missingList = new List<string>();

            float tempVal = 0;
            string tempString = "";
            foreach (int id in p.connectedItems) {
                GameObject target = PhotonView.Find(id).gameObject;
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
                commentList.Add(tempString);
            } else {
                total += 0;
                missingList.Add("missing steak");
            }

            if (fry != null) {
                (tempVal, tempString) = f.Evaluate(fry);
                total += tempVal;
                commentList.Add(tempString);
            } else {
                total += 0;
                missingList.Add("missing fries");
            }

            if (bearnaise != null) {
                (tempVal, tempString) = b.Evaluate(bearnaise);
                total += tempVal;
                commentList.Add(tempString);
            } else if (!hasSauce) {
                total += 5;
            } else {
                total += 0;
                missingList.Add("missing bearnaise");
            }

            comments += string.Join(", ", missingList.ToArray()) + '\n';
            comments += string.Join("", commentList.ToArray());
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
        public SteakOrder(int doneness) {
            expectedDoneness = doneness;
        }
        public SteakOrder()
        {
            expectedDoneness = (int)Random.Range(0, Steak.donenessLabels.Length - 1);
        }

        public (float, string) Evaluate(Steak s)
        {
            int result = 5;
            string comments = "Steak notes: ";
            List<string> commentList = new List<string>();
            int doneness = s.GetDonenessValue();
            if (doneness == -1)
            {
                result = 0;
                comments += "steak was still mooin\n";
                return (result, comments);
            }
            if (doneness != expectedDoneness)
            {
                result -= 2;
                commentList.Add("expected " + Steak.donenessLabels[expectedDoneness] + " received " + s.GetDonenessLabel());
            }

            if (s.searTime < 30)
            {
                result -= 1;
                commentList.Add("not seared enough");
            }
            if (s.searTime > 45)
            {
                result -= 1;
                commentList.Add("burnt exterior");
            }
            if (s.restTime < 45)
            {
                result -= 1;
                commentList.Add("not rested long enough");
            }
            if (Mathf.Abs(s.seasoning.salt - 7) > 2)
            {
                result -= 1;
                commentList.Add(s.seasoning.salt > 7 ? "too much salt" : "not enough salt");
            }

            if (Mathf.Abs(s.seasoning.pepper - 5) > 2)
            {
                result -= 1;
                commentList.Add(s.seasoning.pepper > 5 ? "too much pepper" : "not enough pepper");
            }

            comments += (commentList.Count > 0 ? string.Join(", ", commentList.ToArray()) : "no comments, perfectly cooked!") + '\n';
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

        public FryOrder(bool salt, bool parsley, bool crispy) {
            noSalt = !salt;
            noParsley = !parsley;
            extraCrispy = crispy;
        }
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
            List<string> commentList = new List<string>();
            float total = 5;

            if (f.temp.maxTemp > 190 * 1.15f) {
                if(!extraCrispy) {
                    total -= 1;
                    commentList.Add("overcooked");
                }
            }
            if (f.temp.maxTemp < 190)
            {
                total -= 2;
                commentList.Add("undercooked");
            }
            if (f.seasoning.parsley < 4f && !noParsley)
            {
                total -= .5f;
                commentList.Add("not enough parsley");
            }
            else if (f.seasoning.parsley > 0f && noParsley)
            {
                total -= .5f;
                commentList.Add("parsley allergy ignored");
            }
            if (f.seasoning.salt < 4f && !noSalt)
            {
                total -= .5f;
                commentList.Add("under salted");
            }
            else if (f.seasoning.salt > 8f && !noSalt)
            {
                total -= .5f;
                commentList.Add("too salty");
            }
            else if (f.seasoning.salt > 0f && noSalt)
            {
                total -= .5f;
                commentList.Add("no salt request was ignored");
            }

            comments += (commentList.Count > 0 ? string.Join(", ", commentList.ToArray()) : "no comments, perfectly cooked!") + '\n';
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

    private class CrabCakeOrder : Orderable {

        public (float, string) Evaluate(Dish p) {
        
            float total = 0;
            string comments = "Crab Cakes: ";
            List<string> commentList = new List<string>();
            int crabCakesFound = 0;
            bool foundSprouts = false;
            foreach (int id in p.connectedItems) {
                GameObject target = PhotonView.Find(id).gameObject;
                if (target.tag == "crab_cake") {
                    float itemTotal = 5;
                    crabCakesFound += 1;
                    Cookable c = target.GetComponent<Cookable>();
                    Temperature temp = target.GetComponent<Temperature>();
                    if (c.searTime < c.desiredSearTime) {
                        commentList.Add("under seared");
                        itemTotal -= 1;
                    } else if (c.searTime > c.desiredSearTime * 1.5) {
                        commentList.Add("over seared");
                        itemTotal -= 1;
                    }

                    if (temp.maxTemp < c.cookedTemp) {
                        commentList.Add("under cooked");
                        itemTotal -= 2;
                    } else if (temp.maxTemp > c.cookedTemp * 1.33) {
                        commentList.Add("overcooked");
                        itemTotal -= 3;
                    }
                    total += itemTotal;
                } else if (target.tag == "sprouts") {
                    foundSprouts = true;
                    total += 5;
                }
            }

            if (!foundSprouts) {
                commentList.Add("missing sprouts");
            }

            if (crabCakesFound < 2) {
                commentList.Add("missing crab cake (there should be 2)");
            }

            LiquidContainer l = p.GetComponent<LiquidContainer>();
            if (l.currentVolume <= 0) {
                commentList.Add("missing sauce");
            } else if (l.tag == "bearnaise") {
                total += 5;
            } else {
                // wrong sauce tag
                commentList.Add("incorrect sauce");
            }
            comments += (commentList.Count > 0 ? string.Join(", ", commentList.ToArray()) : "no comments, perfectly cooked!") + '\n';
            return (total / 4, comments);
        }

        public override string ToString()
        {
            return "Crab Cakes\n";
        }
    }

    private class TableBreadOrder : Orderable {

        public override string ToString() {
            return "Table Bread\n";
        }

        public (float, string) Evaluate(Dish p) {
            float total = 0;
            string comments = "Table Bread: ";
            List<string> commentList = new List<string>();
            bool foundBread = false;
            bool foundOil = false;
            foreach (int id in p.connectedItems) {
                GameObject target = PhotonView.Find(id).gameObject;
                if (target.tag == "bread") {
                    foundBread = true;
                    Cookable c = target.gameObject.GetComponent<Cookable>();
                    Temperature temp = target.gameObject.GetComponent<Temperature>();
                    if (temp.maxTemp < c.cookedTemp) {
                        total += 3;
                        commentList.Add("bread was cold");
                    } else if (temp.maxTemp > c.cookedTemp * 1.33) {
                        total += 2;
                        commentList.Add("bread was too hot");
                    } else {
                        total += 5;
                    }
                } else if (target.tag == "olive oil") {
                    total += 5;
                    foundOil = true;
                }
            }

            if (!foundBread) {
                commentList.Add("missing bread");
            }
            
            if (!foundOil) {
                commentList.Add("missing olive oil");
            }

            comments += (commentList.Count > 0 ? string.Join(", ", commentList.ToArray()) : "no comments, perfectly cooked!") + '\n';
            return (total / 2, comments);
        }
    }

    private class RoastChickenOrder : Orderable {
        public override string ToString()
        {
            return "Roast Chicken w/ Vegetables\n";
        }

        public (float, string) Evaluate(Dish p) {
            float total = 0;
            string comments = "Roast Chicken w/ Vegetables: ";
            List<string> commentList = new List<string>();
            bool breastFound = false;
            int wingCount = 0;
            bool vegFound = false;
            foreach (int id in p.connectedItems) {
                GameObject target = PhotonView.Find(id).gameObject;
                if (target.tag == "breast") {
                    float itemTotal = 5;
                    breastFound = true;
                    Cookable c = target.gameObject.GetComponent<Cookable>();
                    Temperature temp = target.gameObject.GetComponent<Temperature>();
                    if (temp.maxTemp < c.cookedTemp) {
                        total += 0;
                        commentList.Add("chicken breast undercooked");
                        break;
                    } else if (temp.maxTemp > c.cookedTemp * 1.33) {
                        itemTotal -= 2;
                        commentList.Add("chicken breast overcooked");
                    }

                    Seasonable s = target.gameObject.GetComponent<Seasonable>();
                    if (s.salt <= 0 || s.pepper <= 0 || s.parsley <= 0) {
                        itemTotal -= 2;
                        commentList.Add("chicken breast seasoning was off");
                    }
                    total += itemTotal;
                } else if (target.tag == "wing") {
                    wingCount += 1;
                    float itemTotal = 5;
                    Cookable c = target.GetComponent<Cookable>();
                    Temperature temp = target.GetComponent<Temperature>();
                    if (temp.maxTemp < c.cookedTemp) {
                        total += 0;
                        commentList.Add("wing undercooked");
                        break;
                    } else if (temp.maxTemp > c.cookedTemp * 1.33) {
                        itemTotal -= 2;
                        commentList.Add("wing overcooked");
                    }

                    Seasonable s = target.GetComponent<Seasonable>();
                    if (s.salt <= 0 || s.pepper <= 0 || s.parsley <= 0) {
                        itemTotal -= 2;
                        commentList.Add("wing seasoning was off");
                    }
                    total += itemTotal;
                } else if (target.tag == "vegetables") {
                    vegFound = true;
                    float itemTotal = 5;
                    Cookable c = target.GetComponent<Cookable>();
                    Temperature temp = target.GetComponent<Temperature>();
                    if (temp.maxTemp < c.cookedTemp) {
                        total += 0;
                        commentList.Add("vegetables undercooked");
                        break;
                    }

                    Seasonable s = target.GetComponent<Seasonable>();
                    if (s.salt <= 0 || s.pepper <= 0 || s.parsley <= 0) {
                        itemTotal -= 2;
                        commentList.Add("vegetable seasoning was off");
                    }
                    total += itemTotal;
                }
            }
            if (!vegFound) {
                commentList.Add("missing vegetables");
            }
            if (!breastFound) {
                commentList.Add("missing chicken breast");
            }
            if (wingCount < 2) {
                commentList.Add("missing wing (there should be 2)");
            }
            LiquidContainer l = p.GetComponent<LiquidContainer>();
            if (l.currentVolume <= 0) {
                commentList.Add("forgot sauce");
            } else if (l.tag == "pan sauce") {
                total += 5;
            } else {
                total += 1;
                commentList.Add("wrong sauce");
            }

            comments += (commentList.Count > 0 ? string.Join(", ", commentList.ToArray()) : "no comments, perfectly cooked!") + '\n';
            return (total / 5, comments);

        }
    }
}