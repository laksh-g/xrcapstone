using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppable : MonoBehaviour
{
    public Transform toppingLocation = null;
    private bool taken = false;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "bread" && !taken) {
            taken = true;
            other.gameObject.transform.position = toppingLocation.position;
            other.gameObject.transform.parent = gameObject.transform;
            Destroy(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}
