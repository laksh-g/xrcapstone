using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListings : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _text;

    public Player Player {get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = player.NickName;
    }

    // public void JoinSelectedRoom()
    // {
    //     PhotonNetwork.JoinRoom(RoomInfo.Name);
    // }

}
