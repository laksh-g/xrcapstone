using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bearnaise : MonoBehaviour
{
    public float volume; // in ounces
    MeshRenderer mesh;
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        t  = GetComponent<Transform>();
        mesh.enabled = false;
        volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (volume > 1) {
            mesh.enabled = true;
            t.localPosition = new Vector3(0f, .09F, 0f);
        } if (volume > 2) {
            t.localPosition = new Vector3(0f, .144F, 0f);
        } if (volume > 3) {
            t.localPosition = new Vector3(0f, .199F, 0f);
        } 
    }
}
