using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class Plateable : MonoBehaviour, IPunObservable
{
    private Transform cachedParent = null;
    [SerializeField]
    private FixedJoint joint = null;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(joint);
        }
        else
        {
            joint = (FixedJoint)stream.ReceiveNext();
        }
    }
}


