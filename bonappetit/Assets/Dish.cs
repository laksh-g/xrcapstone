using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dish : MonoBehaviourPunCallbacks
{
    public string dishID;
    public Transform itemFolder = null;

    private PhotonView _view = null;

    void Awake() {
        _view = GetComponent<PhotonView>();
    }


    public string GetCompletionInfo()
    {   string result = "";
        foreach (Transform t in itemFolder) {
            if (t.tag != "occupied") {
                result += "Missing " + t.tag + "\n";
            }
        }
        return result;
    }

    public int GetNumOfMissingComponents() {
        int result = 0;
        foreach (Transform t in itemFolder) {
            if (t.tag != "occupied") {
                result++;
            }
        }
        return result;
    }

    public void TransferFoodOwnership() {
        if (!_view.IsMine) {
            PhotonView[] views = GetComponentsInChildren<PhotonView>();
            foreach (PhotonView view in views) {
                view.RequestOwnership();
            }
        }
    }
}
