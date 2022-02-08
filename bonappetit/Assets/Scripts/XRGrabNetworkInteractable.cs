using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;
    private AudioSource a;
    public AudioClip grabSound;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        a = GetComponent<AudioSource>();
        if (a == null) {
            a = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override protected void OnSelectEntering(SelectEnterEventArgs interactor)
    {
        a.PlayOneShot(grabSound);
        photonView.RequestOwnership();
        base.OnSelectEntering(interactor);
    }
}