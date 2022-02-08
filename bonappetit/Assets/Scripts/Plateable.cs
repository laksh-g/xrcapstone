using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateable : MonoBehaviour
{
    private Transform cachedParent = null;
    private FixedJoint joint = null;
    // Start is called before the first frame update

    void OnTriggerEnter (Collider other) {
        if(other.gameObject.tag == "plate" && joint == null) {
            print("Acquired plate");
            cachedParent = transform.parent;
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponentInParent<Rigidbody>();
            joint.breakForce = Mathf.Infinity;
            joint.enableCollision = true;
            print(other.transform);
            transform.parent = other.transform.parent == null ? other.transform : other.transform.parent;
        }
    }

    //void OnTriggerStay(Collider other) {
     //   OnTriggerEnter(other);
    //}

    public void Unstick() {
        if (joint != null) {
            print("Unstuck from plate");
            transform.parent = cachedParent;
            Destroy(joint);
            joint = null;
        }
    }
    void OnJointBreak(float breakforce) {
        print("Lost plate");
        transform.parent = cachedParent;
        Destroy(joint);
        joint = null;
    }

}


