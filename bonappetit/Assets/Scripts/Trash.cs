using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class Trash : MonoBehaviour
{
    public bool specificTrash = false;
    public string[] acceptedTags = null;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other) {
        if (other.tag == "order") {
            // don't destroy orders because those assets are reused
            return;
        }
        PhotonView view = other.GetComponent<PhotonView>();
        if (view == null || !view.IsMine) {
            return;
        }
        if (specificTrash) {
            if (acceptedTags.Contains(other.gameObject.tag)) {
                PhotonNetwork.Destroy(other.gameObject);
            }
        } else {
            PhotonNetwork.Destroy(other.gameObject);
        }
    }
}
