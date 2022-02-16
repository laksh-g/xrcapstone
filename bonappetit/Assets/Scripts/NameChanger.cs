using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NameChanger : MonoBehaviour
{

    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSelectEntered()
    {
        //overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        TouchScreenKeyboard.Open("");
    }

    public void onSave()
    {
        if (overlayKeyboard != null)
        {
            inputText = overlayKeyboard.text; 
        } else {
            inputText = "Player";
        }
        PhotonNetwork.NickName =  inputText;
    }
}
