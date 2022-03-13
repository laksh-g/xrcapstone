using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ToggleTurning : MonoBehaviour
{
    public Slider mainSlider;
    // Start is called before the first frame update
    void Start()
    {
        mainSlider.value = PlayerPrefs.GetFloat("TurnSetting");
        mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
    }

    public void ValueChangeCheck()
	{
        ExitGames.Client.Photon.Hashtable playerCustomProps = PhotonNetwork.LocalPlayer.CustomProperties;
        playerCustomProps["turnOption"] = (int) mainSlider.value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProps);
        PlayerPrefs.SetFloat("TurnSetting", mainSlider.value);
	}
}
