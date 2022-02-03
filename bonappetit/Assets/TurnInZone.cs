using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInZone : MonoBehaviour
{
    public GameManager gm = null;

    public HashSet<Plate> contents = new HashSet<Plate>();
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
            contents.Add(other.gameObject.GetComponent<Plate>());
        }
        if (other.tag == "order") {
            TurnIn(other.GetComponent<Printable>().orderNum);
        }
    }

    void TurnIn(int orderNum) {
        float score;
        string comments;
        if (contents != null && contents.Count > 0) {
            (score, comments) = gm.EvaluateOrder(contents, orderNum);
            print(score + " " + comments);
            foreach (Plate child in contents) {
                Destroy(child.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "plate") {
            contents.Remove(other.GetComponent<Plate>());
        }
    }
}
