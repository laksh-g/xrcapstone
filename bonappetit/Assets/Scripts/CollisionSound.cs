using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource a;
    public AudioClip collisionSound = null;
    private float threshold = 1.5f;

    void Awake() {
        a = GetComponent<AudioSource>();
        if (a == null) {
            a = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision other) {
        if (collisionSound != null && other.relativeVelocity.magnitude > threshold) {
            a.PlayOneShot(collisionSound);
            Debug.Log("Collision sounded with magnitude:" + other.relativeVelocity.magnitude);
        }
    }
}
