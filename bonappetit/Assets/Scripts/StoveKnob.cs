using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoveKnob : MonoBehaviour
{
    public int val; // 0 - 3
    // Start is called before the first frame update
    void Start()
    {
    val = 0; // off  
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void RotateKnob()
    {
        transform.Rotate(0f, 0f, 90.0f);
        if (val == 3) {
            val = 0;
        } else {
            val++;
        }
    }
}
