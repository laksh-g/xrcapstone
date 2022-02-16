using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;

    public Transform HCTransform;
    public Transform STransform;
    public Transform RTransform;

    // private GameObject textGo;
    // private TMP_Text tm;
    // public bool DisableOnOwnObjects;
    // private PhotonView photonView;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //Debug.Log("Entered Room method");
        //XROrigin rig = FindObjectOfType<XROrigin>();

        // Testing new spawnner
        /*
        if (PhotonNetwork.NickName == "Saucier"){
            rig.MoveCameraToWorldLocation(STransform.position);
            rig.RotateAroundCameraUsingOriginUp(90);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", STransform.position, STransform.rotation);

        }else if (PhotonNetwork.NickName == "Rotisseur"){
            rig.MoveCameraToWorldLocation(RTransform.position);
            rig.RotateAroundCameraUsingOriginUp(-90);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", RTransform.position, RTransform.rotation);
        } else{
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            
            rig.MoveCameraToWorldLocation(HCTransform.position);
            rig.RotateAroundCameraUsingOriginUp(180);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", HCTransform.position, HCTransform.rotation);
        }
        */

        // photonView = GetComponent<PhotonView>();
        // if (tm == null) //wenn textmesh
        // {
        //     textGo = spawnedPlayerPrefab.GetComponentInChildren<TMP_Text>().gameObject;
        //     tm = textGo.GetComponent<TMP_Text>();
        // }

        // bool showInfo = !this.DisableOnOwnObjects || photonView.IsMine;
        // if (textGo != null)
        // {
        //     textGo.SetActive(showInfo);
        // }
        // if (!showInfo)
        // {
        //     return;
        // }

        // tm.text = PhotonNetwork.NickName;
    }

    void Start()
    {
        XROrigin rig = FindObjectOfType<XROrigin>();

        PhotonNetwork.AutomaticallySyncScene = false;

        // Testing new spawnner

        ExitGames.Client.Photon.Hashtable playerCustomProps = PhotonNetwork.LocalPlayer.CustomProperties;

        Debug.Log(playerCustomProps["role"]);
        Debug.Log((string) playerCustomProps["role"]);

        if ((string) playerCustomProps["role"] == "SaucierRole"){
            rig.MoveCameraToWorldLocation(STransform.position);
            rig.RotateAroundCameraUsingOriginUp(90);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", STransform.position, STransform.rotation);

        }else if ((string) playerCustomProps["role"] == "RotisseurRole"){
            rig.MoveCameraToWorldLocation(RTransform.position);
            rig.RotateAroundCameraUsingOriginUp(-90);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", RTransform.position, RTransform.rotation);
        } else{
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            
            rig.MoveCameraToWorldLocation(HCTransform.position);
            rig.RotateAroundCameraUsingOriginUp(180);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", HCTransform.position, HCTransform.rotation);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
