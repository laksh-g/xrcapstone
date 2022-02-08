using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dispenser : MonoBehaviour
{
    public Transform dispensePoint = null;
    public GameObject dispensablePrefab = null;
    public int batchSize = 20;
    public float timeout = 10;
    public bool isActivated = false;
    private bool isTimeout = false;
    
    private AudioSource a = null;
    public AudioClip dispenseSound = null;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
        if (a == null) {
            a = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated) {
            a.PlayOneShot(dispenseSound);
            Dispense();
            isActivated = false;
            isTimeout = true;
        }

        if(isTimeout) {
            timer += Time.deltaTime;
        }

        if (timer >= timeout) {
            timer = 0;
            isTimeout = false;
        }
        
    }

    public void Dispense() {
        if (!isTimeout) {
            for(int i = 0; i < batchSize; i++) {
                PhotonNetwork.Instantiate(dispensablePrefab.name, dispensePoint.position, Quaternion.identity);
            }
            isTimeout = true;
        }
    }
}
