using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    [SerializeField] private GameObject hooksFolder;
    [SerializeField] private float threshold;
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
        foreach(Transform hook in hooks) {
            if (hook.position == transform.position) {
                hook.gameObject.tag = "open";
                return;
            }
        }
    }

    public void EndHold() {
        Transform match = null;
        float closestDistance = float.PositiveInfinity;
        float distanceToHook;
        foreach(Transform hook in hooks) {
            distanceToHook = CalculateDistance(hook);
            if (hook != hooksFolder.transform && hook.gameObject.tag == "open" && distanceToHook < threshold
                && distanceToHook < closestDistance) {
                match = hook;
            }
        }
        
        if (match != null) {
            body.isKinematic = true;
            transform.position = match.position;
            transform.rotation = match.rotation;
            match.gameObject.tag = "occupied";
            return;   
        }
        body.isKinematic = false;
    }

    private float CalculateDistance(Transform target) {
        return Mathf.Pow(transform.position.x - target.position.x, 2) + 
        Mathf.Pow(transform.position.y - target.position.y, 2) +
        Mathf.Pow(transform.position.z - target.position.z, 2);
    }
}
