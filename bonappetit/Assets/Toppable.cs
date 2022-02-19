using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppable : MonoBehaviour
{
    public Transform toppingLocation = null;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "bread") {
            
        }
    }
}
