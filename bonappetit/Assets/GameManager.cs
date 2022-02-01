using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timer;
    private float score;
    private bool isActive;

    private float potentialScore;
    private List<Order> openOrders; 
    private List<int> deductions;

    private readonly string[] deductionMeanings = {};

    private readonly Dictionary<string, class> dict; 

    private readonly int MAX_ORDERS = 3;

    // Start is called before the first frame update
    void Start()
    {
        openOrders = DrawOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            timer += Time.deltaTime;
        }
    }

    void SendOrder()

    private void DrawOrder() {
        if (openOrders.Count < MAX_ORDERS) {
            float i = Random.Range(0,1);
        }
    }

    private class Orderable {
        public float Evaluate(Plate p){return 5;}
        public override string ToString(){return null;}
    }

    private class SteakFrites : Orderable {
        SteakOrder s = null;
        FryOrder f = null;
        BearnaiseOrder b = null;
        bool hasSauce = true;
        public float Evaluate(Plate p) {
            float total = 5;
            Steak steak = null;
            HashSet<Fry> fries = new HashSet<Fry>();
            Fry currFry = null;
            Bearnaise sauce = null;
            bool hasSteak = false;
            foreach (Transform child in p.transform) {
                GameObject target = child.gameObject;
                steak = target.GetComponent<Steak>();
                if (steak != null) {
                    total -= s.Evaluate(steak);
                    hasSteak = true;
                    continue;
                }
                currFry = target.GetComponent<Fry>();
                if (currFry != null) {
                    fries.Add(currFry);
                    continue;
                }
                sauce = target.GetComponent<Bearnaise>();
                if ((sauce != null && !hasSauce) || (sauce == null && hasSauce)) {
                    total -= .5f;
                } else if (sauce != null) {
                    total -= b.Evaluate();
                }
            }

            if (hasSteak = false) {
                total = 0;
            }
            return total;
        }

        string ToString() {
            return "Steak Frites\n" + (hasSauce? : "- NO SAUCE\n") + SteakOrder.ToString() + FryOrder.ToString();
        }
    }

    private class SteakOrder : Orderable {

    }
}

