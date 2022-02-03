using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool specificTrash = false;
    public string[] acceptedTags = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (specificTrash) {
            if (acceptedTags.Contains(other.gameObject.tag)) {
                Destroy(other.gameObject);
            }
        } else {
            Destroy(other.gameObject);
        }
    }
}
