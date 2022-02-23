using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool isOn = false;
    private HeatingElement h = null;
    public MeshRenderer fire = null;
    public Light fireLight = null;
    private Knob k = null;
    public AudioClip startSound;

    public AudioSource startSource = null;
    private AudioSource playSource = null;
    // Start is called before the first frame update
    void Start()
    {
        k = GetComponent<Knob>();
        playSource = GetComponent<AudioSource>();
        fire.enabled = false;
        fireLight.enabled = false;
    }
    public void FireActivate() {
        k.val = 1;
        startSource.PlayOneShot(startSound);
        playSource.Play();
        fire.enabled = true;
        fireLight.enabled = true;
        
    }

    public void FireDeactivate() {
        k.val = 0;
        playSource.Stop();
        fire.enabled = false;
        fireLight.enabled = false;
        
    }
}
