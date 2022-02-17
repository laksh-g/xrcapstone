using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LiquidContainer : MonoBehaviour
{
    [SerializeField]
    public float currentVolume = 0; // in mL
    public float capacity = 0; // in mL
    public bool isFillable = false;
    public bool isPourable = false;
    public Material liquidMaterial = null;
    public GameObject liquid = null;
    [Header("Fillable Settings")]
    public Transform liquidStart = null;
    public Transform liquidEnd = null;
    [Header("Pourable Settings")]
    public bool variablePourRate = false;
    public int pourRateMultiplier = 1;
    public Transform spout = null;
    public GameObject streamPrefab = null;
    private bool isPouring = false;
    private Stream stream = null;
    private MeshRenderer liquidMesh = null;

    private LiquidContainer scooper = null;

    private readonly float baseRate = 0.5f; // the base pour rate
    private readonly float scoopRate = 4.0f; // the rate which liquid can be scooped from this object

    // Start is called before the first frame update
    void Start()
    {
        if (liquid != null) {
            liquidMesh = liquid.GetComponent<MeshRenderer>();
            if (isFillable) {
                liquidMesh.enabled = false;
                liquid.transform.position = liquidStart.position;
            }
        }
    }

    void FixedUpdate() {
        if (isPourable && isPouring) {
            print("Pouring");
            float pourRate = CalculatePourRate();
            currentVolume = Mathf.Max(0f, currentVolume - pourRate);
            if (stream.container != null && stream.container.currentVolume < stream.container.capacity) {
                stream.container.liquidMaterial = liquidMaterial;
                if (tag == "bearnaise" && stream.container.gameObject.GetComponent<Bearnaise>() == null) {
                    stream.container.gameObject.tag = "bearnaise";
                    stream.container.gameObject.AddComponent<Bearnaise>();
                }
                stream.container.currentVolume = Mathf.Min(stream.container.currentVolume + pourRate, stream.container.capacity);
                print("Filling!");
            }

            if (stream.foodItem != null) {
                if (tag == "truffle oil") {
                    stream.foodItem.truffleOil += pourRate;
                }

            }
        }

        if (scooper != null && currentVolume > 0f && scooper.currentVolume < scooper.capacity) {
            scooper.liquidMaterial = liquidMaterial;
            currentVolume = Mathf.Max(currentVolume - scoopRate, 0f);
            scooper.currentVolume = Mathf.Min(scooper.currentVolume + scoopRate, scooper.capacity);
            if (tag == "bearnaise") {
                scooper.gameObject.tag = "bearnaise";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPourable) {
            float threshold = 100 - 55 * (currentVolume / capacity);
            bool check = CalculatePourAngle() > threshold && currentVolume > 0f;
        
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

        if (liquidMesh != null && liquidMaterial != null && liquidMesh.material != liquidMaterial) {
            liquidMesh.material = liquidMaterial;
        }

        if(isFillable) {
            if (getPercentage() > 0.1) {
                liquidMesh.enabled = true;
                // a + (b - a) * t
                liquid.transform.position = Vector3.Lerp(liquidStart.position, liquidEnd.position, getPercentage());
            } else {
                liquidMesh.enabled = false;
            }
        }
    }

    private float getPercentage() {
        return currentVolume / capacity;
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

    private float CalculatePourRate() {
        if (!variablePourRate) {
            return baseRate * pourRateMultiplier;
        }
        return baseRate * 5 * pourRateMultiplier * (CalculatePourAngle() / 180); 
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "scooper") {
            scooper = other.GetComponentInParent<LiquidContainer>();
            scooper.liquidMaterial = liquidMaterial;
        }
    }

    void OnTriggerExit(Collider other) {
        scooper = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentVolume);
        }
        else
        {
            currentVolume = (float)stream.ReceiveNext();
        }
    }
}
