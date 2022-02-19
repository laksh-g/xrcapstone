using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class Steak : MonoBehaviour, IPunObservable
{
    // metadata used for quality checking
    public Seasonable seasoning = null;
    public float restTime; // in seconds
    [SerializeField]
    public bool isResting; // true when steak was just taken off heating source
    public GameObject smokePrefab;
    private ParticleSystem smokeInstance;
    public Material raw;
    public Material done;
    public Material burnt;

    public AudioClip sizzle;

    private AudioSource a;

    [SerializeField]
    public float searTime = 0;
    private MeshRenderer steakMesh;

    public Temperature temp = null;

    private HeatingElement heater = null;
    public readonly float[] donenessTemps = {48, 52, 54, 60, 66, 71}; // in Celsius
    public static string[] donenessLabels = {"Blue", "Rare", "Medium Rare", "Medium", "Medium Well", "Well Done"}; 

    // Start is called before the first frame update
    void Start()
    {
        restTime = 0;
        steakMesh = GetComponent<MeshRenderer>();
        seasoning = GetComponent<Seasonable>();
        temp = GetComponent<Temperature>();
        a = GetComponent<AudioSource>();

    }

    void Update() {
        if (isResting) {
            restTime += Time.deltaTime * 4;
        }
        if (heater != null && heater.s != null) {
            if (heater.s.val > 0 && !a.isPlaying) {a.Play();}
            a.volume = .5f;
            if ((heater.s.val == 3 && heater.s.numSettings == 4) || (heater.s.val == 1 && heater.s.numSettings == 2)) {
                searTime += Time.deltaTime * 4;
                a.volume = 1f;
                if (smokeInstance == null) {
                    smokeInstance = Instantiate(smokePrefab, transform.position, Quaternion.Euler(-90, 0, 0), transform).GetComponent<ParticleSystem>();
                }
            } else if (smokeInstance != null) {
                smokeInstance.Stop();
                Destroy(smokeInstance);
            }
        } else if (smokeInstance != null) {
            if (a.isPlaying) {a.Stop();}
            smokeInstance.Stop();
            Destroy(smokeInstance);
        } else if (a.isPlaying) {
            a.Stop();
        }
        if (searTime <= 120) {
            steakMesh.material = raw;
        } else if (searTime <= 180){
            steakMesh.material = done;
        } else {
            steakMesh.material = burnt;
        }


    }

    void OnTriggerEnter(Collider other) {
        isResting = false;
        heater = other.GetComponent<HeatingElement>();
    }

    void OnTriggerExit(Collider other) {
        if (heater != null) {
            isResting = true;
            heater = null;
        }
    }

    public string GetDonenessLabel() {
        int d = GetDonenessValue();
        if (d == -1) {
            return "Raw";
        }
        return donenessLabels[d];
    }

    public int GetDonenessValue() {
        for (int i = donenessLabels.Length - 1; i >= 0; i--) {
            if (temp.maxTemp > donenessTemps[i]) {
                return i;
            }
        }
        return -1;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isResting);
            stream.SendNext(searTime);
        }
        else
        {
            isResting = (bool)stream.ReceiveNext();
            searTime = (float)stream.ReceiveNext();
        }
    }


}
