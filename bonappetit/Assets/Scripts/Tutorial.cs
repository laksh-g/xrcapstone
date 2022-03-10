using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial : MonoBehaviour
{
    public TutorialType tutorialType;

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
    GameObject ramekin;
    GameObject saucePan;
    Dictionary<string, GameObject> plates;

    public enum TutorialType
    {
        steakFrites,
        frenchOnion,
        roastChicken,
        crabCakes,
        tableBread,
        headChef
    };

    // Start is called before the first frame update
    void Start()
    {
        textInfo = GameObject.Find("ObjectInformation").GetComponent<TextInformation>();
        plates = new Dictionary<string, GameObject>();

        highlightObjs = new List<GameObject[]>();

        if (tutorialType.Equals(TutorialType.steakFrites))
        {
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Fridge_02_p03"),
                GameObject.Find("Prop_Fridge_02_p02"),
                GameObject.Find("SteakSpawner") });
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
            highlightObjs.Add(new GameObject[] { null });
            highlightObjs.Add(new GameObject[] { GameObject.Find("RamekinSpawner"),
                GameObject.Find("Prop_KitchenTable_06"),
                GameObject.Find("Prop_KitchenTable_06").transform.Find("Prop_KitchenTable_06_p04").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Heater_p04") });
            highlightObjs.Add(new GameObject[] { null });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_KitchenTable_01") });

            tutorialText = new string[]
            {
                "Step 1: Cook a steak! Get a steak from the fridge. \n\nTIP: Press and hold the grip button to grab objects. Grab and pull the fridge handle to open the fridge.",
                "Step 2: Place the steak on the cutting board and season it with 7g of salt and 5g of pepper. \n\nTIP: Salt and pepper come out from the top of the shakers.",
                "Step 3: Place the steak on the griller and turn knob to highest setting to sear the steak. While pointing at a knob, press and release the grip button to turn the knob.",
                "Step 4: Let the steak sear for 30s, then turn knob to medium setting and cook until steak is at the desired doneness. \n\nTIP: Hover over the steak with the left controller to check its doneness in the object information panel.",
                "Step 5: Let the steak rest for 45s and plate it up. \n\nTIP: The rotisseur role is in charge of cooking steaks. The saucier role is in charge fries and bearnaise sauce.",
                "Step 6: Cook some fries! Grab a fry basket. \n\nTIP: The station for fries is on the other side of the kitchen",
                "Step 7: Grab some fries out of the kitchen cabinet and place them in the fry basket.",
                "Step 8: Place the fry basket back in the frier and click the knob to cook the fries. \n\nTIP: Some knobs are on a timer. Wait for the timer to finish for fully cooked fries!",
                "Step 9: When the timer goes off, pour the fries out of the fry basket and garnish with 4g of salt and 4g of parsley. \n\nTIP: Fries cannot be grabbed out of the fry basket. Instead, tilt the fry basket to pour the fries out.",
                "Step 10: Plate the fries with the steak. \n\nTIP: When turning in an order, all the components have to be on the same plate.",
                "Step 11: Plate the bearnaise sauce! Grab a ramekin and a ladle. \n\nTIP: Additional cooking tools can be found in drawers.",
                "Step 12: Sauce is located in the left side of the heater. Ladle ~150mL of bearnaise sauce into a ramekin.",
                "Step 13: Plate the bearnaise with the steak and fries.",
                "Step 14: To serve the dish, place it on the flashing table. \n\nTIP: Serving is the head chef's role.",
                "Congratulations!!! \nYou have successfully cooked steak frites! \n\nTIP: You can press the left menu button to return to the lobby."
            };

            header = "Steak Frites Tutorial\n";
        }
        else if (tutorialType.Equals(TutorialType.frenchOnion))
        {
            highlightObjs.Add(new GameObject[] { GameObject.Find("SoupBowlSpawner"),
                GameObject.Find("Prop_KitchenTable_06").transform.Find("Prop_KitchenTable_06_p04").gameObject,
                GameObject.Find("Prop_KitchenTable_06") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Heater_p05") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("SoupBreadSpawner"),
                GameObject.Find("Prop_KitchenCabinet_03") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("CheeseSpawner"),
                GameObject.Find("Prop_Fridge_02_p03"),
                GameObject.Find("Prop_Fridge_02_p02"),
                GameObject.Find("Prop_KitchenTable_06 (1)"),
                GameObject.Find("Prop_KitchenTable_06 (1)").transform.Find("Prop_KitchenTable_06_p02").gameObject });
            highlightObjs.Add(new GameObject[] { null });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Torch") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_KitchenTable_01") });

            tutorialText = new string[]
            {
                "Step 1: Let's cook some french onion soup! Grab a bowl from the dishes table and a ladle from the drawer in the right side of the kitchen. \n\nTIP: Press and hold the grip button to grab objects. Grab and pull the drawer handle to open the drawer.",
                "Step 2: Soup is located in the right side of the heater. Ladle the soup into the bowl until full, about 450mL.",
                "Step 3: Grab a piece of bread from the kitchen cabinet and drop it in the soup bowl.",
                "Step 4: Place the bowl down on the counter. Grab a grater from the drawer and cheese from the fridge on the other side of the kitchen.",
                "Step 5: Grate the cheese over the bowl of soup. To grate, hold the grater steady and gently rub the cheese along the top of the grater. Grate the cheese until it becomes visible in the soup. \n\nTIP: Grating the cheese gently is key.",
                "Step 6: Grab the torch and torch the bowl of french onion soup until the cheese melts. \n\nTIP: The cheese should visibly flatten when melted.",
                "Step 7: To serve the dish, place it on the flashing table.",
                "Congratulations!!! \nYou have successfully cooked french onion soup! \n\nTIP: You can press the left menu button to return to the lobby."
            };

            header = "French Onion Soup Tutorial\n";
        }
        // highlightObjs.Add(new GameObject[] { GameObject.Find() });
        else if (tutorialType.Equals(TutorialType.roastChicken))
        {
            highlightObjs.Add(new GameObject[] { GameObject.Find("RoastingPanSpawner") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("VegetableSpawner"),
                GameObject.Find("ChickenSpawner"),
                GameObject.Find("Prop_Fridge_02_p03"),
                GameObject.Find("Prop_Fridge_02_p02") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("BottleSpawner (1)").transform.Find("Olive Oil").gameObject,
                GameObject.Find("BottleSpawner (1)").transform.Find("Spice Box (1)").gameObject,
                GameObject.Find("SpiceSpawner (1)").transform.Find("Shaker (1)").gameObject,
                GameObject.Find("SpiceSpawner (1)").transform.Find("Pepper Variant").gameObject,
                GameObject.Find("SpiceSpawner (1)").transform.Find("Spice Box (1)").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Oven_01"),
                GameObject.Find("Prop_Oven_01").transform.Find("Prop_Oven_01_p02").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("DinnerPlateSpawner"),
                null });
            highlightObjs.Add(new GameObject[] { GameObject.Find("SpiceSpawner (2)").transform.Find("Spice Box (1)").gameObject,
                GameObject.Find("SpiceSpawner (2)").transform.Find("Parsley Variant").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("PanSpawner"),
                null });
            highlightObjs.Add(new GameObject[] { GameObject.Find("ShallotSpawner"),
                GameObject.Find("Prop_KitchenCabinet_03") });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Stove"),
                GameObject.Find("Prop_Stove").transform.Find("Knobs").gameObject });
            highlightObjs.Add(new GameObject[] { GameObject.Find("BottleSpawner").transform.Find("Spice Box (1)").gameObject,
                GameObject.Find("BottleSpawner").transform.Find("jarst_champagne").gameObject });
            highlightObjs.Add(new GameObject[] { null });
            highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_KitchenTable_01") });

            tutorialText = new string[]
            {
                "Step 1: Roast the chicken and vegetables! Grab a roasting pan. \n\nTIP: Press and hold the grip button to grab objects.",
                "Step 2: Add 1 chicken breast, 2 chicken wings, and an order of vegetables to the roasting pan. \n\nTIP: Grab and pull the fridge handle to open the fridge.",
                "Step 3: Season everything with salt and pepper, and add about 100mL of olive oil to the pan.",
                "Step 4: Place the roasting pan in the 425° oven and activate the chicken timer. \n\nTIP: Activate the timer by pressing the CHICKEN button.",
                "Step 5: Once the timer goes off, take the roasting pan out, pick up a dinner plate, and put the food on the dinner plate. Save the roasting liquid in the pan.",
                "Step 6: Garnish the plate with parsley. \n\nTIP: Hover over an object with the left controller to get more information.",
                "Step 7: Let's make the pan sauce! Take the roasting liquid from before and pour all of it to a sauce pan.",
                "Step 8: Add shallots to the pan. Shallots are located in the kitchen cabinet.",
                "Step 9: Using the stove, cook shallots over high heat until the shallots melt. Turn the knob on the stove to adjust the heat level. \n\nTIP: While pointing at a knob, press and release the grip button to turn the knob.",
                "Step 10: Reduce heat and add 150mL of chardonnay white wine to deglaze.",
                "Step 11: Pour completed sauce over the chicken and vegetables.",
                "Step 12: To serve the dish, place it on the flashing table.",
                "Congratulations!!! \nYou have successfully cooked roast chicken with summer vegetables! \n\nTIP: You can press the left menu button to return to the lobby."
            };

            header = "Roast Chicken with Summer Vegetables Tutorial\n";
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
            else
            {
                Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renders)
                {
                    startColor.Add(r.material.color);
                    currRenders.Add(r);
                }
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

            switch (tutorialType)
            {
                case TutorialType.steakFrites:
                    UpdateSteakFrites();
                    break;
                case TutorialType.frenchOnion:
                    UpdateFrenchOnion();
                    break;
                case TutorialType.roastChicken:
                    UpdateRoastChicken();
                    break;
                default:
                    textInfo.DebugText("Error: tutorialType default. Tutorial not found");
                    break;
            }

        }


        string debugt = "right: " + (rightSelect != null ? rightSelect.name + ":" + rightSelect.tag + ":" : "null") + '\n' +
                        "left: " + (leftSelect != null ? leftSelect.name : "null") + '\n' +
                        "step = " + step;
        textInfo.DebugText(debugt);
    }

    void UpdateSteakFrites()
    {
        bool hasPlate = plates.TryGetValue("Stone Plate", out GameObject plate);
        if ((step == 0 && IsHolding("steak", true)) ||
            (step == 1 && steak.GetComponent<Steak>().seasoning.salt > 0f && steak.GetComponent<Steak>().seasoning.pepper > 0f) ||
            (step == 2 && steak.GetComponent<Steak>().searTime > 10) ||
            (step == 3 && steak.GetComponent<Steak>().searTime > 30 && IsHolding(steak.name)) ||
            (step == 4 && IsPlated(plate, "Steak")) ||
            (step == 5 && IsHolding("fry basket", true)) ||
            (step == 6 && IsHolding("fry", true)) ||
            (step == 7 && IsHolding("Prop_Frier_p04")) ||
            (step == 8 && fries != null && fries.GetComponent<Fries>().seasoning.salt > 0f && fries.GetComponent<Fries>().seasoning.parsley > 0f) ||
            (step == 9 && IsPlated(plate, "Fries")) ||
            (step == 10 && IsHolding("ladle", true) && IsHolding("ramekin", true)) ||
            (step == 11 && ramekin.GetComponent<LiquidContainer>().currentVolume >= 140) ||
            (step == 12 && IsPlated(plate, "Sauce")))
        {
            UpdateStep();
        }
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
        bool hasPlate = plates.TryGetValue("Soup Bowl", out GameObject plate);
        if ((step == 0 && IsHolding("ladle", true) && IsHolding("Soup bowl")) ||
            (step == 1 && hasPlate && plate.TryGetComponent(out LiquidContainer liquid) && liquid.currentVolume >= 300) ||
            (step == 2 && hasPlate && IsPlated(plate, "Bread")) ||
            (step == 3 && IsHolding("grater", true) && IsHolding("gruyere", true)) ||
            (step == 4 && hasPlate && plate.TryGetComponent(out Seasonable seasonable) && seasonable.gruyere >= 5) ||
            (step == 5 && hasPlate && plate.GetComponentInChildren<Cheese>() != null && plate.GetComponentInChildren<Cheese>().toastingTime >= 10))
        {
            UpdateStep();
        }
        else if (step == 6)
        {
            var table = GameObject.Find("Prop_KitchenTable_01 (1)");
            if (plate.transform.position.z <= table.transform.position.z + 0.5 && plate.transform.position.z >= table.transform.position.z - 0.5 && !IsHolding("plate", true))
            {
                UpdateStep();
            }
        }
    }

    void UpdateRoastChicken()
    {
        bool hasPan = plates.TryGetValue("Roasting Pan", out GameObject pan);
        bool hasPlate = plates.TryGetValue("Dinner Plate 2", out GameObject plate);
        if ((step == 0 && IsHolding("Roasting Pan")) ||
            (step == 1 && IsPlated(pan, "Veggies") && IsPlated(pan, "Wing1") && IsPlated(pan, "Wing2") && IsPlated(pan, "Breast")) ||
            (step == 2 && pan.TryGetComponent(out LiquidContainer liquid) && liquid.currentVolume >= 100) ||
            (step == 3 && IsHolding("Button")) ||
            (step == 4 && IsPlated(plate, "Vegetables") && IsPlated(plate, "Wing1") && IsPlated(plate, "Wing2") && IsPlated(plate, "Breast")) ||
            (step == 5 && plate.GetComponentInChildren<Seasonable>() != null && plate.GetComponentInChildren<Seasonable>().parsley > 0) ||
            (step == 6 && saucePan != null && saucePan.TryGetComponent(out LiquidContainer liquid2) && liquid2.currentVolume >= 20) ||
            (step == 7 && IsPlated(saucePan, "Shallots")) ||
            (step == 8 && saucePan.TryGetComponent(out Temperature temp) && temp.temp >= 60) ||
            (step == 9 && saucePan.CompareTag("chardonnay")) ||
            (step == 10 && plate.TryGetComponent(out LiquidContainer liquid3) && liquid3.currentVolume >= 20))
        {
            UpdateStep();
        }
        else if (step == 11)
        {
            var table = GameObject.Find("Prop_KitchenTable_01 (1)");
            if (plate.transform.position.z <= table.transform.position.z + 0.5 && plate.transform.position.z >= table.transform.position.z - 0.5 && !IsHolding("plate", true))
            {
                UpdateStep();
            }
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

    // hook tag changes to occupied when item is plated. checks the given hook to see if item is plated.
    bool IsPlated(GameObject p, string hookName)
    {
        if (p == null || hookName == null)
        {
            return false;
        }

        Transform[] children = p.transform.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        { 
            if (child.name.Equals(hookName))
            {
                return child.CompareTag("occupied");
            }
        }
        return false;
    }

    bool HasCheese(GameObject p)
    {
        if (p != null && p.GetComponent<Seasonable>() != null)
        {
            return p.GetComponent<Seasonable>().gruyere >= 5;
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
                else
                {
                    Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
                    foreach (var r in renders)
                    {
                        startColor.Add(r.material.color);
                        currRenders.Add(r);
                    }
                }
            }
        }

        if (step < tutorialText.Length) 
            textInfo.TutorialText(header + tutorialText[step]);
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
            if (interactableObj.TryGetComponent(out Description desc))
            {
                if (plates.ContainsKey(desc.label))
                {
                    plates[desc.label] = interactableObj;
                }
                else
                {
                    plates.Add(desc.label, interactableObj);
                }

                UpdatePlate(interactableObj);
            }
        }
        else if (interactableObj.tag.Equals("ramekin"))
        {
            ramekin = interactableObj;
        }
        else if (interactableObj.name.Contains("Prop_Pan_02"))
        {
            saucePan = interactableObj;
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
        List<int> indices = new List<int>();
        switch (tutorialType)
        {
            case TutorialType.steakFrites:
                if (p.GetComponent<Description>().label.Equals("Stone Plate"))
                {
                    indices = new List<int> { 9, 12 };
                }
                break;
            case TutorialType.frenchOnion:
                if (p.GetComponent<Description>().label.Equals("Soup Bowl"))
                {
                    indices = new List<int> { 4 };
                }
                break;
            case TutorialType.roastChicken:
                if (p.GetComponent<Description>().label.Equals("Roasting Pan"))
                {
                    indices = new List<int> { 4, 6 };
                }
                else if (p.GetComponent<Description>().label.Equals("Dinner Plate 2"))
                {
                    indices = new List<int> { 10 };
                }
                break;
        }
        foreach (int i in indices)
        {
            highlightObjs[i][highlightObjs[i].Length - 1] = p;
            //highlightObjs[i] = new GameObject[] { p };
        }

    }
}
