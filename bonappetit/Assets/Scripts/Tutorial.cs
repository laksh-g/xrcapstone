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

    // Start is called before the first frame update
    void Start()
    {
        textInfo = GameObject.Find("ObjectInformation").GetComponent<TextInformation>();

        highlightObjs = new ArrayList();
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Fridge_02_p03"),
                                             GameObject.Find("Tray of steaks") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Salt Shaker"),
                                             GameObject.Find("Pepper Variant") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Griller").transform.GetChild(5).GetChild(0).gameObject,
                                             GameObject.Find("Prop_Griller_p03") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Griller_p03") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Plate_02 (8)") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Frier_p02"),
                                             GameObject.Find("Prop_Frier_p03") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("FryDispenser (1)"),
                                             GameObject.Find("FryDispenser (2)"),
                                             GameObject.Find("FryDispenser (3)"),
                                             GameObject.Find("FryDispenser Button") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Prop_Frier (1)"),
                                             GameObject.Find("Prop_Frier_p04") });

        tutorialText = new string[] { 
            "Step 1: Cook a steak! Get a steak from the fridge.",
            "Step 2: Salt and pepper the steak.",
            "Step 3: Place the steak on the griller and turn knob to highest setting to sear the steak.",
            "Step 4: Let the steak sear for 2 minutes, then turn knob to medium setting and cook until steak is at the desired doneness.",
            "Step 5: Let the steak rest for 3 minutes and plate it up.",
            "Step 6: Cook some fries! Grab a fry basket out of the frier.",
            "Step 7: Hold the fry basket under the fry dispenser and press the red button to get some fries.",
            "Step 8: Place the fry basket back in the frier and turn the knob to cook the fries.",
            "Step 9: Let the fries cook until they reach 190C or 219C for extra crispy. Hover over the fries with left controller to check the state of the fries." 
        };

        startColor = new ArrayList();
        foreach (var obj in (GameObject[])highlightObjs[step])
        {
            startColor.Add(obj.GetComponent<Renderer>().material.color);
        }

        elapsed = blinkTime;
        textInfo.TutorialText(tutorialText[step]);
    }

    // Update is called once per frame
    void Update()
    {
        if (step < highlightObjs.Count)
        {
            Blink((GameObject[])highlightObjs[step]);

            if ((step == 0 && IsHolding("steak", true)) ||
                (step == 1 && steak.GetComponent<Steak>().seasoning.salt > 0f && steak.GetComponent<Steak>().seasoning.pepper > 0f) ||
                (step == 2 && steak.GetComponent<Steak>().searTime > 5))
            {
                UpdateStep();
            }
            else if (step == 3 && steak.GetComponent<Steak>().searTime > 120 && IsHolding(steak.name))
            {
                UpdateStep();
            }
            // or find a way to check if steak is on a plate
            else if (step == 4 && IsHolding("plate", true))
            {
                UpdateStep();
            } 
            else if (step == 5 && IsHolding("fry basket", true)) 
            {
                UpdateStep();
            }
            else if (step == 6 && IsHolding("fry basket", true) && IsHolding("FryDispenser Button"))
            {
                UpdateStep();
            }
            else if (step == 7 && IsHolding("Prop_Frier_p04"))
            {
                UpdateStep();
            }
        }

        string debugt = "right: " + (rightSelect != null ? rightSelect.name : "null") + '\n' +
                        "left: " + (leftSelect != null ? leftSelect.name : "null");
        textInfo.DebugText(debugt);

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

    void UpdateStep()
    {
        var objs = (GameObject[])highlightObjs[step];
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<Renderer>().material.color = (Color)startColor[i];
        }

        step++;
        elapsed = blinkTime;

        startColor.Clear();
        if (step < highlightObjs.Count) 
        {
            foreach (var obj in (GameObject[])highlightObjs[step])
            {
                startColor.Add(obj.GetComponent<Renderer>().material.color);
            }
        }

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
