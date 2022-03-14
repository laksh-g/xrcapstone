using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRSimpleNetworkInteractable : XRSimpleInteractable
{
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    override protected void OnSelectEntering(SelectEnterEventArgs interactor)
    {
        photonView.RequestOwnership();
        base.OnSelectEntering(interactor);
    }
}
