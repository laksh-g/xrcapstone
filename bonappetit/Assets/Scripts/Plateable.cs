using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateable : MonoBehaviour
{
    private Transform cachedParent = null;

    private FixedJoint joint = null;


    void FixedUpdate() {
        if (joint != null && CalculatePlateAngle(transform.parent) > 60) {
            Unstick();
        }
    }
    void OnTriggerEnter (Collider other) {
        if(other.gameObject.tag == "plate" && joint == null && CalculatePlateAngle(other.transform) < 10) {
            Debug.Log("Acquired plate");
            // set rotation to match plate
            var euler = other.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(euler.x, 0, euler.z);
            // move to y position of the plate
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            cachedParent = transform.parent;
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponentInParent<Rigidbody>();
            joint.breakForce = Mathf.Infinity;
            transform.parent = other.transform.parent == null ? other.transform : other.transform.parent;
        }
    }

    public void Unstick() {
        if (joint != null) {
            Debug.Log("Unstuck from plate");
            transform.parent = cachedParent;
            Destroy(joint);
            joint = null;
        }
    }

    private float CalculatePlateAngle(Transform p) {
        float zAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - p.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
    }

}


