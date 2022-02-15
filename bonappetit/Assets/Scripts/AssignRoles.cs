using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class AssignRoles : MonoBehaviourPunCallbacks
{
    public Button HeadChefButton;
    public Button RotisseurButton;
    public Button SaucierButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            if(ht.ContainsKey("RotisseurRole")){
                RotisseurButton.interactable = false;
            } else {
                RotisseurButton.interactable = true;
            }
            if(ht.ContainsKey("SaucierRole")){
                SaucierButton.interactable = false;
            } else {
                SaucierButton.interactable = true;
            }
            if(ht.ContainsKey("HeadChefRole")){
                HeadChefButton.interactable = false;
            } else {
                HeadChefButton.interactable = true;
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
                        ht.Remove(name);
                    }
                }
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

            ExitGames.Client.Photon.Hashtable playerCustomProps = PhotonNetwork.LocalPlayer.CustomProperties;
            playerCustomProps["role"] = role;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProps);
        }
    }
}
