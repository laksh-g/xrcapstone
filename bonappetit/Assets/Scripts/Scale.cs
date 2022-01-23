using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    public float reading; // in grams
    // Start is called before the first frame update
    void Start()
    {
        reading = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Tare() {
        reading = 0f;
    }

    void OnTriggerStay(Collider other) {
        Rigidbody r = other.GetComponent<Rigidbody>();
        if (r != null) {
            reading += r.mass;
        }
    }

    void OnTriggerExit(Collider other) {
        Rigidbody r = other.GetComponent<Rigidbody>();
        if (r != null) {
            reading -= r.mass;
        }
    }

    // for UI
    public string GetReading() {
        if (reading >= 1000) {
            return (reading / 1000f).ToString() + "kg";
        }
        return reading.ToString() + "g";
    }
}
