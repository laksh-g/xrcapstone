using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knob : MonoBehaviour
{
    public int val; // 0 - numSettings
    public int numSettings;
    public Light greenLight;
    public Light redLight;

    public AudioClip on;

    public AudioClip off;

    private AudioSource a;

    public readonly string[] labels4 = {"Off", "Low", "Medium", "High"};
    public readonly string[] labels2 = {"Off", "On"};


    // Start is called before the first frame update
    void Start()
    {
    a = GetComponent<AudioSource>();
    val = 0; // off 
    if (greenLight != null && redLight != null) {
        redLight.enabled = true;
        greenLight.enabled = false;
    }
    }

    public void RotateKnob()
    {
        float rotateDegrees = 360f / numSettings;
        transform.Rotate(0f, 0f, rotateDegrees);
        if (val == numSettings - 1) {
            a.PlayOneShot(off);
            val = 0;
            if (greenLight != null && redLight != null) {
                greenLight.enabled = false;
                redLight.enabled = true;
            }
        } else {
            val++;
            a.PlayOneShot(on);
            if (greenLight != null && redLight != null) {
                redLight.enabled = false;
                greenLight.enabled = true;
            }
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
