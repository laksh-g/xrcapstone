using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knob : MonoBehaviour
{
    public int val; // 0 - numSettings
    public int numSettings;
    private readonly string[] labels4 = {"Off", "Low", "Medium", "High"};
    private readonly string[] labels2 = {"Off", "On"};

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
        float rotateDegrees = 360f / numSettings;
        transform.Rotate(0f, 0f, rotateDegrees);
        if (val == numSettings) {
            val = 0;
        } else {
            val++;
        }
    }

    // returns a label for the current setting of the knob
    public string getLabel() {
        if (numSettings == 4) {
            return labels4[val];
        } else if (numSettings == 2) {
            return labels2[val];
        }

        return val.ToString();
    }
}
