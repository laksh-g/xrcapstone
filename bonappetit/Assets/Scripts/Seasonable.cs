using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class Seasonable : MonoBehaviour, IPunObservable
{
    [SerializeField]
    public float salt = 0;
    [SerializeField]
    public float pepper = 0;
    [SerializeField]
    public float parsley = 0;
    [SerializeField]
    public float truffleOil = 0;
    [SerializeField]
    public float gruyere = 0;

    private PhotonView _view = null;

    void Awake() {
        _view = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && _view.IsMine)
        {
            stream.SendNext(salt);
            stream.SendNext(pepper);
            stream.SendNext(parsley);
            stream.SendNext(truffleOil);
            stream.SendNext(gruyere);
        }
        else
        {
            salt = (float)stream.ReceiveNext();
            pepper = (float)stream.ReceiveNext();
            parsley = (float)stream.ReceiveNext();
            truffleOil = (float)stream.ReceiveNext();
            gruyere = (float)stream.ReceiveNext();
        }
    }
}
