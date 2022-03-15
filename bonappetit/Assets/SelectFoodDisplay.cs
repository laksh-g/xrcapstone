using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
public class SelectFoodDisplay : XRSimpleInteractable
{
    private bool selected;
    [SerializeField]
    public int itemId;

    List<Color> startColor;
    List<Renderer> currRenders;

    //public GameObject currObj;
    // Start is called before the first frame update
    void Start()
    {
        startColor = new List<Color>();
        currRenders = new List<Renderer>();
        selected = false;
        // Renderer[] renders = currObj.GetComponentsInChildren<Renderer>();
        // foreach (Renderer r in renders)
        // {
        //     startColor.Add(r.material.color);
        //     currRenders.Add(r);
        // }
    }

    override protected void OnSelectEntering(SelectEnterEventArgs interactor)
    {
        selected = !selected;
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.LocalPlayer.IsMasterClient) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            // Set correct property
            //ht["ticketOption"] = (int) mainSlider.value;
            if(ht.ContainsKey("FoodDisplay")){
                if(selected){
                    ht["FoodDisplay"] = ((string)ht["FoodDisplay"]).Remove(itemId, 1).Insert(itemId,"1");
                }else{
                    ht["FoodDisplay"] = ((string)ht["FoodDisplay"]).Remove(itemId, 1).Insert(itemId,"0");
                }
                Debug.Log("Updated string to" + ht["FoodDisplay"]);
                PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
            }
        }

        // if (selected){
        //     for (int i = 0; i < startColor.Count; i++){
        //             Color currentColor = Color.Lerp(startColor[i], Color.yellow, Mathf.Cos(elapsed / blinkTime) * 0.7f + 0.3f);
        //             currRenders[i].material.color = currentColor;
        //             //currRenders[idx][i].material = highlightMaterial;
        //         }
        // }
        base.OnSelectEntering(interactor);
    }
}
