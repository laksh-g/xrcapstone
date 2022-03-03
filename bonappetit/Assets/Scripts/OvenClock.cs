using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OvenClock : MonoBehaviour
{
    private bool isActive = false;

    private float timeLimit = 10;

    private float startTime;

    private float timeRemaining;

    public TextMeshPro displayText;

    private AudioSource a = null;

    public AudioClip timerSound = null;

    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            timeRemaining = timeLimit - ((float) Time.time - startTime);
            displayText.text = Text();
            if (timeRemaining <= 0) {
                isActive = false;
                a.PlayOneShot(timerSound);
            }
        }
        
    }

    public void StartTimer(float limit) {
        if (!isActive) {
            isActive = true;
            startTime = Time.time;
        }
    }

    private string Text() {
        int minutes = (int) timeRemaining / 60;
        int seconds = (int) timeRemaining % 60;
        return (minutes.ToString("00") + ":" + seconds.ToString("00"));
    }
}
