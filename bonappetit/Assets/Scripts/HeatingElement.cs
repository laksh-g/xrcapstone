using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatingElement : MonoBehaviour
{
    public float tempDelta;
    public Knob s;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch(s.val) {
            case(0):
            tempDelta = - .005f;
            break;
            case(1):
            tempDelta = .02f;
            break;
            case(2):
            tempDelta = .05f;
            break;
            case(3):
            tempDelta = .1f;
            break;
        }
    }
}
