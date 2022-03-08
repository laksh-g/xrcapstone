using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class SelectFoodDisplay : XRSimpleInteractable
{
    private bool selected;
    [SerializeField]
    public int itemId;
    // Start is called before the first frame update
    void Start()
    {
        selected = false;
    }

    override protected void OnSelectEntering(SelectEnterEventArgs interactor)
    {
        selected = !selected;
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            // Set correct property
            //ht["ticketOption"] = (int) mainSlider.value;
            if(ht.ContainsKey("FoodDisplay")){
                if(selected){
                    ht["FoodDisplay"] = ((string)ht["FoodDisplay"]).Remove(itemId, 1).Insert(itemId,"0");
                }else{
                    ht["FoodDisplay"] = ((string)ht["FoodDisplay"]).Remove(itemId, 1).Insert(itemId,"1");
                }
                Debug.Log("Updated string to" + ht["FoodDisplay"]);
                PhotonNetwork.CurrentRoom.SetCustomProperties(ht);   
            }
        }
        base.OnSelectEntering(interactor);
    }
}
