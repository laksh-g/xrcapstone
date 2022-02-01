using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scale : MonoBehaviour
{
    public float reading; // in grams
    public TextMeshPro text;
    
    private float cachedReading;
    // Start is called before the first frame update
    void Start()
    {
        reading = 0f;
        cachedReading = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (cachedReading != reading) {
            cachedReading = reading;
            text.text = GetReading();
        }
    }

    public void Tare() {
        reading = 0f;
    }

    void OnTriggerEnter(Collider other) {
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
            return (reading / 1000f).ToString("0.00") + "kg";
        }
        return reading.ToString("0.0") + "g";
    }
}
