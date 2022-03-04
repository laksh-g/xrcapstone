using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoastingController : MonoBehaviour
{
    private Temperature _temp;
    private LiquidContainer liquid;
    public Material drippingMaterial = null;
    // Start is called before the first frame update
    void Start()
    {
        _temp = GetComponent<Temperature>();
        liquid = GetComponent<LiquidContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_temp.temp >= 74 && tag != "pan drippings") {
            liquid.liquidMaterial = drippingMaterial;
            tag = "pan drippings";
            liquid.currentVolume = Mathf.Min(liquid.capacity, liquid.currentVolume + 200);
        }
    }
}
