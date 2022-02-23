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
        string tagStr = selectedObject.tag;
        text.text += "Item: " + char.ToUpper(tagStr[0]) + tagStr.Substring(1) + '\n';

        var temp = selectedObject.GetComponent<Temperature>();
        if (temp != null) {
            text.text += "Current temperature: " + temp.temp.ToString("F1") + " C" + '\n';
        }

        var cook = selectedObject.GetComponent<Cookable>();
        if (cook != null) {
            text.text += "Cook Status: " + cook.GetStatus() + '\n';
        }

        var steak = selectedObject.GetComponent<Steak>();
        if (steak != null) {
            text.text += "Sear Time: " + steak.searTime.ToString("F0") + " s" + '\n';
            text.text += "Rest time: " + steak.restTime.ToString("F0") + " s" + '\n';
            text.text += "Doneness: " + steak.GetDonenessLabel() + '\n';
            text.text += "Salt: " + steak.seasoning.salt.ToString("F2") + " g" + '\n';
            text.text += "Pepper: " + steak.seasoning.pepper.ToString("F2") + " g" + '\n';
        }

        var fries = selectedObject.GetComponent<Fries>();
        if (fries != null) {
            text.text += "Salt: " + fries.seasoning.salt.ToString("F2") + " g" + '\n';
            text.text += "Parsley: " + fries.seasoning.parsley.ToString("F2") + " g" + '\n';
        }

        var bearnaise = selectedObject.GetComponent<Bearnaise>();
        if (bearnaise != null) {
            text.text += "Status: " + (bearnaise.isSeparated ? "Separated\n" : "Good\n");
        }

        var knob = selectedObject.GetComponent<Knob>();
        if (knob != null) {
            text.text += "Setting: " + knob.getLabel() + '\n';
        }

        var liquidContainer = selectedObject.GetComponent<LiquidContainer>();
        if (liquidContainer != null) {
            //text.text += "Capacity: " + liquidContainer.capacity.ToString("F0") + " mL" + '\n';
            text.text += "Current volume: " + liquidContainer.currentVolume.ToString("F0") + " mL" + '\n';
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
}