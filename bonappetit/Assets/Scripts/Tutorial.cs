using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial : MonoBehaviour
{
    public TutorialType tutType;

    List<GameObject[]> highlightObjs;
    List<Color> startColor;
    List<Renderer> currRenders;
    string[] tutorialText;
    string header;

    int step = 0;
    readonly float blinkTime = 0.2f;
    float elapsed;

    TextInformation textInfo;

    GameObject leftSelect;
    GameObject rightSelect;
    GameObject steak;
    GameObject fries;
    GameObject plate;
    GameObject ramekin;

    public enum TutorialType
    {
        steakFrites,
        frenchOnion,
        headChef
    };

    // Start is called before the first frame update
    void Start()
    {
        textInfo = GameObject.Find("ObjectInformation").GetComponent<TextInformation>();

        highlightObjs = new List<GameObject[]>();

        if (tutType.Equals(TutorialType.steakFrites))
        {
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Fridge_02_p03"),
                                                 GameObject.Find("Prop_Fridge_02_p02"),
                                                 GameObject.Find("SteakSpawner").transform.Find("Prop_Tray (1)").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("SpiceSpawner (1)").transform.Find("Shaker (1)").gameObject,
                                                 GameObject.Find("SpiceSpawner (1)").transform.Find("Pepper Variant").gameObject,
                                                 GameObject.Find("SpiceSpawner (1)").transform.Find("Spice Box (1)").gameObject,
                                                 GameObject.Find("CuttingBoard") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Griller").transform.Find("Hitbox").gameObject,
                                                 GameObject.Find("Prop_Griller_p03") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Griller_p03") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("StonePlateSpawner") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("BasketSpawner (1)") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("FrySpawner"),
                                                 GameObject.Find("Prop_KitchenCabinet_03") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Frier (1)") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("SpiceSpawner").transform.Find("Shaker (1)").gameObject,
                                                 GameObject.Find("SpiceSpawner").transform.Find("Parsley Variant").gameObject,
                                                 GameObject.Find("SpiceSpawner").transform.Find("Spice Box (1)").gameObject,
                                                 GameObject.Find("CuttingBoard (2)") });
            highlightObjs.Add(new GameObject[] { plate });
            highlightObjs.Add(new GameObject[] { GameObject.Find("RamekinSpawner"),
                                                 GameObject.Find("Prop_KitchenTable_06").transform.Find("Prop_KitchenTable_06_p04").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Heater_p04") });
            highlightObjs.Add(new GameObject[] { plate });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_KitchenTable_01") });

            tutorialText = new string[] 
            {
                "Step 1: Cook a steak! Get a steak from the fridge. \n\nTIP: Press and hold the grip button to grab objects. Grab and pull the fridge handle to open the fridge.",
                "Step 2: Place the steak on the cutting board and season it with 7g of salt and 5g of pepper. \n\nTIP: Salt and pepper come out from the top of the shakers.",
                "Step 3: Place the steak on the griller and turn knob to highest setting to sear the steak. Press and release the grip button to turn the knob. \n\nTIP: Hovering over items with the left controller will give you information about the item. Hover over the knob with the left controller to see the current setting.",
                "Step 4: Let the steak sear for 120s, then turn knob to medium setting and cook until steak is at the desired doneness. \n\nTIP: Hover over the steak with the left controller to check its doneness in the object information panel.",
                "Step 5: Let the steak rest for 180s and plate it up. \n\nTIP: The rotisseur role is in charge of cooking steaks. The saucier role is in charge fries and bearnaise sauce.",
                "Step 6: Cook some fries! Grab a fry basket. \n\nTIP: The station for fries is on the other side of the kitchen",
                "Step 7: Grab some fries out of the kitchen cabinet and place them in the fry basket.",
                "Step 8: Place the fry basket back in the frier and click the knob to cook the fries. \n\nTIP: Some knobs are on a timer. Wait for the timer to finish for fully cooked fries!",
                "Step 9: When the timer goes off, pour the fries out of the fry basket and garnish with 4g of salt and 4g of parsley. \n\nTIP: Fries cannot be grabbed out of the fry basket. Instead, tilt the fry basket to pour the fries out.",
                "Step 10: Plate the fries with the steak. \n\nTIP: When turning in an order, all the components have to be on the same plate.",
                "Step 11: Plate the bearnaise sauce! Grab a ramekin and a ladle. \n\nTIP: Additional cooking tools can be found in drawers.",
                "Step 12: Sauce is located in the heater. Ladle 1 scoop of bearnaise sauce into a ramekin.",
                "Step 13: Plate the bearnaise with the steak and fries.",
                "Step 14: To serve the dish, place it on the flashing tables. \n\nTIP: Serving is the head chef's role.",
                "Congratulations!!! \nYou have successfully cooked steak frites! \n\nTIP: You can press the left menu button to return to the lobby."
            };

            header = "Steak Frites Tutorial\n";
        }
        else if (tutType.Equals(TutorialType.frenchOnion))
        {
            highlightObjs.Add(new GameObject[] { GameObject.Find("SoupBowlSpawner"),
                GameObject.Find("Prop_KitchenTable_06").transform.Find("Prop_KitchenTable_06_p04").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Heater_p05") });

            tutorialText = new string[]
            {
                "Step 1: Grab a bowl and a ladle. Ladles are located in drawers. \n\nTIP: Press and hold the grip button to grab objects. Grab and pull the drawer handle to open the drawer.",
                "Step 2: Soup is located in the heater. Ladle the soup into the bowl until full."
            };

            header = "French Onion Soup Tutorial\n";
        }
        else
        {
            tutorialText = new string[] { "Press the left menu button to return to the lobby." };
            header = "No tutorial available\n";
        }

        startColor = new List<Color>();
        currRenders = new List<Renderer>();
        foreach (var obj in highlightObjs[step])
        {
            if (obj == null)
                continue;

            if (obj.GetComponent<Renderer>() != null)
            {
                startColor.Add(obj.GetComponent<Renderer>().material.color);
                currRenders.Add(obj.GetComponent<Renderer>());
            }

            Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renders)
            {
                startColor.Add(r.material.color);
                currRenders.Add(r);
            }
        }
        
        elapsed = blinkTime;
        textInfo.TutorialText(header + tutorialText[step]);
    }

    // Update is called once per frame
    void Update()
    {
        if (step < highlightObjs.Count)
        {
            Blink();

            switch (tutType)
            {
                case TutorialType.steakFrites:
                    UpdateSteakFrites();
                    break;
                case TutorialType.frenchOnion:
                    UpdateFrenchOnion();
                    break;
                default:
                    textInfo.DebugText("Error: tutType default");
                    break;
            }

        }

        /*
        string debugt = "right: " + (rightSelect != null ? rightSelect.name + ":" + rightSelect.tag + ":" : "null") + '\n' +
                        "left: " + (leftSelect != null ? leftSelect.name : "null") + '\n' +
                        "step = " + step;
        textInfo.DebugText(debugt);*/
    }

    void UpdateSteakFrites()
    {
        if ((step == 0 && IsHolding("steak", true)) ||
            (step == 1 && steak.GetComponent<Steak>().seasoning.salt > 0f && steak.GetComponent<Steak>().seasoning.pepper > 0f) ||
            (step == 2 && steak.GetComponent<Steak>().searTime > 60) ||
            (step == 3 && steak.GetComponent<Steak>().searTime > 120 && IsHolding(steak.name)) ||
            (step == 4 && IsPlated(plate, "steak")) ||
            (step == 5 && IsHolding("fry basket", true)) ||
            (step == 6 && IsHolding("fry", true)) ||
            (step == 7 && IsHolding("Prop_Frier_p04")) ||
            (step == 8 && fries != null && fries.GetComponent<Fries>().seasoning.salt > 0f && fries.GetComponent<Fries>().seasoning.parsley > 0f) ||
            (step == 9 && IsPlated(plate, "fry")) ||
            (step == 10 && IsHolding("ladle", true) && IsHolding("ramekin", true)) ||
            (step == 11 && ramekin.GetComponent<LiquidContainer>().currentVolume >= 80) ||
            (step == 12 && IsPlated(plate, "bearnaise")))
        {
            UpdateStep();
        }
        /*
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
        }*/
        else if (step == 13)
        {
            var table = GameObject.Find("Prop_KitchenTable_01 (1)");
            if (plate.transform.position.z <= table.transform.position.z + 0.5 && plate.transform.position.z >= table.transform.position.z - 0.5 && !IsHolding("plate", true))
            {
                UpdateStep();
            }
        }
    }

    void UpdateFrenchOnion()
    {
        if ((step == 0 && IsHolding("ladle", true) && IsHolding("Soup bowl")) ||
            (step == 1 && plate.GetComponent<LiquidContainer>().currentVolume >= 300))
        {
            UpdateStep();
        }
    }

    void Blink()
    {
        elapsed += Time.deltaTime;

        for (int i = 0; i < startColor.Count; i++)
        {
            Color currentColor = Color.Lerp(startColor[i], Color.yellow, Mathf.Cos(elapsed / blinkTime) * 0.7f + 0.3f);
            currRenders[i].material.color = currentColor;
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

        bool leftName = leftSelect != null && leftSelect.name.Contains(item);
        bool rightName = rightSelect != null && rightSelect.name.Contains(item);
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
        for (int i = 0; i < startColor.Count; i++)
        {
            currRenders[i].material.color = startColor[i];
        }

        step++;
        elapsed = blinkTime;
        startColor.Clear();
        currRenders.Clear();

        if (step < highlightObjs.Count) 
        {
            foreach (var obj in highlightObjs[step])
            {
                if (obj == null)
                    continue;

                if (obj.GetComponent<Renderer>() != null)
                {
                    startColor.Add(obj.GetComponent<Renderer>().material.color);
                    currRenders.Add(obj.GetComponent<Renderer>());
                }

                Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
                foreach (var r in renders)
                {
                    startColor.Add(r.material.color);
                    currRenders.Add(r);
                }
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

        if (interactableObj.tag.Equals("steak"))
        {
            steak = interactableObj;
        }
        else if (interactableObj.tag.Equals("fry") && (fries == null || (fries.GetComponent<Fries>().seasoning.salt <= 0f && fries.GetComponent<Fries>().seasoning.parsley <= 0f)))
        {
            fries = interactableObj;
        }
        else if (interactableObj.tag.Equals("plate"))
        {
            plate = interactableObj;
            UpdatePlate(plate);
        }
        else if (interactableObj.tag.Equals("ramekin"))
        {
            ramekin = interactableObj;
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

    public void UpdatePlate(GameObject p)
    {
        highlightObjs[9] = new GameObject[] { plate };
        highlightObjs[12] = new GameObject[] { plate };
    }
}
