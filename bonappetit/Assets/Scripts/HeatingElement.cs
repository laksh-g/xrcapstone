using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatingElement : MonoBehaviour
{
    public float tempDelta;
    public Knob s = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    { if (s != null) {
        if (s.numSettings == 4) {
            switch(s.val) {
                case(0):
                tempDelta = 0f;
                break;
                case(1):
                tempDelta = .5f;
                break;
                case(2):
                tempDelta = 1f;
                break;
                case(3):
                tempDelta = 3f;
                break;
            }
        } else if (s.numSettings == 2) {
            switch(s.val) {
                case(0):
                tempDelta = 0f;
                break;
                case(1):
                tempDelta = 4f;
                break;
            }
        }
    }
    }
}
