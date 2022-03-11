using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class Spawner : MonoBehaviour
{
    public bool onlyAllowOneActiveCopy = false;
    private Vector3 spawnPosition;

    private Quaternion spawnRotation;

    public GameObject initialSpawnedObject;

    public GameObject prefab;

    private Transform activeCopy = null;


    // Start is called before the first frame update
    void Start()
    {
        Transform t = initialSpawnedObject.transform;
        spawnPosition = new Vector3(t.position.x, t.position.y, t.position.z);
        spawnRotation = new Quaternion(t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
        initialSpawnedObject.layer = 3;
    }


    void OnTriggerExit(Collider other) {
        Debug.Log (tag + " left the spawner");
        
        if (other.gameObject == initialSpawnedObject) {
            PhotonView otherview = other.gameObject.GetComponent<PhotonView>();
            Debug.Log(tag + " spawnedObj left the spawner");
            initialSpawnedObject.layer = prefab.layer;
            Transform oldActiveCopy = activeCopy;
            activeCopy = initialSpawnedObject.transform;
            if (onlyAllowOneActiveCopy && oldActiveCopy != null) {
                Debug.Log("reusing spawned asset " + tag);
                oldActiveCopy.SetPositionAndRotation(spawnPosition, spawnRotation);
                initialSpawnedObject = activeCopy.gameObject;
            } else {
                    Debug.Log("Photon network offline :" + PhotonNetwork.OfflineMode);
                    initialSpawnedObject = PhotonNetwork.Instantiate(prefab.name, spawnPosition, spawnRotation);
                    initialSpawnedObject.layer = 3;
            }
            
        }
    }

    [PunRPC]
    private void Respawn() {
        Debug.Log(tag + " spawnedObj left the spawner");
            initialSpawnedObject.layer = prefab.layer;
            Transform oldActiveCopy = activeCopy;
            activeCopy = initialSpawnedObject.transform;
            if (onlyAllowOneActiveCopy && oldActiveCopy != null) {
                Debug.Log("reusing spawned asset " + tag);
                oldActiveCopy.SetPositionAndRotation(spawnPosition, spawnRotation);
                initialSpawnedObject = activeCopy.gameObject;
            } else {
                    Debug.Log("Photon network offline :" + PhotonNetwork.OfflineMode);
                    initialSpawnedObject = PhotonNetwork.Instantiate(prefab.name, spawnPosition, spawnRotation);
                    initialSpawnedObject.layer = 3;
            }
    }
}
