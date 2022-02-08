using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookable : MonoBehaviour
{
    public float cookedTemp; // in C
    
    // materials for transitions
    public Material raw;
    public Material cooked;
    public Material burnt;
    private Temperature temp;
    private MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        temp = GetComponent<Temperature>();
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(temp.maxTemp >= cookedTemp * 1.15F) {
            mesh.material = burnt;
        }
        else if(temp.maxTemp >= cookedTemp) {
            mesh.material.Lerp(cooked, burnt, (temp.maxTemp - cookedTemp) / (cookedTemp * .15f));
        } else {
            // transition between textures
            mesh.material.Lerp(raw, cooked, temp.maxTemp / cookedTemp);
        }
    }

    public string GetStatus() {
        if (temp.maxTemp >= cookedTemp * 1.15F) {
            return "Overdone";
        }
        if (temp.maxTemp >= cookedTemp) {
            return "Done";
        }
        return "Underdone";
    }
}
