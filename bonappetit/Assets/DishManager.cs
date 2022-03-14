using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DishManager : MonoBehaviour
{
    public Transform steakFrites;
    public Transform tableBread;
    public Transform onionSoup;
    public Transform chicken;
    public Transform crabCakes;
    // Start is called before the first frame update
    void Start()
    {
       ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
       if(ht != null && ht.ContainsKey("FoodDisplay")) {
           SetupScene((string)ht["FoodDisplay"]);
       }
    }

    public void SetupScene(string menu) {
        if (menu[0] == '0') {
            steakFrites.gameObject.SetActive(false);
        }
        if (menu[1] == '0') {
            crabCakes.gameObject.SetActive(false);
        }
        if (menu[2] == '0') {
            onionSoup.gameObject.SetActive(false);
        }
        if (menu[3] == '0') {
            chicken.gameObject.SetActive(false);
        }
        if (menu[4] == '0') {
            tableBread.gameObject.SetActive(false);
        }
    }

}
