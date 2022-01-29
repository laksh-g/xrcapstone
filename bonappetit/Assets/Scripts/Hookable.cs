using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    [SerializeField] private GameObject hooksFolder;
    private Rigidbody body;

    private Transform[] hooks;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        hooks = hooksFolder.GetComponentsInChildren<Transform>();
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
        foreach(Transform hook in hooks) {
            if (hook != hooksFolder.transform && isCloseTo(hook)) {
                body.isKinematic = true;
                transform.position = hook.position;
                transform.rotation = hook.rotation;
                return;
            }
        }
        body.isKinematic = false;
    }

    private bool isCloseTo(Transform target) {
        return Mathf.Abs(transform.position.x - target.position.x) < .1F 
        && Mathf.Abs(transform.position.y - target.position.y) < .1F 
        && Mathf.Abs(transform.position.z - target.position.z) < .1F;
    }
}
