using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fries : MonoBehaviour
{
    public Seasonable seasoning;
    private AudioSource a = null;
    public Temperature temp;


    // Start is called before the first frame update
    void Start()
    {
        temp = GetComponent<Temperature>();
        seasoning = GetComponent<Seasonable>();
        a = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "heater" && temp.tempDelta > 0)
        {
            if (!a.isPlaying) {a.Play();}
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "heater" && temp.tempDelta > 0)
        {
            if (!a.isPlaying) {a.Play(); 
        }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "heater")
        {
            if (a.isPlaying) {a.Stop();}
        }
    }
}
