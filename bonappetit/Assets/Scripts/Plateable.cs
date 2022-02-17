using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateable : MonoBehaviour
{
    private Transform cachedParent = null;
    private FixedJoint joint = null;
    void OnTriggerEnter (Collider other) {
        if(other.gameObject.tag == "plate" && joint == null) {
            print("Acquired plate");
            // set rotation to match plate
            var euler = other.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(euler.x, 0, euler.z);
            // move to y position of the plate
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;

            cachedParent = transform.parent;
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponentInParent<Rigidbody>();
            joint.breakForce = Mathf.Infinity;
            //gameObject.GetComponent<Rigidbody>().enabled = false;
            transform.parent = other.transform.parent == null ? other.transform : other.transform.parent;
        }
    }

    public void Unstick() {
        if (joint != null) {
            print("Unstuck from plate");
            transform.parent = cachedParent;
            Destroy(joint);
            //gameObject.GetComponent<Rigidbody>().detectCollisions = true;
            //gameObject.GetComponent<Rigidbody>().isKinematic = false;
            joint = null;
        }
    }

}


