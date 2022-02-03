using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    HeatingElement thisHeater = null;
    HeatingElement otherHeater = null;
    // Start is called before the first frame update
    void Start()
    {
        thisHeater = GetComponentInChildren<HeatingElement>();
        thisHeater.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other) {
        otherHeater = other.GetComponent<HeatingElement>();
        if (otherHeater != null) {
            thisHeater.s = otherHeater.s;
            thisHeater.enabled = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (otherHeater != null) {
            thisHeater.s = null;
            thisHeater.enabled = false;
        }
    }
}
