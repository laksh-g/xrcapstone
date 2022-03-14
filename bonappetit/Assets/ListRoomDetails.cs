using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ListRoomDetails : MonoBehaviour
{
    public TextMeshProUGUI difficulty;
    public TextMeshProUGUI time;
    public TextMeshProUGUI TicketType;
    // Start is called before the first frame update
    void Start()
    {
        // if (PhotonNetwork.CurrentRoom != null) {
        //     ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        //     if(!ht.ContainsKey("difficulty")){
        //         ht["difficulty"] = "Beginner";
        //     }
        //     if(!ht.ContainsKey("time")){
        //         ht["time"] = "4:00";
        //     }
        //     if(!ht.ContainsKey("turnOption")){
        //         ht["turnOption"] = 0;
        //     }
        //     PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        // }  
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            if(!ht.ContainsKey("difficulty")){
                ht["difficulty"] = "Cafe Kitchen";
            }
            if(!ht.ContainsKey("time")){
                ht["time"] = "4:00";
            }
            if(!ht.ContainsKey("ticketOption")){
                ht["ticketOption"] = 0;
            }

            difficulty.text = "Map: " + (string)ht["difficulty"];
            time.text = "Time: " + (string)ht["time"] + " mins";
            int val = (int) ht["ticketOption"];
            if (val == 0){
                TicketType.text = "Ticket Type: Single";
            }else{
                TicketType.text = "Ticket Type: Multiple";
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
    }
}
