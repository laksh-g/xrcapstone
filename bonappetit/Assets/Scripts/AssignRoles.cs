using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;

public class AssignRoles : MonoBehaviourPunCallbacks
{
    public Button HeadChefButton;
    public Button RotisseurButton;
    public Button SaucierButton;
    public Button SousButton;

    public Button StartGame;
    public Button RoomSettings;
    private bool buttonActivated;

    private PhotonView _view;

    [SerializeField]
    public Dictionary<int, int> RoleMap = new Dictionary<int, int>();

    // Start is called before the first frame update
    void Start()
    {
        StartGame.interactable = false;
        RoomSettings.interactable = false;
        buttonActivated = false;
        _view = GetComponent<PhotonView>();
        RoleMap[0] = -1;
        RoleMap[1] = -1;
        RoleMap[2] = -1;
        RoleMap[3] = -1;

        updateButtons();
        InvokeRepeating("UpdateLastButton", 0.1f, 5f);
    }

    void OnLeftRoom() {
        RoleMap[0] = -1;
        RoleMap[1] = -1;
        RoleMap[2] = -1;
        RoleMap[3] = -1;
        base.OnLeftRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private IEnumerator SetCustomProperties()
    // {
    //     yield return new WaitForSeconds(1.5f);
    // }

    public void UpdateLastButton(){
        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        if(!buttonActivated){
            if((string)ht["difficulty"] == "Fine Dining Kitchen"){
                SousButton.gameObject.SetActive(true);
                buttonActivated = true;
            }else{
                SousButton.gameObject.SetActive(false);
            }
        }
    }

    public void updateButtons(){
        if (PhotonNetwork.CurrentRoom != null) {
            
            if(RoleMap[0] != -1){
                RotisseurButton.interactable = false;
                RotisseurButton.GetComponentInChildren<TMP_Text>().text = (RoleMap[0]).ToString();
            } else {
                RotisseurButton.interactable = true;
                RotisseurButton.GetComponentInChildren<TMP_Text>().text = "Rotisseur";
            }
            if(RoleMap[1] != -1){
                SaucierButton.interactable = false;
                SaucierButton.GetComponentInChildren<TMP_Text>().text = (RoleMap[1]).ToString();
            } else {
                SaucierButton.interactable = true;
                SaucierButton.GetComponentInChildren<TMP_Text>().text = "Saucier";
            }
            if(RoleMap[2] != -1){
                HeadChefButton.interactable = false;
                HeadChefButton.GetComponentInChildren<TMP_Text>().text = (RoleMap[2]).ToString();
            } else {
                HeadChefButton.interactable = true;
                HeadChefButton.GetComponentInChildren<TMP_Text>().text = "Head Chef";
            }
            if(RoleMap[3] != -1){
                SousButton.interactable = false;
                SousButton.GetComponentInChildren<TMP_Text>().text = (RoleMap[3]).ToString();
            } else {
                SousButton.interactable = true;
                SousButton.GetComponentInChildren<TMP_Text>().text = "Sous Chef";
            }
        }
    }

    public void SetRole(string role)
    {
        Debug.Log("Selected role" + role);
        if (PhotonNetwork.CurrentRoom != null) {
            if(role == "DefaultRole"){
                _view.RPC("SendRoleUpdates", RpcTarget.AllBufferedViaServer, -1, PhotonNetwork.LocalPlayer.ActorNumber);
            }else{
                string[] roles = {"RotisseurRole", "SaucierRole", "HeadChefRole", "SousChefRole"};

                int index = Array.IndexOf(roles, role);


                _view.RPC("SendRoleUpdates", RpcTarget.AllBufferedViaServer, index, PhotonNetwork.LocalPlayer.ActorNumber);


                ExitGames.Client.Photon.Hashtable playerCustomProps = PhotonNetwork.LocalPlayer.CustomProperties;
                playerCustomProps["role"] = role;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProps);

                if(role == "HeadChefRole"){
                    StartGame.interactable = true;
                    RoomSettings.interactable = true;
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                }else{
                    StartGame.interactable = false;
                    RoomSettings.interactable = false;
                    //StartGame.interactable = true;
                }
            }
        }
    }

    [PunRPC]
    public void SendRoleUpdates(int roleID, int playerID) {
        List<int> updates = new List<int>();
        foreach(int role in RoleMap.Keys){
            if(RoleMap[role] == playerID){
                updates.Add(role);
                //RoleMap[role] = -1;
            }
        }

        foreach(int up in updates){
            RoleMap[up] = -1;
        }

        if(roleID != -1){
            if(RoleMap[roleID] == -1){
                RoleMap[roleID] = playerID;
            }
        }
        updateButtons();
    }
}
