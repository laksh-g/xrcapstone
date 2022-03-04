using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceBuilder : MonoBehaviour
{
    public bool hasDrippings = false;
    public bool hasShallots = false;
    public bool hasWine = false;

    public Transform shallotTransform = null;

    public Temperature parentTemp = null;

    // Update is called once per frame
    void Update()
    {
        if (!hasDrippings && parentTemp.tag == "pan drippings") {
            hasDrippings = true;
        } else if (!hasWine && parentTemp.tag == "chardonnay") {
            hasWine = true;
        }
        
        if (!hasShallots && shallotTransform.tag == "occupied" && parentTemp.maxTemp > 60 && hasDrippings) {
            foreach (Transform t in parentTemp.transform) {
                if (t.tag == "shallots") {
                    Destroy(t.gameObject);
                    hasShallots = true;
                }
            }
        }

        if (parentTemp.tag != "pan sauce" && hasShallots && hasDrippings && hasWine) {
            parentTemp.tag = "pan sauce";
        }
        
    }
}
