using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;



public class RoomMenuSelector : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public string MenuType;
    [SerializeField]
    public TextMeshProUGUI label;

    private int index;
    private List<string> list;
    private int maxIdx;
    private Dictionary <string,List<string>> Menu;

    ExitGames.Client.Photon.Hashtable ht;

    // Start is called before the first frame update
    void Start()
    {
        Menu = new Dictionary <string,List<string>>();
        Menu.Add("difficulty", new List<string>() {"Cafe Kitchen", "Restaurant Kitchen", "Fine Dining Kitchen"});
        Menu.Add("time", new List<string>() {"4:00", "6:00", "8:00", "10:00"});
        index = 0;
        list = Menu[MenuType];
        maxIdx = list.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        label.text = "<b>"+ list[index] +"</b> ";
        //update the UI
    }

    public void increment()
    {
        if(index < maxIdx){
            index = index + 1;
        }
        // if (PhotonNetwork.CurrentRoom != null) {
        //     ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        //     ht[MenuType] = list[index];
        // }
    }

    public void decrement()
    {
        if(index>0){
            index = index - 1;
        }

        // if (PhotonNetwork.CurrentRoom != null) {
        //     ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        //     ht[MenuType] = list[index];
        // }
    }

    public void updateProperties(){
        if (PhotonNetwork.CurrentRoom != null) {
            ht = PhotonNetwork.CurrentRoom.CustomProperties;
            ht[MenuType] = list[index];
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
    }

    public void openRoom(){
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }
}
