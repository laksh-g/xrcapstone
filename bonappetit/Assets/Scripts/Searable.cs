using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searable : MonoBehaviour
{

    private HeatingElement heater;

    private Collider cachedCollider;

    private float searTime;
    public float desiredSearTime = 60;

    private MeshRenderer _mesh = null;

    public Material raw;
    public Material cooked;
    public Material burnt;
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _mesh.material = raw;
    }

    // Update is called once per frame
    void Update()
    {
         
        if (heater != null && heater.s != null && heater.s.val == 3) {
            searTime += Time.deltaTime;
            if (searTime >= desiredSearTime * 1.33) {
                _mesh.material = burnt;
            } else if (searTime >= desiredSearTime) {
                _mesh.material = cooked;
            } else {
                _mesh.material = raw;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        heater = other.GetComponent<HeatingElement>();
        if (heater != null) {
            cachedCollider = other;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other == cachedCollider) {
            heater = null;
            cachedCollider = null;
        }
    }
}
