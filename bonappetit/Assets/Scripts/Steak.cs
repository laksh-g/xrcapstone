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
    [SerializeField]
    public float restTime; // in seconds
    public bool isResting; // true when steak was just taken off heating source
    public GameObject smokePrefab;
    private ParticleSystem smokeInstance;
    public Transform raw;
    public Transform done;
    public Transform burnt;

    public AudioClip sizzle;

    private AudioSource a;

    [SerializeField]
    public float searTime = 0;
    private MeshRenderer steakMesh;

    public Temperature temp = null;

    private HeatingElement heater = null;
    public readonly float[] donenessTemps = {48, 52, 54, 60, 66, 71}; // in Celsius
    public static string[] donenessLabels = {"Blue", "Rare", "Medium Rare", "Medium", "Medium Well", "Well Done"}; 

    private PhotonView _view;
    // Start is called before the first frame update
    void Start()
    {
        restTime = 0;
        steakMesh = GetComponent<MeshRenderer>();
        seasoning = GetComponent<Seasonable>();
        temp = GetComponent<Temperature>();
        a = GetComponent<AudioSource>();
        raw = transform.Find("Uncooked");
        done = transform.Find("Cooked");
        burnt = transform.Find("Burnt");
        _view = GetComponent<PhotonView>();

    }

    void Update() {
        if (_view.IsMine && isResting) {
            restTime += Time.unscaledDeltaTime;
        }
        if (heater != null && heater.s != null) {
            if (heater.s.val > 0 && !heater.isCooler && !a.isPlaying) {a.Play();}
            a.volume = .5f;
            if ((heater.s.val == 3 && heater.s.numSettings == 4)) {
                if (_view.IsMine) {
                    searTime += Time.deltaTime;
                }
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
        if (searTime <= 30) {
            raw.gameObject.SetActive(true);
            done.gameObject.SetActive(false);
            burnt.gameObject.SetActive(false);
        } else if (searTime <= 45){
            raw.gameObject.SetActive(false);
            done.gameObject.SetActive(true);
            burnt.gameObject.SetActive(false);
        } else {
            raw.gameObject.SetActive(false);
            done.gameObject.SetActive(false);
            burnt.gameObject.SetActive(true);
        }


    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "heater") {
            heater = other.GetComponent<HeatingElement>();
            if (heater != null && (heater.s == null || heater.s.val != 0)) {
                isResting = false;
                restTime = 0;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (heater != null && other.gameObject == heater.gameObject) {
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
            stream.SendNext(restTime);
            stream.SendNext(searTime);
        }
        else
        {
            restTime = (float)stream.ReceiveNext();
            searTime = (float)stream.ReceiveNext();
        }
    }


}
