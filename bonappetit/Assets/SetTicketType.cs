using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SetTicketType : MonoBehaviour
{
    public Slider mainSlider;
    // Start is called before the first frame update
    void Start()
    {
        mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
    }

    public void ValueChangeCheck()
	{
         if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            ht["ticketOption"] = (int) mainSlider.value;
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
         }
	}
}
