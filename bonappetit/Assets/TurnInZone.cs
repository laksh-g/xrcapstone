using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnInZone : MonoBehaviour
{
    public GameManager gm = null;

    public HashSet<GameObject> contents = new HashSet<GameObject>();
    public bool activate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activate) {
            TurnIn(1);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "plate") {
            GameObject p = other.gameObject;
            if (p == null) {
                print("can't harvest plate");
            }
            contents.Add(p);
        }
        if (other.tag == "order") {
            TurnIn(other.gameObject.GetComponent<Printable>().orderNum);
        }
    }

    void TurnIn(int orderNum) {
        float score;
        string comments;
        if (contents != null && contents.Count > 0) {
            (score, comments) = gm.EvaluateOrder(contents, orderNum);
            print(score + " " + comments);
            foreach (GameObject child in contents.ToList()) {
                contents.Remove(child);
                Destroy(child);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "plate") {
            contents.Remove(other.gameObject);
        }
    }
}
