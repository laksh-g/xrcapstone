using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bearnaise : MonoBehaviour
{
    public LiquidContainer container;
    public float heatingTime = 0;
    public bool isSeparated = false;
    public Temperature temp;
    public static float SEPARATION_TEMP = 75f;
    void Start() {
        container = GetComponent<LiquidContainer>();
        temp = GetComponent<Temperature>();
    }

    void Update() {
        if (isSeparated = false && temp.maxTemp > 75f) {
            isSeparated = true;
        }
        if (temp.heater != null) {
            heatingTime += Time.deltaTime;
        }
    }
}
