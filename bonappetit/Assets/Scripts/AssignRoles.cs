using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class AssignRoles : MonoBehaviourPunCallbacks
{
    public Button HeadChefButton;
    public Button RotisseurButton;
    public Button SaucierButton;

    public Button StartGame;


    // Start is called before the first frame update
    void Start()
    {
        StartGame.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            if(ht.ContainsKey("RotisseurRole") && (int)ht["RotisseurRole"] != -1){
                RotisseurButton.interactable = false;
                RotisseurButton.GetComponentInChildren<TMP_Text>().text = ((int)ht["RotisseurRole"]).ToString();
            } else {
                RotisseurButton.interactable = true;
                RotisseurButton.GetComponentInChildren<TMP_Text>().text = "Rotisseur";
            }
            if(ht.ContainsKey("SaucierRole") && (int)ht["SaucierRole"] != -1){
                SaucierButton.interactable = false;
                SaucierButton.GetComponentInChildren<TMP_Text>().text = ((int)ht["SaucierRole"]).ToString();
            } else {
                SaucierButton.interactable = true;
                SaucierButton.GetComponentInChildren<TMP_Text>().text = "Saucier";
            }
            if(ht.ContainsKey("HeadChefRole") && (int)ht["HeadChefRole"] != -1){
                HeadChefButton.interactable = false;
                HeadChefButton.GetComponentInChildren<TMP_Text>().text = ((int)ht["HeadChefRole"]).ToString();
            } else {
                HeadChefButton.interactable = true;
                HeadChefButton.GetComponentInChildren<TMP_Text>().text = "Head Chef";
            }
        }
    }

    private IEnumerator SetCustomProperties()
    {
        yield return new WaitForSeconds(1);
    }

    public void SetRole(string role)
    {
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            ht[role] = PhotonNetwork.LocalPlayer.ActorNumber;
        
            string[] roles = {"RotisseurRole", "SaucierRole", "HeadChefRole"};
            
            foreach(string name in roles)
            {
                if(name != role) {
                    if(ht.ContainsKey(name) && (int) ht[name] == PhotonNetwork.LocalPlayer.ActorNumber) {
                        Debug.Log(string.Format("Removed role {0} with player actor number {1}.", name, ht[name]));
                        //ht.Remove(name);
                        ht[name] = -1;
                    }
                }
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

            ExitGames.Client.Photon.Hashtable playerCustomProps = PhotonNetwork.LocalPlayer.CustomProperties;
            playerCustomProps["role"] = role;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProps);

            if(role == "HeadChefRole"){
                StartGame.interactable = true;
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            }else{
                StartGame.interactable = false;
                //StartGame.interactable = true;
            }
        }
    }
}
