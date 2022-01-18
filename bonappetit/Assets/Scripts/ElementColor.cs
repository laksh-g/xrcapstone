using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementColor : MonoBehaviour
{
    public Material green;
    public Material red;
    public Material yellow;
    public Material off;
    public StoveKnob s;
    // Start is called before the first frame updates
    void Start()
    {
        GetComponent<MeshRenderer>().material = off;
        s = GetComponent<HeatingElement>().s;
    }

    // Update is called once per frame
    void Update()
    {
        switch(s.val) {
            case(0):
            GetComponent<MeshRenderer>().material = off;
            break;
            case(1):
            GetComponent<MeshRenderer>().material = green;
            break;
            case(2):
            GetComponent<MeshRenderer>().material = yellow;
            break;
            case(3):
            GetComponent<MeshRenderer>().material = red;
            break;
        }
    }
}
