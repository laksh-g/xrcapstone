using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private Transform hookPosition;
    [SerializeField] private Transform tubPosition;
    private Rigidbody body;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BeginHold() {
        body.isKinematic = false;
        body.detectCollisions = true;

    }

    public void EndHold() {
        if (isCloseTo(hookPosition)) {
            body.isKinematic = true;
            transform.position = hookPosition.position;
        } else if (isCloseTo(tubPosition)) {
            body.isKinematic = true;
            transform.position = tubPosition.position;
        }

    }

    private bool isCloseTo(Transform target) {
        return Mathf.Abs(transform.position.x - target.position.x) < .05F 
        && Mathf.Abs(transform.position.y - target.position.y) < .075F 
        && Mathf.Abs(transform.position.z - target.position.z) < .1F;
    }
}
