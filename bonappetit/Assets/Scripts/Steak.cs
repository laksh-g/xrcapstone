using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : MonoBehaviour
{
    // metadata used for quality checking
    public Seasonable seasoning = null;
    public float restTime; // in seconds
    public bool isResting; // true when steak was just taken off heating source
    public GameObject smokePrefab;
    private ParticleSystem smokeInstance;
    public Material raw;
    public Material done;
    public Material burnt;

    public AudioClip sizzle;

    private AudioSource a;

    
    public float searTime = 0;
    private MeshRenderer steakMesh;

    public Temperature temp = null;
    private HeatingElement heater = null;
    public readonly float[] donenessTemps = {48, 52, 54, 60, 66, 71}; // in Celsius
    public static string[] donenessLabels = {"Blue", "Rare", "Medium Rare", "Medium", "Medium Well", "Well Done"}; 

    // Start is called before the first frame update
    void Start()
    {
        restTime = 0;
        steakMesh = GetComponent<MeshRenderer>();
        seasoning = GetComponent<Seasonable>();
        temp = GetComponent<Temperature>();

    }

    void Update() {
        if (isResting) {
            restTime += Time.deltaTime * 4;
        }
        if (heater != null) {
            if (heater.s.val == 3) {
                searTime += Time.deltaTime * 4;
                if (smokeInstance == null) {
                    smokeInstance = Instantiate(smokePrefab, transform.position, Quaternion.Euler(-90, 0, 0), transform).GetComponent<ParticleSystem>();
                }
            } else if (smokeInstance != null) {
                smokeInstance.Stop();
                Destroy(smokeInstance);
            }
        } else if (smokeInstance != null) {
            smokeInstance.Stop();
            Destroy(smokeInstance);
        }
        if (searTime <= 120) {
            steakMesh.material.Lerp(raw, done, searTime/120);
        } else if (searTime <= 180){
            steakMesh.material.Lerp(done, burnt, (searTime - 120)/60);
        } else {
            steakMesh.material = burnt;
        }


    }

    void OnTriggerEnter(Collider other) {
        isResting = false;
        heater = other.GetComponent<HeatingElement>();
    }

    void OnTriggerExit(Collider other) {
        if (heater != null) {
            isResting = true;
            heater = null;
        }
    }

    public string GetDonenessLabel() {
        int d = GetDonenessValue();
        if (d == -1) {
            return "Raw";
        }
        return donenessLabels[d];
    }

    public int GetDonenessValue() {
        for (int i = donenessLabels.Length - 1; i >= 0; i--) {
            if (temp.maxTemp > donenessTemps[i]) {
                return i;
            }
        }
        return -1;
    }


}
