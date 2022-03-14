using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SauceBuilder : MonoBehaviour, IPunObservable
{
    [SerializeField]
    public bool hasDrippings = false;
    [SerializeField]
    public bool hasShallots = false;
    [SerializeField]
    public bool hasWine = false;

    public Transform shallotTransform = null;

    public Temperature _temp = null;
    public Material sauceMaterial = null;
    private LiquidContainer liquid = null;

    private Dish _dish;

    private PhotonView _view;

    void Start() {
        liquid = GetComponent<LiquidContainer>();
        _temp = GetComponent<Temperature>();
        _dish = GetComponent<Dish>();
        _view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDrippings && tag == "pan drippings") {
            hasDrippings = true;
        } else if (!hasWine && tag == "chardonnay") {
            hasWine = true;
        }
        
        if (!hasShallots && shallotTransform.tag == "occupied" && _temp.temp > 60 && hasDrippings) {
            foreach (int id in _dish.connectedItems) {
                PhotonView view = PhotonView.Find(id);
                if (view != null && view.tag == "shallots") {
                    Plateable p = view.GetComponent<Plateable>();
                    p.Unstick(view.ViewID);
                    if (view.IsMine) {
                        PhotonNetwork.Destroy(view);
                    }
                    hasShallots = true;
                    break;
                }
            }
        }

        if (tag != "pan sauce" && hasShallots && hasDrippings && hasWine) {
            tag = "pan sauce";
            liquid.liquidMaterial = sauceMaterial;

        }
        
    }


public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hasShallots);
            stream.SendNext(hasDrippings);
            stream.SendNext(hasWine);
        }
        else
        {
            hasShallots = (bool)stream.ReceiveNext();
            hasDrippings = (bool)stream.ReceiveNext();
            hasWine = (bool)stream.ReceiveNext();
        }
    }
    
}
