using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shaker : MonoBehaviour
{
    Seasonable target = null;
    bool isPouring = false;
    Rigidbody r = null;

    private ParticleSystem p = null;
    private AudioSource a = null;
    public AudioClip shakeSound = null;
    private readonly float pourRate = 0.05f; // in grams

    private PhotonView _view;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        p = GetComponentInChildren<ParticleSystem>();
        a = GetComponent<AudioSource>();
        _view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
            float threshold = 90;
            bool check = (CalculatePourAngle() > threshold) && (r.velocity.y < -1);
        
            if (isPouring != check) {
                isPouring = check; 
                if (isPouring) {
                    a.PlayOneShot(shakeSound);
                    print("Pouring!");
                    if (p != null) {
                    p.Play();
                    }
                } else {
                    if (p != null) {
                        p.Stop();
                    }
                    target = null;
                }
            }
            if (isPouring && _view.IsMine) {
                CheckHit();
                if (target != null) {
                    _view.RPC("SeasonObject", RpcTarget.AllViaServer, target.view.ViewID);
                }
            }
    }

    [PunRPC]
    void SeasonObject(int id) {
        PhotonView obj = PhotonView.Find(id);
        if (obj != null) {
            Seasonable thistarget = obj.GetComponent<Seasonable>();
            if (tag == "salt") {
                    thistarget.salt += pourRate;
            } else if (tag == "pepper") {
                    thistarget.pepper += pourRate;
            } else if (tag == "parsley") {
                    thistarget.parsley += pourRate;
            }
        }
    }
    private float CalculatePourAngle() {
        float zAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
    }

    private void CheckHit() {
        int layerMask = (1 << 9) | (1 << 10); // only cast against layers 9 and 10
        RaycastHit hit;
        Ray ray = new Ray(p.transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 10.0f, layerMask);
        if (hit.collider != null) {
            Seasonable s = hit.collider.GetComponent<Seasonable>();
            if (s != null) {
                target = s;
            } else {
                target = null;
            }
        }
    }
}
