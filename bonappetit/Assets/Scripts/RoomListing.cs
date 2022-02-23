using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _text;

    public RoomInfo RoomInfo {get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = "Room: " + roomInfo.Name;
    }

    public void JoinSelectedRoom()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

}
