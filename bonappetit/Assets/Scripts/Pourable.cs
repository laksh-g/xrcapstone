using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pourable : MonoBehaviour
{
    //public int threshold = 90;
    public float liquidVolume = 750f; // in mL
    public Transform spout = null;
    public GameObject streamPrefab = null;
    public Material liquidMaterial = null;
    private float liquidRemaining; // in mL
    private bool isPouring = false;
    private Stream stream = null;

    // Start is called before the first frame update
    void Start()
    {
        liquidRemaining = liquidVolume;
    }

    void FixedUpdate() {
        if (isPouring) {
            liquidRemaining -= .1F;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float threshold = 100 - 55 * (liquidRemaining / liquidVolume);
        bool check = CalculatePourAngle() > threshold;
        if (isPouring != check) {

            isPouring = check; 
            if (isPouring) {
                print("Start");
                stream = createStream();
                stream.Begin();
            } else {
                print("End");
                stream.End();
                stream = null;
            }
        }
    }

    private Stream createStream() {
        GameObject stream = Instantiate(streamPrefab, spout.position, Quaternion.identity, transform);
        LineRenderer line = stream.GetComponent<LineRenderer>();
        line.material = liquidMaterial;
        return stream.GetComponent<Stream>();
    }

    private float CalculatePourAngle() {
        float zAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.z); 
        float xAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.x);
        return Mathf.Max(zAngle, xAngle);
    }

}
