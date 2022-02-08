using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ShowNetworkRole : MonoBehaviourPunCallbacks
{
    private GameObject textGo;
    private TMP_Text tm;
    public bool DisableOnOwnObjects;

    private PhotonView photonView;

    // Start is called before the first frame update
   void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (tm == null) //wenn textmesh
        {
            textGo = gameObject.GetComponentInChildren<TMP_Text>().gameObject;
            tm = textGo.GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool showInfo = !this.DisableOnOwnObjects || photonView.IsMine;
        if (textGo != null)
        {
            textGo.SetActive(showInfo);
        }
        if (!showInfo)
        {
            return;
        }
        tm.text = PhotonNetwork.NickName;
        Debug.Log(PhotonNetwork.NickName);
        //PhotonPlayer owner = photonView.owner;

        // if (owner != null)
        // {
        //     tm.text = (string.IsNullOrEmpty(owner.NickName)) ? "player" + owner.ID : owner.NickName;
        // }
        // else if (photonView.isSceneView)
        // {
        //     tm.text = "scn";
        // }
        // else
        // {
        //     tm.text = "n/a";
        // }
    }
}
