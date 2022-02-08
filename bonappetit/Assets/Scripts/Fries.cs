using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fries : MonoBehaviour
{
    public Seasonable seasoning;
    private AudioSource a = null;
    public Temperature temp;
    private ParticleSystem bubbles;
    public bool isCooking;
    private HeatingElement heater;
    private System.Single maxRate = 30f;
    private System.Single minRate = 4f;
    private System.Single maxHt = 10f;
    private System.Single minHt = 3f;

    // Start is called before the first frame update
    void Start()
    {
        temp = GetComponent<Temperature>();
        bubbles = GetComponent<ParticleSystem>();
        seasoning = GetComponent<Seasonable>();
        isCooking = false;
        a = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering");
        if (other.gameObject.name == "Oil" && temp.tempDelta > 0)
        {
            bubbles.Play();
            if (!a.isPlaying) {a.Play();}
            heater = other.GetComponent<HeatingElement>();
            isCooking = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Staying");
        if (other.gameObject.name == "Oil" && temp.tempDelta > 0)
        {
            if (!a.isPlaying) {a.Play(); }
            if (!bubbles.isPlaying)
            {
                bubbles.Play();
            }

            var rate = Mathf.Abs(temp.maxTemp - temp.temp) / temp.maxTemp;

            var emission = bubbles.emission;
            emission.rateOverTime = maxRate * rate + minRate;

            var main = bubbles.main;
            main.startLifetime = maxHt * rate + minHt;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Oil")
        {
            if (a.isPlaying) {a.Stop();}
            bubbles.Stop();
            heater = null;
            isCooking = false;
        }
    }
}
