using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    public float currentVolume; // in mL
    public float capacity; // in mL
    public bool isFillable = false;
    public Material liquidMaterial = null;
    public Transform liquidStart = null;
    public Transform liquidEnd = null;
    public GameObject liquid = null;
    private MeshRenderer liquidMesh = null;

    // Start is called before the first frame update
    void Start()
    {
        if (isFillable) {
            liquidMesh = liquid.GetComponent<MeshRenderer>();
            liquidMesh.enabled = false;
            liquid.transform.position = liquidStart.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isFillable) {
            if (getPercentage() > 0.1) {
                if (liquidMaterial != null && liquidMesh.material != liquidMaterial) {
                    liquidMesh.material = liquidMaterial;
                }
                liquidMesh.enabled = true;
                liquid.transform.position = Vector3.Lerp(liquidStart.position, liquidEnd.position, getPercentage());
            }
        }
    }

    private float getPercentage() {
        return currentVolume / capacity;
    }
}
