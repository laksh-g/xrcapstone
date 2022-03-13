using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grater : MonoBehaviour
{
    public float threshold = 0.1f;
    private Seasonable target = null;
    private bool isPouring = false;
    private Rigidbody r = null;

    private ParticleSystem p = null;

    private Rigidbody gratedObj = null;
    private AudioSource a = null;
    public AudioClip shakeSound = null;
    private readonly float pourRate = 0.2f; // in grams

    private PhotonView _view;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        p = GetComponentInChildren<ParticleSystem>();
        a = GetComponent<AudioSource>();
        _view = GetComponent<PhotonView>();
    }
    void Update()
    {
            bool check = gratedObj != null && isGrating();
        
            if (isPouring != check) {
                isPouring = check; 
                if (isPouring) {
                    a.PlayOneShot(shakeSound);
                    Debug.Log("Grating!");
                    p.Play();
                } else {
                    p.Stop();
                    target = null;
                }
            }


            if (_view.IsMine && isPouring) {
                CheckHit();
                if (target != null) {
                    _view.RPC("GrateObject", RpcTarget.AllViaServer, target.GetComponent<PhotonView>().ViewID);
                }
        }
    }


    private bool isGrating() {
        if (gratedObj != null) {
            if (Vector3.Dot(gratedObj.velocity, transform.forward) > threshold) {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "gruyere") {
            gratedObj = other.gameObject.GetComponent<Rigidbody>();
            Debug.Log("Grater registered object: " + gratedObj.tag);
        }
        
    }
    

    void OnTriggerExit(Collider other) {
        if (gratedObj != null) {
            gratedObj = null;
        }
    }

    private void CheckHit() {
        int layerMask = (1 << 9) | (1 << 10); // only cast against layers 9 and 10
        RaycastHit hit;
        Ray ray = new Ray(p.transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 10.0f, layerMask);
        if (hit.collider != null) {
            Seasonable s = hit.collider.GetComponentInParent<Seasonable>();
            if (s != null) {
                target = s;
            } else {
                target = null;
            }
        }
    }

    [PunRPC]
    void GrateObject(int id) {
        PhotonView obj = PhotonView.Find(id);
        if (obj != null) {
            Seasonable thistarget = obj.GetComponent<Seasonable>();
            thistarget.gruyere += pourRate;
        }
    }
}
