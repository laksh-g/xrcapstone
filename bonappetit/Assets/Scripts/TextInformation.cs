using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class TextInformation : MonoBehaviour
{
    private TextMeshProUGUI text;
    private GameObject selectedObject;
    private string tutorialText;
    private string debugText;
    private string defaultText;

    readonly Dictionary<string, List<string>> hookNames = new Dictionary<string, List<string>> 
    { 
        { "Stone Plate", new List<string> { "Sauce", "Fries", "Steak" } },
        { "Soup Bowl", new List<string> { "Bread" } },
        { "Appetizer Plate", new List<string> { "Crab", "Crab2", "SproutLoc" } },
        { "Dinner Plate 2", new List<string> { "Vegetables", "Breast", "Wing2", "Wing1" } },
        { "Bread Board", new List<string> { "Bread", "Oil" } },
        { "Roasting Pan", new List<string> { "Veggies", "Breast", "Wing2", "Wing1" } }
    };
    readonly List<string> liquidNames = new List<string> { "Sauce", "Soup" };
    readonly Dictionary<string, List<string>> foodSeasonings = new Dictionary<string, List<string>>
    {
        { "steak", new List<string> { "salt", "pepper" } },
        { "fry", new List<string> { "salt", "parsley" } },
        { "breast", new List<string> { "salt", "pepper", "parsley" } },
        { "wing", new List<string> { "salt", "pepper", "parsley" } },
        { "vegetables", new List<string> { "salt", "pepper", "parsley" } },
        { "french onion soup", new List<string> { "parsley" } }
    };

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        defaultText = text.text;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "";
        if (tutorialText != null)
        {
            text.text += tutorialText + "\n-------\n";
        }
        if (debugText != null)
        {
            text.text += debugText + "\n-------\n";
        }
        if (selectedObject != null) 
        {
            text.text += "Object Information:\n";
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
// Debug info

        var view = selectedObject.GetComponent<PhotonView>();
       /* if (view != null) {
            text.text += "Is my item: " + view.IsMine + "\n";
        }

        text.text += "Item layer: " + selectedObject.layer + "\n";
        text.text += "Item tag: " + selectedObject.tag + "\n";
      */  
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

        var temp = selectedObject.GetComponent<Temperature>();
        if (temp != null)
        {
            text.text += "Current temperature: " + temp.tempInF().ToString("F1") + "°F" + '\n';
        }

        var liquidContainer = selectedObject.GetComponent<LiquidContainer>();
        if (liquidContainer != null)
        {
            //text.text += "Capacity: " + liquidContainer.capacity.ToString("F0") + " mL" + '\n';
            text.text += "Current volume: " + liquidContainer.currentVolume.ToString("F0") + " mL" + '\n';
        }

        if (desc != null)
        {
            text.text += "Description: " + desc.desc + '\n';
        }

        if ((tagStr.Equals("plate") || ChildHasTag(selectedObject, "plate")) && desc != null)
        {
            string plateType = desc.label;

            if (hookNames.TryGetValue(plateType, out List<string> hooks))
            {
                List<string> occupied = new List<string>();
                List<string> missing = new List<string>();

                // missing liquids check (sauce or soup)
                if (!selectedObject.TryGetComponent(out LiquidContainer liquid))
                {
                    liquid = GetComponentInChildren<LiquidContainer>();
                }
                bool hasLiquid = false;
                if (liquid != null && liquid.currentVolume > 0)
                {
                    hasLiquid = true;
                }

                // missing hookable components check
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

                if (plateType.Equals("Soup Bowl") && selectedObject.TryGetComponent(out Seasonable s))
                {
                    if (s.gruyere < 5)
                    {
                        missing.Add("Shredded Cheese");
                    }
                }

                // text.text += "Currently contains components: " + String.Join(", ", occupied.ToArray()) + '\n';
                if (missing.Count > 0)
                {
                    text.text += "Missing components: " + String.Join(", ", missing.ToArray()) + '\n';
                }
            }

            // missing seasonings check
            // PhotonView view = selectedObject.GetComponent<PhotonView>();
            Dish d = PhotonView.Find(view.ViewID).GetComponent<Dish>();
            var items = d.connectedItems;
            items.Add(view.ViewID);
            foreach (int id in items)
            {
                GameObject target = PhotonView.Find(id).gameObject;
                if (foodSeasonings.TryGetValue(target.tag, out List<string> seasoning) && target.TryGetComponent(out Seasonable s))
                {
                    List<string> missingSeasoning = new List<string>();
                    foreach (var str in seasoning)
                    {
                        switch (str)
                        {
                            case "salt":
                                if (s.salt <= 0)
                                    missingSeasoning.Add("salt");
                                break;
                            case "pepper":
                                if (s.pepper <= 0)
                                    missingSeasoning.Add("pepper");
                                break;
                            case "parsley":
                                if (s.parsley <= 0)
                                    missingSeasoning.Add("parsley");
                                break;
                            case "gruyere":
                                if (s.gruyere <= 0)
                                    missingSeasoning.Add("gruyere");
                                break;
                        }
                    }
                    if (missingSeasoning.Count > 0)
                    {
                        // change tags to be more readable friendly
                        text.text += "Missing seasoning: " + char.ToUpper(s.tag[0]) + s.tag.Substring(1) + " is missing " + String.Join(", ", missingSeasoning.ToArray()) + ".\n";
                    }
                }
            }

            // cheese check
            if (plateType.Equals("Soup Bowl") && 
                selectedObject.TryGetComponent(out Seasonable seasonable) && seasonable.gruyere >= 5 && 
                selectedObject.GetComponentInChildren<Cheese>() != null && selectedObject.GetComponentInChildren<Cheese>().toastingTime < 10)
            {
                text.text += "Missing: Cheese needs to be torched.\n";
            }

        }
        else
        {
            /*
            var temp = selectedObject.GetComponent<Temperature>();
            if (temp != null)
            {
                text.text += "Current temperature: " + temp.temp.ToString("F1") + " C" + '\n';
            }*/

            var cook = selectedObject.GetComponent<Cookable>();
            if (cook != null)
            {
                text.text += "Cook Status: " + cook.GetStatus() + '\n';
                if (cook.isSearable)
                {
                    text.text += "Sear Status: " + cook.GetSearStatus() + '\n';
                    text.text += "Sear Time: " + cook.searTime.ToString("F0") + " s" + '\n';
                }
            }

            var steak = selectedObject.GetComponent<Steak>();
            if (steak != null)
            {
                text.text += "Sear Time: " + steak.searTime.ToString("F0") + " s" + '\n';
                text.text += "Rest Time: " + steak.restTime.ToString("F0") + " s" + '\n';
                text.text += "Doneness: " + steak.GetDonenessLabel() + '\n';
                // text.text += "Salt: " + steak.seasoning.salt.ToString("F1") + " g" + '\n';
                // text.text += "Pepper: " + steak.seasoning.pepper.ToString("F1") + " g" + '\n';
            }

            var fries = selectedObject.GetComponent<Fries>();
            if (fries != null)
            {
                // text.text += "Salt: " + fries.seasoning.salt.ToString("F1") + " g" + '\n';
                // text.text += "Parsley: " + fries.seasoning.parsley.ToString("F1") + " g" + '\n';
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

            var seasonable = selectedObject.GetComponent<Seasonable>();
            if (seasonable != null)
            {
                if (foodSeasonings.TryGetValue(selectedObject.tag, out List<string> seasoning))
                {
                    foreach (var str in seasoning)
                    {
                        switch (str)
                        {
                            case "salt":
                                text.text += "Salt: " + seasonable.salt.ToString("F1") + " g" + '\n';
                                break;
                            case "pepper":
                                text.text += "Pepper: " + seasonable.pepper.ToString("F1") + " g" + '\n';
                                break;
                            case "parsley":
                                text.text += "Parsley: " + seasonable.parsley.ToString("F1") + " g" + '\n';
                                break;
                            case "gruyere":
                                text.text += "Gruyere: " + seasonable.gruyere.ToString("F1") + " g" + '\n';
                                break;
                        }
                    }
                }
                else
                {
                    text.text += "Salt: " + seasonable.salt.ToString("F1") + " g" + '\n';
                    text.text += "Pepper: " + seasonable.pepper.ToString("F1") + " g" + '\n';
                    text.text += "Parsley: " + seasonable.parsley.ToString("F1") + " g" + '\n';
                    text.text += "Gruyere: " + seasonable.gruyere.ToString("F1") + " g" + '\n';
                }
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
