using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhotonView))]
public class Plateable : MonoBehaviourPunCallbacks
{

    private Temperature plateTemp = null;

    private Temperature _temp;

    private bool connected = false;

    public Transform point = null;
    private FixedJoint _joint = null;
    private Transform _transform;
    private Rigidbody _rb;

    private PhotonView _view;

    private XRGrabNetworkInteractable _grab;

    private int plateID = -1;

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
        if (_view.IsMine && connected && point != null && CalculatePlateAngle(point.parent.parent) > 60) {
            _view.RPC("Unstick", RpcTarget.AllViaServer, plateID);
        } else if (connected && point != null && _transform != null) {
            if (plateTemp != null && _temp != null) {
                _temp.heater = plateTemp.heater;
            }
            if (_transform == null)
                Debug.Log("plateable transform null");
            else if (point == null)
                Debug.Log("plateable point is null");
            else
                _transform.SetPositionAndRotation(point.position, point.rotation);
        }
    }

    void OnTriggerEnter (Collider other) {
        if (_view == null)
        {
            Debug.Log("plateable onTrigger _view is null");
        }
        else
        {
            if (!connected && _view.IsMine && other.gameObject.tag == "plate"
            && CalculatePlateAngle(other.transform) < 10 && other.transform.parent != null && other.transform.parent.gameObject.layer != 3)
            {
                connected = true;
                PhotonView view = other.GetComponentInParent<PhotonView>();
                _view.RPC("StickTo", RpcTarget.AllViaServer, view.ViewID);
            }
        }
    }

    [PunRPC]
    public void Unstick(int id) {
        if (connected && id == plateID) {
            connected = false;
            point.tag = tag; // reset tag
            point = null;
            gameObject.layer = 9; // set back to food layer
            _rb.isKinematic = false;
            plateID = -1;
            Dish d = PhotonView.Find(id).GetComponent<Dish>();
            if (d != null) {
                d.connectedItems.Remove(_view.ViewID);
                Debug.LogError("Removed view " + _view.ViewID + " from " + d.connectedItems.ToString());
            }
            
        } else {
            Debug.LogError("Unstick called for unplated object or wrong plate");
        }
    }

    private float CalculatePlateAngle(Transform p) {
        float zAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
    }

    public void GrabUnstick() {
        if (connected && _view.IsMine) {
            _view.RPC("Unstick", RpcTarget.AllViaServer, plateID);
        }
    }
    [PunRPC]
    void StickTo(int id, PhotonMessageInfo info) {
            GameObject target = PhotonView.Find(id).gameObject;
            plateID = id;
            Transform[] transforms = target.GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms) {
                Debug.Log("found hook for " + t.tag);
                if (t.CompareTag(tag)) {
                    _transform.SetPositionAndRotation(t.position, t.rotation);
                    _rb.isKinematic = true;
                    gameObject.layer = 10; // set to plated layer to disable collisions
                    connected = true;
                    point = t;
                    t.tag = "occupied"; // set the tag so other objects don't try to stick here
                    plateTemp = target.GetComponent<Temperature>();
                    Dish d = target.GetComponent<Dish>();
                    if (d != null) {
                        d.connectedItems.Add(_view.ViewID);
                        Debug.Log("New dish contents: " + d.connectedItems.ToString());
                    }
                    break;
                }
            }
            Debug.Log(tag + " failed to find appropriate plate hook");
    }

}


