using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookable : MonoBehaviour
{
    public float cookedTemp; // in C

    public bool isSearable;

    public MeshRenderer side1;
    public MeshRenderer side2;

    public Material seared = null;
    public float desiredSearTime = 10;
    public  float searTime = 0;
    
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
            if(temp.maxTemp >= cookedTemp * 1.33F) {
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

        if (isSearable) {
            if (temp.heater != null && temp.heater.s.val == temp.heater.s.numSettings - 1) {
                searTime += Time.deltaTime;
            }
            if (searTime > desiredSearTime * 1.5) {
                side1.material = burnt;
                side2.material = burnt;
            } else if (searTime > desiredSearTime) {
                side1.material.Lerp(seared, burnt, (searTime - desiredSearTime) / (desiredSearTime * .5f));
                side2.material.Lerp(seared, burnt, (searTime - desiredSearTime) / (desiredSearTime * .5f));
            } else {
                side1.material.Lerp(raw, seared, searTime / desiredSearTime);
                side2.material.Lerp(raw, seared, searTime / desiredSearTime);
            }
        }
    }

    public string GetStatus() {
        if (temp.maxTemp >= cookedTemp * 1.33F) {
            return "Overdone";
        }
        if (temp.maxTemp >= cookedTemp) {
            return "Done";
        }
        return "Underdone";
    }

    public string GetSearStatus() {
        if (searTime > desiredSearTime * 1.5) {
            return "Burnt";
        } else if (searTime > desiredSearTime) {
            return "Good";
        } else {
            return "Underdone";
        }
    }
}
