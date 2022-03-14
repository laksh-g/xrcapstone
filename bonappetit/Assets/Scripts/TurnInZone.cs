using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class TurnInZone : MonoBehaviour
{
    public GameManager gm = null;

    public HashSet<Dish> contents = new HashSet<Dish>();
    public bool activate = false;

    public Material green;
    public Material red;

    private bool isRejecting;

    private float rejectTime;

    private MeshRenderer _mesh;
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activate) {
            TurnIn(1);
        }
    }

    void OnTriggerEnter(Collider other) {
        Dish d = other.gameObject.GetComponent<Dish>();
        if (d != null) {
            contents.Add(d);
        }
        if (other.tag == "order") {
            if (contents.Count > 0) {
                TurnIn(other.gameObject.GetComponent<Printable>().orderNum);
                PhotonNetwork.Destroy(other.gameObject);
            } else {
                StartCoroutine(flashRed());
            }
        }
    }

    void TurnIn(int orderNum) {
        float score;
        string comments;
        (score, comments) = gm.EvaluateOrder(contents, orderNum);
        print(score + " " + comments);
        foreach (Dish child in contents.ToList()) {
            contents.Remove(child);
            foreach (int id in child.connectedItems) {
                GameObject g = PhotonView.Find(id).gameObject;
                PhotonNetwork.Destroy(g);
            }
            PhotonNetwork.Destroy(child.gameObject);
        }
    }

    IEnumerator flashRed() {
        _mesh.material = red;
        yield return new WaitForSeconds(.5f);
        _mesh.material = green;

    }

    void OnTriggerExit(Collider other) {
        Dish d = other.gameObject.GetComponent<Dish>();
        if (d != null) {
            contents.Remove(d);
        }
    }
}
