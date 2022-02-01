using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoopable : MonoBehaviour
{
    private LiquidContainer scooper = null;
    private LiquidContainer container = null;

    // Start is called before the first frame update
    void Start()
    {
        container = GetComponent<LiquidContainer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scooper != null && container.currentVolume > 0f && scooper.currentVolume < scooper.capacity) {
            container.currentVolume = Mathf.Max(container.currentVolume - 0.5f, 0f);
            scooper.currentVolume = Mathf.Min(scooper.currentVolume + 0.5f, scooper.capacity);
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "scooper") {
            scooper = other.GetComponentInParent<LiquidContainer>();
            scooper.liquidMaterial = container.liquidMaterial;
        }
    }

    void OnTriggerExit(Collider other) {
        scooper = null;
    }
}
