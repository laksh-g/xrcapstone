using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OwnerShipTransfer : MonoBehaviourPun
{
    private void OnMouseDown()
    {
        Debug.Log("Called Function");
        base.photonView.RequestOwnership();
    }
}
