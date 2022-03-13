using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cheese : MonoBehaviour, IPunObservable
{
    [SerializeField]
    public float toastingTime = 0;
    private Temperature _temp = null;
    public Transform cheeseTransform = null;
    private Seasonable _seasoning = null;
    public MeshRenderer cheeseMesh = null;
    private HeatingElement h = null;

    public Transform start = null;
    public Transform end = null;

    //public Material startMaterial = null;

    //public Material endMaterial = null;

    public AudioSource _audio = null;
    public ParticleSystem _smoke = null;

    private PhotonView _view;

    void Awake() {
        _temp = GetComponent<Temperature>();
        _seasoning = GetComponent<Seasonable>();
        cheeseMesh.enabled = false;
        _smoke.Stop();
        _view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        h = _temp.heater;
        if (h != null && cheeseMesh.enabled == true && h.s.val == 1) {
            _audio.Play();
            _smoke.Play();
            if (_view.IsMine) {
                toastingTime += Time.deltaTime;
            }
        } else if (_audio.isPlaying) {
            _audio.Stop();
            _smoke.Stop();
        }
        if(cheeseMesh.enabled == false && _seasoning.gruyere > 5) {
            //_mesh.material = startMaterial;
            cheeseMesh.enabled = true;
        }
        if (toastingTime > 0) {
            float percentDone = Mathf.Min(toastingTime / 10, 1);
            cheeseTransform.localScale = Vector3.Lerp(start.localScale, end.localScale, percentDone);
            cheeseTransform.position = Vector3.Lerp(start.position, end.position, percentDone);
            //_mesh.material.Lerp(startMaterial, endMaterial, percentDone);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(toastingTime);
        }
        else
        {
            toastingTime = (float)stream.ReceiveNext();
        }
    }
}
