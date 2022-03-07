using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceBuilder : MonoBehaviour
{
    public bool hasDrippings = false;
    public bool hasShallots = false;
    public bool hasWine = false;

    public Transform shallotTransform = null;

    public Temperature _temp = null;
    public Material sauceMaterial = null;
    private LiquidContainer liquid = null;

    void Start() {
        liquid = GetComponent<LiquidContainer>();
        _temp = GetComponent<Temperature>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDrippings && tag == "pan drippings") {
            hasDrippings = true;
        } else if (!hasWine && tag == "chardonnay") {
            hasWine = true;
        }
        
        if (!hasShallots && shallotTransform.tag == "occupied" && _temp.maxTemp > 60 && hasDrippings) {
            foreach (Transform t in transform) {
                if (t.tag == "shallots") {
                    Destroy(t.gameObject);
                    hasShallots = true;
                }
            }
        }

        if (tag != "pan sauce" && hasShallots && hasDrippings && hasWine) {
            tag = "pan sauce";
            liquid.liquidMaterial = sauceMaterial;

        }
        
    }
}
