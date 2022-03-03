using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerKnob : MonoBehaviour
{
    public IndicatorLight greenLight = null;
    public IndicatorLight redLight = null;
    public int time = 10;

    public Transform from;
    public Transform to;
    private float currentTime = 0;
    public bool isActive = false;
    public AudioClip doneSound = null;
    private AudioSource a = null;
    private Transform _transform = null;
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        a = GetComponent<AudioSource>();
        greenLight.on = true;
        redLight.on = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            float rotateDegrees = 360 * (currentTime / time);
            transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, currentTime/ time);
            currentTime += Time.deltaTime; 
            if (currentTime >= time) {
                a.PlayOneShot(doneSound);
                greenLight.on = true;
                redLight.on = false;
                _transform.rotation = from.rotation;
                currentTime = 0;
                isActive = false;
            }
        }
    }

    public void Activated() {
        if (!isActive) {
            isActive = true;
            redLight.on = true;
            greenLight.on = false;
        }
    }
}
