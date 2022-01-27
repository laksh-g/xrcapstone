using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter  (Collision other) {
        if(other.gameObject.tag == "plate") {
            print("Plated!");
            transform.parent = other.gameObject.transform;
        }
    }

    void OnCollisionExit (Collision other) {
        if(other.gameObject.tag == "plate") {
            transform.parent = null;
        }
    }
}


