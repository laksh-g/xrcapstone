using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dish : MonoBehaviourPunCallbacks
{
    public string dishID;
    public Transform itemFolder = null;

    public HashSet<int> connectedItems = new HashSet<int>();
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

    public void Release() {
        if (connectedItems.Count > 0 && _view.IsMine) {
            foreach (int id in connectedItems) {
                _view.RPC("ReleaseComponent", RpcTarget.AllViaServer, id);
                return;
            }
        }
    }

    [PunRPC] 
    void ReleaseComponent(int viewID) {
        if (connectedItems.Contains(viewID)) {
            connectedItems.Remove(viewID);
            PhotonView target = PhotonView.Find(viewID);
            target.RPC("Unstick", RpcTarget.AllViaServer, _view.ViewID);
            Transform t = target.gameObject.transform;
        }
    }
}
