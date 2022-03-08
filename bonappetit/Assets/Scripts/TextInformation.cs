using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInformation : MonoBehaviour
{
    private TextMeshProUGUI text;
    private GameObject selectedObject;
    private string tutorialText;
    private string debugText;
    readonly string defaultText = "Hover over an item for more info\nToggle with X";
    readonly Dictionary<string, List<string>> hookNames = new Dictionary<string, List<string>> { 
        { "Stone Plate", new List<string> { "Sauce", "Fries", "Steak" } },
        { "Soup Bowl", new List<string> { "Bread" } },
        { "Appetizer Plate", new List<string> { "Crab", "Crab2", "SproutLoc" } },
        { "Dinner Plate 2", new List<string> { "Vegetables", "Breast", "Wing2", "Wing1" } },
        { "Bread Board", new List<string> { "Bread", "Oil" } }
    };
    readonly List<string> liquidNames = new List<string> { "Sauce", "Soup" };

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "";
        if (tutorialText != null)
        {
            text.text += tutorialText + '\n' + '\n';
        }
        if (debugText != null)
        {
            text.text += debugText + '\n' + '\n';
        }
        if (selectedObject != null) 
        {
            UpdateText();
        }
        else
        {
            text.text += defaultText;
        }

    }

    public void UpdateSelected(GameObject obj)
    {
        if (obj.tag != null && obj.tag != "Untagged") 
        {
            selectedObject = obj;
        }
    }

    void UpdateText() 
    {
        //text.text += "Name: " + selectedObject.name + '\n';
        var desc = selectedObject.GetComponent<Description>();
        string tagStr = selectedObject.tag;
        if (desc == null)
        {
            text.text += "Item: " + char.ToUpper(tagStr[0]) + tagStr.Substring(1) + '\n';
        }
        else
        {
            text.text += "Item: " + desc.label + '\n';
        }

        if (desc != null)
        {
            text.text += "Description: " + desc.desc + '\n';
        }

        if (tagStr.Equals("plate") || ChildHasTag(selectedObject, "plate"))
        {
            string plateType = "Stone Plate";
            if (desc != null)
            {
                plateType = desc.label;
            }

            if (hookNames.TryGetValue(plateType, out List<string> hooks))
            {
                List<string> occupied = new List<string>();
                List<string> missing = new List<string>();

                var liquid = selectedObject.GetComponentInChildren<LiquidContainer>();
                bool hasLiquid = false;
                if (liquid != null && liquid.currentVolume > 0)
                {
                    hasLiquid = true;
                }

                var children = selectedObject.gameObject.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                {
                    if (hooks.Contains(child.name))
                    {
                        if (child.CompareTag("occupied"))
                        {
                            occupied.Add(child.name);
                        } 
                        else
                        {
                            missing.Add(child.name);
                        }
                    }
                    else if (liquidNames.Contains(child.name))
                    {
                        if (hasLiquid)
                        {
                            occupied.Add(child.name);
                        }
                        else
                        {
                            missing.Add(child.name);
                        }
                    }
                }

                text.text += "Currently contains components: " + String.Join(", ", occupied.ToArray()) + '\n';
                text.text += "Missing components: " + String.Join(", ", missing.ToArray()) + '\n';
            }
        }
        else
        {
            var temp = selectedObject.GetComponent<Temperature>();
            if (temp != null)
            {
                text.text += "Current temperature: " + temp.temp.ToString("F1") + " C" + '\n';
            }

            var cook = selectedObject.GetComponent<Cookable>();
            if (cook != null && cook.isSearable)
            {
                text.text += "Cook Status: " + cook.GetStatus() + '\n';
            }

            var steak = selectedObject.GetComponent<Steak>();
            if (steak != null)
            {
                text.text += "Sear Time: " + steak.searTime.ToString("F0") + " s" + '\n';
                text.text += "Rest time: " + steak.restTime.ToString("F0") + " s" + '\n';
                text.text += "Doneness: " + steak.GetDonenessLabel() + '\n';
                text.text += "Salt: " + steak.seasoning.salt.ToString("F2") + " g" + '\n';
                text.text += "Pepper: " + steak.seasoning.pepper.ToString("F2") + " g" + '\n';
            }

            var fries = selectedObject.GetComponent<Fries>();
            if (fries != null)
            {
                text.text += "Salt: " + fries.seasoning.salt.ToString("F2") + " g" + '\n';
                text.text += "Parsley: " + fries.seasoning.parsley.ToString("F2") + " g" + '\n';
            }

            var bearnaise = selectedObject.GetComponent<Bearnaise>();
            if (bearnaise != null)
            {
                text.text += "Status: " + (bearnaise.isSeparated ? "Separated\n" : "Good\n");
            }

            var knob = selectedObject.GetComponent<Knob>();
            if (knob != null)
            {
                text.text += "Setting: " + knob.getLabel() + '\n';
            }

            var liquidContainer = selectedObject.GetComponent<LiquidContainer>();
            if (liquidContainer != null)
            {
                //text.text += "Capacity: " + liquidContainer.capacity.ToString("F0") + " mL" + '\n';
                text.text += "Current volume: " + liquidContainer.currentVolume.ToString("F0") + " mL" + '\n';
            }
        }

    }

    public void DebugText(string txt)
    {
        debugText = txt;
    }

    public void TutorialText(string txt)
    {
        tutorialText = txt;
    }

    bool ChildHasTag(GameObject o, string t)
    {
        foreach (Transform child in o.transform)
        {
            if (child.tag.Equals(t))
                return true;
        }
        return false;
    }
}