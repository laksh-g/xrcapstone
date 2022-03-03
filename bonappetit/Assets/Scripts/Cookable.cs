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
    public bool isCollection = false;
    private Temperature temp = null;
    private MeshRenderer mesh = null;
    private MeshRenderer[] meshes = null;

    private bool isStaticMaterial = false;

    // Start is called before the first frame update
    void Start()
    {
        temp = GetComponent<Temperature>();
        if (isCollection) {
            meshes = GetComponentsInChildren<MeshRenderer>();
        } else {
            mesh = GetComponent<MeshRenderer>();
        }
        if (burnt == null || cooked == null || raw == null) {
            isStaticMaterial = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStaticMaterial) {
            if(temp.maxTemp >= cookedTemp * 1.15F) {
                if (isCollection) {
                    foreach(MeshRenderer m in meshes) {
                        m.material = burnt;
                    }
                } else {
                    mesh.material = burnt;
                }
            }
            else if(temp.maxTemp >= cookedTemp) {
                if (isCollection) {
                    foreach(MeshRenderer m in meshes) {
                        m.material.Lerp(cooked, burnt, (temp.maxTemp - cookedTemp) / (cookedTemp * .15f));
                    }
                } else {
                    mesh.material.Lerp(cooked, burnt, (temp.maxTemp - cookedTemp) / (cookedTemp * .15f));
                }
            } else {
                if (isCollection) {
                    foreach(MeshRenderer m in meshes) {
                        m.material.Lerp(raw, cooked, temp.maxTemp / cookedTemp);
                    }
                } else {
                // transition between textures
                mesh.material.Lerp(raw, cooked, temp.maxTemp / cookedTemp);
                }
            }
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
