using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pourable : MonoBehaviour
{
    public Transform spout = null;
    public GameObject streamPrefab = null;
    private Material liquidMaterial = null;
    private bool isPouring = false;
    private Stream stream = null;
    private LiquidContainer thisContainer = null;

    // Start is called before the first frame update
    void Start()
    {
        thisContainer = GetComponent<LiquidContainer>();
    }

    void FixedUpdate() {
        if (isPouring) {
            print("Pouring");
            thisContainer.currentVolume = thisContainer.currentVolume - 0.1F;
            if (stream.container != null && stream.container.currentVolume < stream.container.capacity) {
                stream.container.liquidMaterial = liquidMaterial;
                stream.container.currentVolume = Mathf.Min(stream.container.currentVolume + .1F, stream.container.capacity);
                print("Filling!");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        float threshold = 100 - 55 * (thisContainer.currentVolume / thisContainer.capacity);
        bool check = CalculatePourAngle() > threshold && thisContainer.currentVolume > 0f;
        
        if (isPouring != check) {

            isPouring = check; 
            if (isPouring) {
                liquidMaterial = thisContainer.liquidMaterial;
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
