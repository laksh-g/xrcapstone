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
    // Start is called before the first frame update
    void Start()
    {
       temp = 72;
       inFridge = false; 
       tempDelta = ambientDelta();
    }

    void Update() {
    }

    void FixedUpdate()
    {
        tempDelta = ambientDelta();
        temp = temp + tempDelta; 
        maxTemp = System.Math.Max(maxTemp, temp);
    }

    private void OnTriggerStay(Collider other)
    {
        HeatingElement h = other.GetComponent<HeatingElement>();
        if (h != null) {
            tempDelta = h.tempDelta;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        tempDelta = ambientDelta();
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
