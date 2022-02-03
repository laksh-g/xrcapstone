using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    Seasonable target = null;
    bool isPouring = false;
    Rigidbody r = null;

    private ParticleSystem p = null;
    private readonly float pourRate = 0.05f; // in grams
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        p = GetComponentInChildren<ParticleSystem>();
    }

    void FixedUpdate() {
        if (isPouring) {
            CheckHit();
            if (target != null) {
                if (tag == "salt") {
                    target.salt += pourRate;
                } else if (tag == "pepper") {
                    target.pepper += pourRate;
                } else if (tag == "parsley") {
                    target.parsley += pourRate;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
            float threshold = 90;
            bool check = (CalculatePourAngle() > threshold) && (r.velocity.y < -1);
        
            if (isPouring != check) {
                isPouring = check; 
                if (isPouring) {
                    print("Pouring!");
                    p.Play();
                } else {
                    p.Stop();
                    target = null;
                }
            }
    }

    private float CalculatePourAngle() {
        float zAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
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
