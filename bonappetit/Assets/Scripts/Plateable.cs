using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Plateable : MonoBehaviourPunCallbacks
{

    private Temperature plateTemp = null;

    private Temperature _temp;

    private bool connected = false;

    public Transform point = null;

    private Transform _transform;
    private Rigidbody _rb;

    private PhotonView _view;

    private XRGrabNetworkInteractable _grab;

    void Awake() {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabNetworkInteractable>();
        _temp = GetComponent<Temperature>();
    }

    void Start() {
        _view = GetComponent<PhotonView>();
    }

    void Update() {
        if (connected && point != null && CalculatePlateAngle(point.parent.parent) > 60) {
            _view.RPC("Unstick", RpcTarget.All);
        } else if (connected) {
            if (plateTemp != null && _temp != null) {
                _temp.heater = plateTemp.heater;
            }
            _transform.SetPositionAndRotation(point.position, point.rotation);
        } else if (_rb.isKinematic == true) {
            Debug.LogError(tag + " Object has been left in kinematic state and will now be unplated");
            Unstick();
        }
    }
    void OnTriggerEnter (Collider other) {
        if(!connected && other.gameObject.tag == "plate" 
        && CalculatePlateAngle(other.transform) < 10 ) {
            PhotonView view = other.GetComponentInParent<PhotonView>();
            _view.RPC("StickTo", RpcTarget.All, view.ViewID);
        }
    }

    [PunRPC]
    public void Unstick() {
        if (connected) {
            Debug.Log(tag +  " unstuck from plate");
            connected = false;
            point.tag = tag; // reset tag
            point = null;
            gameObject.layer = 9; // set back to food layer
            _transform.parent = null;
            _rb.isKinematic = false;
        }
    }

    private float CalculatePlateAngle(Transform p) {
        float zAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
    }

    [PunRPC]
    void StickTo(int id, PhotonMessageInfo info) {
            GameObject target = PhotonView.Find(id).gameObject;
            if (connected) {
                // release from plate if it already has one
                Unstick();
            }
            Transform[] transforms = target.GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms) {
                Debug.Log("found hook for " + t.tag);
                if (t.CompareTag(tag)) {
                    _rb.isKinematic = true;
                    gameObject.layer = 10; // set to plated layer to disable collisions
                    connected = true;
                    point = t;
                    t.tag = "occupied"; // set the tag so other objects don't try to stick here
                    plateTemp = target.GetComponent<Temperature>();
                    _transform.parent = target.transform;
                    Debug.Log(tag + " acquired plate");
                    break;
                }
            }
            Debug.Log(tag + " failed to find appropriate plate hook");
    }

}


