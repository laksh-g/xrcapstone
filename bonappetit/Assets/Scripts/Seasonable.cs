using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class Seasonable : MonoBehaviour
{
   
    public float salt = 0;
    
    public float pepper = 0;
  
    public float parsley = 0;
    
    public float gruyere = 0;

    public PhotonView view = null;

    void Awake() {
        view = GetComponent<PhotonView>();
    }
}
