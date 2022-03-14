using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Temperature))]
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

    public bool automaticRefill = false;
    [Header("Pourable Settings")]
    public bool variablePourRate = false;
    public int pourRateMultiplier = 1;
    public Transform spout = null;
    public GameObject streamPrefab = null;
    private bool isPouring = false;
    private Stream stream = null;
    private MeshRenderer liquidMesh = null;

    private LiquidContainer scooper = null;

    public  Temperature temperature = null;

    private PhotonView _view = null;
    private readonly float baseRate = 0.5f; // the base pour rate
    private readonly float scoopRate = 20.0f; // the rate which liquid can be scooped from this object

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
        temperature = GetComponent<Temperature>();
        if (temperature == null) {
            temperature = gameObject.AddComponent<Temperature>();
        }
        _view = GetComponent<PhotonView>();
    }

    [PunRPC]
    void Pour(bool hastarget, int targetid, float amount) {
        currentVolume = Mathf.Max(0f, currentVolume - amount);
        if (hastarget) {
            PhotonView view = PhotonView.Find(targetid);
            LiquidContainer container = view.GetComponent<LiquidContainer>();
            container.liquidMaterial = liquidMaterial; // inherit material
            container.gameObject.tag = tag; // inherit tag
            container.temperature.temp = (temperature.temp + container.temperature.temp) / 2;
            container.currentVolume = Mathf.Min(container.currentVolume + amount, container.capacity);
        }
    }

    [PunRPC]
    void Scoop(int scooperid) {
        PhotonView view = PhotonView.Find(scooperid);
        LiquidContainer scoop = view.GetComponent<LiquidContainer>();
        if (!isFillable) {
            currentVolume = Mathf.Max(currentVolume - scoopRate, 0f);
        }
        scoop.currentVolume = Mathf.Min(scoop.currentVolume + scoopRate, scoop.capacity);
        scoop.gameObject.tag = tag; // inherit tag
        scoop.temperature.temp = (temperature.temp + scoop.temperature.temp) / 2;
        scoop.liquidMaterial = liquidMaterial;
    }
    void FixedUpdate() {
        if (_view.IsMine && isPourable && isPouring) {
            float pourRate = CalculatePourRate();
            if (stream.container != null && stream.container.currentVolume < stream.container.capacity) {
                Pour(true, stream.container._view.ViewID, pourRate);
                _view.RPC("Pour", RpcTarget.Others, true, stream.container._view.ViewID, pourRate);
            } else {
                Pour(false, -1, pourRate);
                _view.RPC("Pour", RpcTarget.Others, false, -1, pourRate);
            }
        }

        if (scooper != null && currentVolume > 0f && scooper.currentVolume < scooper.capacity) {
            Scoop(scooper._view.ViewID);
            _view.RPC("Scoop", RpcTarget.Others, scooper._view.ViewID);
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

        if (liquidMesh != null && liquidMesh.material != liquidMaterial) {
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

    public void Refill() {
        currentVolume = capacity;
    }

/*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentVolume);
            stream.SendNext(tag);
        }
        else
        {
            currentVolume = (float)stream.ReceiveNext();
            tag = (string)stream.ReceiveNext();
        }
    }
    */
}
