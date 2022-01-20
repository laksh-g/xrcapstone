using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{

    public HashSet<MonoBehaviour> objects; // all the items on this plate

    // Start is called before the first frame update
    void Start()
    {
        objects = new System.HashSet<MonoBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
