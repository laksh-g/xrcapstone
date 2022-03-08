using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool isOn = false;
    public GameObject fire = null;

    public Knob k = null;
    public AudioClip startSound;
    public AudioSource startSource = null;
    private AudioSource playSource = null;
    // Start is called before the first frame update
    void Start()
    {
        playSource = GetComponent<AudioSource>();
        fire.SetActive(false);
    }

    void Update() {if (isOn && fire.activeSelf == false) {FireActivate();} if(!isOn && fire.activeSelf == true) {FireDeactivate();}}
    public void FireActivate() {
        k.val = 1;
        startSource.PlayOneShot(startSound);
        playSource.Play();
        fire.SetActive(true);
    }

    public void FireDeactivate() {
        k.val = 0;
        playSource.Stop();
        fire.SetActive(false);
    }
}
