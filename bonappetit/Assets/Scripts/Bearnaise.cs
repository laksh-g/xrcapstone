using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bearnaise : MonoBehaviour
{
    public LiquidContainer container;
    public Temperature temp;
    public static float SEPARATION_TEMP = 75f;
    void Start() {
        container = GetComponent<LiquidContainer>();
        temp = GetComponent<Temperature>();
    }
}
