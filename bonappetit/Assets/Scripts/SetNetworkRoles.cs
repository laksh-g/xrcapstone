using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class SetNetworkRoles : MonoBehaviourPunCallbacks
{
    //public NetworkManager nm;
    public GameObject roomsUI;
    private int roomIndex;
    void Start()
    {
        //SetNickname()
    }

    // public void setRoomIndex(int index)
    // {
    //     roomIndex = index;
    //     selectRoles.SetActive(true);
    // }

    public void SetNickname(string role)
    {   
        // DefaultRoom roomSettings = nm.getRoomSettings(roomIndex);
        // PhotonNetwork.LoadLevel(roomSettings.sceneIndex);
        // nm.InitializeRoom(roomIndex);
        PhotonNetwork.NickName = role;
        if(role == "Head Chef"){
            Debug.Log("Assigned Master Client");
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
        roomsUI.SetActive(true);
        Debug.Log($"Set role to {PhotonNetwork.NickName}");
    }   
}
