using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System;

public class SelectTurnProvider : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject XROrigin;
    void Start()
    {
        var continuousTurn = XROrigin.GetComponent<ActionBasedContinuousTurnProvider>();
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable playerCustomProps = PhotonNetwork.LocalPlayer.CustomProperties;
            float turnOption = PlayerPrefs.GetFloat("TurnSetting");
            int val = (int) turnOption;
            Debug.Log("slider value: " + val);
            if(val == 1){
                continuousTurn.enabled = false;
            }else{
                continuousTurn.enabled = true;
            }
        }
    }
}
