using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour
{
    public float temp;
    public float tempDelta;
    public bool isCooked;
    // Start is called before the first frame update
    void Start()
    {
       temp = 72; 
       tempDelta = -.01f;
       isCooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        temp = System.Math.Max(72, temp + tempDelta); 
        if (temp >= 140f) {
            isCooked = true;
        }
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
        
        tempDelta = -.001f;
    }
}
