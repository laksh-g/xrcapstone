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

    public int viewID;
    void Awake() {
        _view = GetComponent<PhotonView>();
        viewID = _view.ViewID;
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
            _view.RequestOwnership();
            foreach (int id in connectedItems) {
                PhotonView view = PhotonView.Find(id);
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
