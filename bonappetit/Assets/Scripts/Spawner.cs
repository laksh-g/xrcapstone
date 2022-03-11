using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhotonView))]
public class Spawner : MonoBehaviour
{
    public bool onlyAllowOneActiveCopy = false;
    private Vector3 spawnPosition;

    private Quaternion spawnRotation;

    public GameObject initialSpawnedObject;

    public GameObject prefab;

    private Transform activeCopy = null;

    private PhotonView _view = null;


    // Start is called before the first frame update
    void Start()
    {
        Transform t = initialSpawnedObject.transform;
        spawnPosition = new Vector3(t.position.x, t.position.y, t.position.z);
        spawnRotation = new Quaternion(t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
        initialSpawnedObject.layer = 3;
        _view = GetComponent<PhotonView>();
    }


    void OnTriggerExit(Collider other) {
        Debug.Log (tag + " left the spawner");
        PhotonView otherview = other.gameObject.GetComponent<PhotonView>();
        if (other.gameObject == initialSpawnedObject && otherview.IsMine) {
            Transform oldActive = activeCopy;
            activeCopy = initialSpawnedObject.transform;
            Debug.Log("Starting spawn protocol for " + other.tag);
            other.gameObject.layer = prefab.layer;
            _view.RPC("ReleaseObject", RpcTarget.All, otherview.ViewID);
            if (onlyAllowOneActiveCopy && oldActive != null) {
                oldActive.gameObject.layer = 3;
                oldActive.SetPositionAndRotation(spawnPosition, spawnRotation);
                initialSpawnedObject = oldActive.gameObject;
                LiquidContainer l = initialSpawnedObject.GetComponent<LiquidContainer>();
                if (l != null) {
                    l.Refill();
                }
            } else {
                initialSpawnedObject = PhotonNetwork.Instantiate(prefab.name, spawnPosition, spawnRotation);
                initialSpawnedObject.layer = 3;
            }
            _view.RPC("SetupObject", RpcTarget.All, initialSpawnedObject.GetComponent<PhotonView>().ViewID);
            
        }
    }

    [PunRPC]
    void SetupObject(int viewID) {
            GameObject target = PhotonView.Find(viewID).gameObject;
            target.layer = 3;
            initialSpawnedObject = target;
    }

    [PunRPC]
    void ReleaseObject(int viewID) {
        GameObject target = PhotonView.Find(viewID).gameObject;
        if (target != initialSpawnedObject) {
            Debug.LogError("Releasing object that is not the current one in the spawner");
        }
        target.layer = prefab.layer;
        initialSpawnedObject = null;
    }
}
