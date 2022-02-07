using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;

    public Transform HCTransform;
    public Transform STransform;
    public Transform RTransform;


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        XROrigin rig = FindObjectOfType<XROrigin>();

        if (PhotonNetwork.NickName == "Saucier"){
            rig.MoveCameraToWorldLocation(STransform.position);
            rig.RotateAroundCameraUsingOriginUp(90);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", STransform.position, STransform.rotation);
        }else if (PhotonNetwork.NickName == "Rotisseur"){
            rig.MoveCameraToWorldLocation(RTransform.position);
            rig.RotateAroundCameraUsingOriginUp(-90);
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", RTransform.position, RTransform.rotation);
        } else{
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
