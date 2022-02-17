using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grater : MonoBehaviour
{
    public float threshold = 0.1f;
    private Seasonable target = null;
    private bool isPouring = false;
    private Rigidbody r = null;

    private ParticleSystem p = null;

    private Rigidbody gratedObj = null;
    private AudioSource a = null;
    public AudioClip shakeSound = null;
    private readonly float pourRate = 0.05f; // in grams
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        p = GetComponentInChildren<ParticleSystem>();
        a = GetComponent<AudioSource>();
    }
    void Update()
    {
            bool check = gratedObj != null && isGrating();
        
            if (isPouring != check) {
                isPouring = check; 
                if (isPouring) {
                    a.PlayOneShot(shakeSound);
                    print("Pouring!");
                    p.Play();
                } else {
                    p.Stop();
                    target = null;
                }
            }
    }


    private bool isGrating() {
        if (gratedObj != null) {
            if (Vector3.Dot(gratedObj.velocity, transform.forward) > threshold) {
                return true;
            }
        }
        return false;
    }

    void OnTriggerStay(Collider other) {
        gratedObj = other.gameObject.GetComponent<Rigidbody>();
    }

    void OnTriggerExit(Collider other) {
        gratedObj = null;
    }
    void FixedUpdate()
    {
        if (isPouring) {
            CheckHit();
            if (target != null) {
                if (tag == "parmesan") {
                    target.salt += pourRate;
                }
            }
        }
    }

    private void CheckHit() {
        RaycastHit hit;
        Ray ray = new Ray(p.transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 10.0f);
        Seasonable s = hit.collider.GetComponent<Seasonable>();
        if (s != null) {
            target = s;
        } else {
            target = null;
        }
    }
}
