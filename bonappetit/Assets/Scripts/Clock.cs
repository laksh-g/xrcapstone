using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
public class Clock : MonoBehaviourPunCallbacks
{
    public float startTime = -1;

    public bool gameIsActive = false;
    private TextMeshPro t = null;

    public static int GAME_LENGTH = 600;

    public XRInteractionManager im = null;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            if(!ht.ContainsKey("time")){
                GAME_LENGTH = 240;
            }else{
                string time = ((string)ht["time"]).Split(':')[0];
                GAME_LENGTH = int.Parse(time)*60;
            }
        }
        t = GetComponentInChildren<TextMeshPro>();
        t.text = (GAME_LENGTH / 60).ToString("00") + ":" + (GAME_LENGTH % 60).ToString("00");
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime != -1) {
            gameIsActive = true;
            im.enabled = true;
            t.text = Text();
        }
        if ((int)(GAME_LENGTH - ((float) PhotonNetwork.Time - startTime)) == 1) {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("startTime")) {
            startTime = (float) propertiesThatChanged["startTime"];
        }
    }

    private string Text() {
        float time = (float) PhotonNetwork.Time;
        float time_elapsed = GAME_LENGTH - ((float) PhotonNetwork.Time - startTime);
        int minutes = (int) time_elapsed / 60;
        int seconds = (int) time_elapsed % 60;
        return (minutes.ToString("00") + ":" + seconds.ToString("00"));
    }

    public static int getGameLength(){
        return GAME_LENGTH;
    }
}
