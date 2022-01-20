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

    public readonly float neededRest = 300f; // in seconds
    public readonly float[] donenessTemps = {48, 52, 54, 60, 66, 71}; // in Celsius
    public readonly string[] donenessLabels = {"Blue", "Rare", "Medium Rare", "Medium", "Medium Well", "Well Done"}; 

    // Start is called before the first frame update
    void Start()
    {
        restTime = 0;

    }

    void Update() {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnTriggerExit(Collider other) {
    }


}
