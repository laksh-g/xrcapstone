using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class Spawner : MonoBehaviour
{
    //public GameObject prefab;
    private bool offline = true;
    private Vector3 spawnPosition;

    private Quaternion spawnRotation;

    public GameObject initialSpawnedObject;

    public GameObject prefab;
    private bool spawnedNew = false;
    //Rigidbody _rb = null;
    // Start is called before the first frame update
    void Start()
    {
        Transform t = initialSpawnedObject.transform;
        spawnPosition = new Vector3(t.position.x, t.position.y, t.position.z);
        spawnRotation = new Quaternion(t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
        initialSpawnedObject.layer = 3;
        if (PhotonNetwork.OfflineMode) {
            offline = false;
        }
    }


    void OnTriggerExit(Collider other) {
        Debug.Log (tag + " left the spawner");
        if (other.gameObject == initialSpawnedObject) {
            Debug.Log(tag + " spawnedObj left the spawner");
            initialSpawnedObject.layer = prefab.layer;
            if (offline) {
                initialSpawnedObject = Instantiate(prefab, spawnPosition, spawnRotation);
                initialSpawnedObject.layer = 3;
                
            } else {
                initialSpawnedObject = PhotonNetwork.Instantiate(prefab.name, spawnPosition, spawnRotation);
                initialSpawnedObject.layer = 3;
            }
        }
    }
}
