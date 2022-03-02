using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateable : MonoBehaviour
{
    private Transform cachedParent = null;

    private bool connected = false;

    private Transform point = null;

    private Transform _transform;
    private Rigidbody _rb;

    private XRGrabNetworkInteractable _grab;

    void Awake() {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabNetworkInteractable>();
    }

    void Update() {
        if (connected && CalculatePlateAngle(point.parent.parent) > 60) {
            Unstick();
        } else if (connected) {
            _transform.SetPositionAndRotation(point.position, point.rotation);
        } else if (_rb.isKinematic == true) {
            Unstick();
        }
    }
    void OnTriggerEnter (Collider other) {
        if(!connected && other.gameObject.tag == "plate" 
        && CalculatePlateAngle(other.transform) < 10 ) {
            Transform[] transforms = other.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms) {
                Debug.Log("found hook for " + t.tag);
                if (t.CompareTag(tag)) {
                    _rb.isKinematic = true;
                    gameObject.layer = 10; // set to plated layer to disable collisions
                    connected = true;
                    point = t;
                    t.tag = "occupied"; // set the tag so other objects don't try to stick here
                    cachedParent = _transform.parent;
                    _transform.parent = other.transform.parent;
                    Debug.Log(tag + " acquired plate");
                    break;
                }
            }
            Debug.Log(tag + " failed to find appropriate plate hook");
        }
    }

    public void Unstick() {
        if (connected) {
            Debug.Log(tag +  " unstuck from plate");
            connected = false;
            point.tag = tag; // reset tag
            point = null;
            gameObject.layer = 9; // set back to food layer
            _rb.isKinematic = false;
            _transform.parent = cachedParent;
        }
    }

    private float CalculatePlateAngle(Transform p) {
        float zAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
    }

}


