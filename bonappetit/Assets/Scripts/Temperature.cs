using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour
{
    public float temp;
    public float tempDelta;
    public float maxTemp;
    public bool inFridge;
    public float k = .25F; // heat transfer coefficient * surface area
    private HeatingElement heater;
    // Start is called before the first frame update
    void Start()
    {
       temp = 21;
       inFridge = false; 
    }

    void Update() {
        if (heater != null && heater.s.val != 0) {
            tempDelta = heater.tempDelta;
        } else {
            tempDelta = ambientDelta();
        }
        temp += tempDelta * Time.deltaTime;
        maxTemp = System.Math.Max(maxTemp, temp); 
    }

    void OnTriggerEnter(Collider other) {
        heater = other.GetComponent<HeatingElement>();
    }

    private void OnTriggerExit(Collider other)
    {
        
        heater = null;
    }

    // Following Newton's law of cooling
    private float ambientDelta() {
            return - k * (temp - (inFridge? 4F : 21F));
    }

    float tempInF() {
        return temp * 1.8F + 32;
    }

    float tempInC() {
        return temp;
    }


}
