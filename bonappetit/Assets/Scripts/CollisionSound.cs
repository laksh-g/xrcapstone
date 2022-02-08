using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource a;
    public AudioClip collisionSound = null;
    // Start is called before the first frame update

    void Awake() {
        a = GetComponent<AudioSource>();
        if (a == null) {
            a = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision other) {
        if (collisionSound != null) {
            a.PlayOneShot(collisionSound);
        }
    }
}
