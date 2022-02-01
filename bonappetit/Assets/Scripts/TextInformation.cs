using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class TextInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedObject != null) 
        {
            UpdateText();
        }

    }

    public void UpdateSelected(GameObject obj)
    {
        selectedObject = obj;
    }

    void UpdateText() 
    {
        text.text = "Item: " + selectedObject.name + '\n';
        var liquidContainer = selectedObject.GetComponent<LiquidContainer>();
        if (liquidContainer != null) 
        {
            text.text += "Capacity: " + liquidContainer.capacity + '\n';
            text.text += "Current Volume: " + liquidContainer.currentVolume + '\n';
        }
        var temp = selectedObject.GetComponent<Temperature>();
        if (temp != null) {
            text.text += "Temperature: " + temp.temp + '\n';
        }
    }

}