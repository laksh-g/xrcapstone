using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppable : MonoBehaviour
{
    public Transform toppingLocation = null;
    private bool taken = false;
    
    void Update() {

    }
    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (!taken && rb != null) {
            taken = true;
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.gameObject.transform.position = toppingLocation.position;
            other.gameObject.transform.parent = toppingLocation.parent;
        }
    }
}
