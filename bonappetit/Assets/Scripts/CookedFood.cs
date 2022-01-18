using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedFood : MonoBehaviour
{
    public Material afterCooked;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool isCooked = GetComponentInParent<Temperature>().isCooked;
        if (isCooked) {
            GetComponent<MeshRenderer>().material = afterCooked;
        }
    }
}
