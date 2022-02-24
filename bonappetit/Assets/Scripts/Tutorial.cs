using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial : MonoBehaviour
{
    ArrayList highlightObjs;
    ArrayList startColor;
    string[] tutorialText;

    int step = 0;
    readonly float blinkTime = 0.2f;
    float elapsed;

    TextInformation textInfo;

    GameObject leftSelect;
    GameObject rightSelect;
    GameObject steak;
    GameObject fries;
    GameObject plate;
    GameObject saucepan;
    GameObject ramekin;

    // Start is called before the first frame update
    void Start()
    {
        textInfo = GameObject.Find("ObjectInformation").GetComponent<TextInformation>();
        saucepan = GameObject.Find("Sauce pan (1)");
        plate = GameObject.Find("Prop_Plate_02 (1)");
        ramekin = GameObject.Find("Ramekin (4)");
        ramekin.GetComponent<Renderer>().material.color = Color.yellow;

        highlightObjs = new ArrayList();
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Fridge_02_p03"),
                                             GameObject.Find("Tray of steaks") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Salt Shaker"),
                                             GameObject.Find("Pepper Variant") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Griller").transform.GetChild(5).GetChild(0).gameObject,
                                             GameObject.Find("Prop_Griller_p03") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Griller_p03") });
        highlightObjs.Add(new GameObject[] { plate });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Frier_p02") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("FryDispenser (1)"),
                                             GameObject.Find("FryDispenser (2)"),
                                             GameObject.Find("FryDispenser (3)"),
                                             GameObject.Find("FryDispenser Button") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Frier (1)"),
                                             GameObject.Find("Prop_Frier_p04") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Parsley Variant"),
                                             GameObject.Find("Salt Shaker (1)") });
        highlightObjs.Add(new GameObject[] { plate });
        highlightObjs.Add(new GameObject[] { saucepan, 
                                             GameObject.Find("Ladle 1").transform.GetChild(0).gameObject });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Heater_p04"),
                                             GameObject.Find("Prop_Heater") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Stove") });
        highlightObjs.Add(new GameObject[] { plate, ramekin });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_KitchenTable_01 (1)"),
                                             GameObject.Find("Prop_KitchenTable_01") });

        tutorialText = new string[] { 
            "Step 1: Cook a steak! Get a steak from the fridge.",
            "Step 2: Salt and pepper the steak. \n\nTIP: Salt and pepper come out from the top of the shakers.",
            "Step 3: Place the steak on the griller and turn knob to highest setting to sear the steak. \n\nTIP: Hovering over items with the left controller will give you more information about the item. Hover over the knob with the left controller to see the current setting.",
            "Step 4: Let the steak sear for 2 minutes, then turn knob to medium setting and cook until steak is at the desired doneness. \n\nTIP: The steak will visually change when it is correctly seared. Use the object information panel to see the doneness.",
            "Step 5: Let the steak rest for 3 minutes and plate it up. \n\nTIP: The rotisseur role is only in charge of cooking steaks. The saucier cooks the fries and bearnaise sauce.",
            "Step 6: Cook some fries! Grab a fry basket out of the frier.",
            "Step 7: Hold the fry basket under the fry dispenser and press the red button to get some fries. \n\nTIP: There is a cooldown for dispensing fries.",
            "Step 8: Place the fry basket back in the frier and turn on the frier to cook the fries. \n\nTIP: Some knobs only have 2 settings, some have 4.",
            "Step 9: Let the fries cook until they reach 190C or 219C for extra crispy. Then garnish with salt and parsley.",
            "Step 10: Plate it with the steak. \n\nTIP: When turning in an order, all the components have to be on the same plate.",
            "Step 11: Cook the bearnaise sauce! Grab a sauce pan and a ladle. \n\nTIP: There are additional ladles and ramekins in the drawers.",
            "Step 12: Sauce is located in the heater. Ladle 1.5 scoops of Bearnaise sauce into the sauce pan.",
            "Step 13: Heat the sauce over low heat until 60C.",
            "Step 14: Pour the sauce into a ramekin, then plate it with the steak and fries.",
            "Step 15: To serve the dish, place it on the flashing tables. \n\nTIP: This is the head chef's role. Once the order ticket is placed on the table, all the dishes on the table will be served.",
            "Congratulations! You have successfully cooked steak frites!"
        };

        startColor = new ArrayList();
        foreach (var obj in (GameObject[])highlightObjs[step])
        {
            startColor.Add(obj.GetComponent<Renderer>().material.color);
        }

        elapsed = blinkTime;
        textInfo.TutorialText("Steak Frites Tutorial\n" + tutorialText[step]);

    }

    // Update is called once per frame
    void Update()
    {
        if (step < highlightObjs.Count)
        {
            Blink((GameObject[])highlightObjs[step]);

            if ((step == 0 && IsHolding("steak", true)) ||
                (step == 1 && steak.GetComponent<Steak>().seasoning.salt > 0f && steak.GetComponent<Steak>().seasoning.pepper > 0f) ||
                (step == 2 && steak.GetComponent<Steak>().searTime > 5) ||
                (step == 3 && steak.GetComponent<Steak>().searTime > 120 && IsHolding(steak.name)) ||
                (step == 4 && IsPlated(plate, "steak")) ||
                (step == 5 && IsHolding("fry basket", true)) ||
                (step == 6 && IsHolding("fry basket", true) && IsHolding("FryDispenser Button")) ||
                (step == 7 && IsHolding("Prop_Frier_p04")) || 
                (step == 9 && IsPlated(plate, "fry")) ||
                (step == 10 && IsHolding("Ladle 1") && IsHolding("Sauce pan (1)")) ||
                (step == 11 && saucepan.GetComponent<LiquidContainer>().currentVolume >= 100) ||
                (step == 12 && saucepan.GetComponent<LiquidContainer>().temperature.temp >= 60) ||
                (step == 13 && IsPlated(plate, "bearnaise")))
            {
                UpdateStep();
            }
            else if (step == 8)
            {
                if (fries == null)
                {
                    GameObject[] allFries = GameObject.FindGameObjectsWithTag("fry");
                    foreach (GameObject f in allFries)
                    {
                        if (f.GetComponent<Temperature>().heater != null)
                        {
                            fries = f;
                            break;
                        }
                    }
                }
                else if (fries.GetComponent<Fries>().seasoning.salt > 0f && fries.GetComponent<Fries>().seasoning.parsley > 0f)
                {
                    UpdateStep();
                }
            }
            else if (step == 14)
            {
                var table = GameObject.Find("Prop_KitchenTable_01 (1)");
                if (plate.transform.position.z <= table.transform.position.z + 0.5 && plate.transform.position.z >= table.transform.position.z - 0.5)
                {
                    UpdateStep();
                }
            }

        }


        /*
        string debugt = "right: " + (rightSelect != null ? rightSelect.name + ":" + rightSelect.tag + ":" : "null") + '\n' +
                        "left: " + (leftSelect != null ? leftSelect.name : "null") + '\n' +
                        "step = " + step;
        textInfo.DebugText(debugt);*/

    }

    void Blink(GameObject[] objs)
    {
        elapsed += Time.deltaTime;
        for (int i = 0; i < objs.Length; i++)
        {
            Color currentColor = Color.Lerp((Color)startColor[i], Color.yellow, Mathf.Cos(elapsed / blinkTime) * 0.7f + 0.3f);
            objs[i].GetComponent<Renderer>().material.color = currentColor;
        }
    }

    bool IsHolding(string item, bool isTag = false)
    {
        if (isTag)
        {
            bool leftTag = leftSelect != null && leftSelect.tag.Equals(item);
            bool rightTag = rightSelect != null && rightSelect.tag.Equals(item);
            return leftTag || rightTag;
        }

        bool leftName = leftSelect != null && leftSelect.name.Equals(item);
        bool rightName = rightSelect != null && rightSelect.name.Equals(item);
        return leftName || rightName;
    }

    bool IsPlated(GameObject p, string foodTag)
    {
        if (p == null || foodTag == null)
            return false;

        foreach (Transform child in p.transform)
        {
            if (child.tag.Equals(foodTag))
                return true;
        }
        return false;
    }

    void UpdateStep()
    {
        var objs = (GameObject[])highlightObjs[step];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i] == null)
                continue;

            objs[i].GetComponent<Renderer>().material.color = (Color)startColor[i];
        }

        step++;
        elapsed = blinkTime;
        startColor.Clear();
        if (step < highlightObjs.Count) 
        {
            foreach (var obj in (GameObject[])highlightObjs[step])
            {
                if (obj == null)
                {
                    startColor.Add(null);
                    continue;
                }
                startColor.Add(obj.GetComponent<Renderer>().material.color);
            }
        }

        if (step < tutorialText.Length) 
            textInfo.TutorialText("Steak Frites Tutorial\n" + tutorialText[step]);
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        var interactorObj = args.interactorObject.transform.gameObject;
        var interactableObj = args.interactableObject.transform.gameObject;
        bool isRight = interactorObj.name.Equals("RightHand Controller");
        bool isLeft = interactorObj.name.Equals("LeftHand Controller");
        if (isLeft)
        {
            leftSelect = interactableObj;
        } 
        else if (isRight)
        {
            rightSelect = interactableObj;
        }

        if (interactableObj.tag.Equals("steak") && (steak == null || steak.GetComponent<Steak>().seasoning.salt <= 0))
        {
            steak = interactableObj;
        } 
        else if (interactableObj.tag.Equals("fry") && (fries == null || (fries.GetComponent<Fries>().seasoning.salt <= 0f && fries.GetComponent<Fries>().seasoning.parsley <= 0f)))
        {
            fries = interactableObj;
        }
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        var interactorObj = args.interactorObject.transform.gameObject;
        var interactableObj = args.interactableObject.transform.gameObject;
        bool isRight = interactorObj.name.Equals("RightHand Controller");
        bool isLeft = interactorObj.name.Equals("LeftHand Controller");
        if (isLeft && leftSelect.Equals(interactableObj))
        {
            leftSelect = null;
        }
        else if (isRight && rightSelect.Equals(interactableObj))
        {
            rightSelect = null;
        }
    }
}
