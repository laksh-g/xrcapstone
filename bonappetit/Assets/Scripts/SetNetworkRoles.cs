using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SetNetworkRoles : MonoBehaviourPunCallbacks
{
    public GameObject roomUI;

    void Start()
    {
        //SetNickname()
    }

    public void SetNickname(string role)
    {
        roomUI.SetActive(true);
        PhotonNetwork.NickName = role;
        Debug.Log($"Set role to {PhotonNetwork.NickName}");
    }   
}
