using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coversCompleted;
    public Transform ticketSpawn;
    public GameObject ticketPrefab;
    private float timer;
    private float score;
    private bool isActive;
    private int orderNum = 0;
    private Hashtable openOrders = new Hashtable();
    private readonly int MAX_ORDERS = 3;

    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating("DrawNewOrder", 0f, 20f);
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
        }
    }

    public void DrawNewOrder()
    {
        if (canMakeMore())
        {
            orderNum++;
            Order newOrder = new Order(orderNum);
            openOrders.Add(orderNum, newOrder);

            createTicket(newOrder);
        }
    }

    public void RedrawLastOrder()
    {
        Order order = (Order)openOrders[orderNum];
        Destroy(order.gObj);

        createTicket(order);
    }

    public bool canMakeMore()
    {
        return openOrders.Count < MAX_ORDERS;
    }

    private void createTicket(Order newOrder)
    {
        GameObject newTicket = Instantiate(ticketPrefab, ticketSpawn.position, Quaternion.identity);

        newTicket.GetComponent<Printable>().orderNum = newOrder.orderNum;
        newTicket.GetComponent<Printable>().orderString = newOrder.ToString();

        newOrder.gObj = newTicket;
    }

    public float GetScore()
    {
        return score;
    }

    public (float, string) EvaluateOrder(HashSet<Plate> plates, int orderNum)
    {
        if (!openOrders.ContainsKey(orderNum))
        {
            return (0f, "Old order sent, wasted food");
        }
        Order order = (Order)openOrders[orderNum];
        print("Received order" + order);
        float total = 0;
        string orderComments = "";
        float maxScore;
        string matchComments = "";
        string plateComments = "";
        float currScore;
        HashSet<Orderable> remainingCovers = new HashSet<Orderable>(order.contents);
        Orderable closestMatch;
        foreach (Plate p in plates)
        {
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

            float rand = Random.Range(0, 1);
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
            hasSauce = Random.Range(0, 1) > .20;
        }
        public (float, string) Evaluate(Plate p)
        {
            float total = 0;
            string comments = "";
            Steak steak = null;
            HashSet<Fries> fries = new HashSet<Fries>();
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
                    fries.Add(currFry);
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
            (tempVal, tempString) = f.Evaluate(fries);
            total += tempVal;
            comments += tempString;

            if (hasSteak == false)
            {
                total = 0;
                comments += "Missing steak";
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
            float rand = Random.Range(0, 1);
            noSalt = rand < .80 ? false : true;
            noParsley = rand > .1 ? false : true;
            rand = Random.Range(0, 1);
            extraCrispy = rand > .8 ? true : false;
        }

        public (float, string) Evaluate(HashSet<Fries> f)
        {
            string comments = "Fries: ";
            float total = 5;
            float parsley = 0;
            float salt = 0;
            int numBurntFries = 0;
            int numRawFries = 0;
            foreach (Fries fry in f)
            {
                parsley += fry.seasoning.parsley;
                salt += fry.seasoning.salt;
                if (fry.temp.maxTemp > 190 * 1.15f)
                {
                    numBurntFries += 1;
                }
                else if (fry.temp.maxTemp < 190f)
                {
                    numRawFries += 1;
                }
            }

            if (f.Count < 130 / 8)
            {
                total -= 2;
                comments += "Not enough fries, ";
            }
            if (numRawFries > 3)
            {
                total -= 1;
                comments += "Order contained raw fries, ";
            }
            if (numBurntFries > f.Count / 2 && !extraCrispy)
            {
                total -= 1;
                comments += "Fries overcooked, ";
            }
            if (parsley < 5f && !noParsley)
            {
                total -= .5f;
                comments += "Not enough parsley, ";
            }
            else if (parsley > 0f && noParsley)
            {
                total -= .5f;
                comments += "Order contained parsley, ";
            }
            if (salt < 5f && !noSalt)
            {
                total -= .5f;
                comments += "Not enough salt, ";
            }
            else if (salt > 8f && !noSalt)
            {
                total -= .5f;
                comments += "Too salty, ";
            }
            else if (salt > 0f && noSalt)
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

