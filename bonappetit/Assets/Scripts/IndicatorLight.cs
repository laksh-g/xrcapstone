using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLight : MonoBehaviour
{
    MeshRenderer _mesh = null;
    private Material offMat;
    public Material onMat;

    public bool on = false;
    // Start is called before the first frame update
    void Start()
    {
       _mesh = GetComponent<MeshRenderer>(); 
       offMat = _mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (on && _mesh.material != onMat) {
            _mesh.material = onMat;
        } else if (!on && _mesh.material != offMat) {
            _mesh.material = offMat;
        }
    }
}
