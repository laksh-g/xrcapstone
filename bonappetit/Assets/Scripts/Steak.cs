using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : MonoBehaviour
{
    // metadata used for quality checking
    public float salt; // in grams
    public float pepper; // in grams
    public float restTime; // in seconds
    public bool isResting; // true when steak was just taken off heating source

    public Material raw;
    public Material done;
    public Material burnt;

    
    private float searTime = 0;
    private MeshRenderer steakMesh;

    private Temperature temp = null;
    private HeatingElement heater = null;
    public readonly float neededRest = 300f; // in seconds
    public readonly float[] donenessTemps = {48, 52, 54, 60, 66, 71}; // in Celsius
    public readonly string[] donenessLabels = {"Blue", "Rare", "Medium Rare", "Medium", "Medium Well", "Well Done"}; 

    // Start is called before the first frame update
    void Start()
    {
        restTime = 0;
        steakMesh = GetComponent<MeshRenderer>();
        temp = GetComponent<Temperature>();

    }

    void Update() {
        if (isResting) {
            restTime += Time.deltaTime * 4;
        }
        if (heater != null) {
            if (heater.s.val == 3) {
                searTime += Time.deltaTime * 4;
            }
        }
        if (searTime <= 120) {
            steakMesh.material = raw;
        } else if (searTime <= 180){
            steakMesh.material = done;
        } else {
            steakMesh.material = burnt;
        }


    }

    void OnTriggerEnter(Collider other) {
        isResting = false;
        heater = other.GetComponent<HeatingElement>();
    }

    void OnTriggerExit(Collider other) {
        heater = null;
        isResting = true;
    }

    public string GetDonenessLabel() {
        for (int i = donenessLabels.Length; i >= 0; i--) {
            if (temp.maxTemp > donenessTemps[i]) {
                return donenessLabels[i];
            }
        }
        return "Raw";
    }


}
