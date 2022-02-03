using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public Transform dispensePoint = null;
    public GameObject dispensablePrefab = null;
    public int batchSize = 20;
    public float timeout = 10;
    public bool isActivated = false;
    private bool isTimeout = false;
        
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated) {
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
                Instantiate(dispensablePrefab, dispensePoint.position, Quaternion.identity);
            }
            isTimeout = true;
        }
    }
}
