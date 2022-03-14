using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class Temperature : MonoBehaviour, IPunObservable
{
    [SerializeField]
    public float temp = 21;

    public float tempDelta;

    [SerializeField]
    public float maxTemp;
    private bool inFridge = false;
    private float k = .02F; // heat transfer coefficient * surface area
    public HeatingElement heater = null;

    private float restTime = 0; // temp will continue to rise for a period after coming off heat
    private bool isResting = false;

    private int cachedVal = 0;

    private PhotonView _view = null;

    void Awake() {
        _view = GetComponent<PhotonView>();
    }

    void Update() {
        if (isResting && restTime < 10f) {
            restTime += Time.deltaTime;
        } else {
            isResting = false;
            restTime = 0;
        }

        if (heater != null && heater.s != null) {
            if (heater.s.val == 0 && cachedVal > 0) {
                isResting = true;
            } else if (isResting && heater.s.val > 0) {
                isResting = false;
                restTime = 0;
            }
            cachedVal = heater.s.val;
        }
        tempDelta = SetDelta();
        if (_view.IsMine) {
            temp += tempDelta * Time.deltaTime;
            maxTemp = System.Math.Max(maxTemp, temp); 
        }
    }

    private float SetDelta() {
        if (isResting) {
            return restDelta();
        } else if (heater != null) {
            return heaterDelta();
        }
        return ambientDelta();
    
    }

    private float heaterDelta() {
        if (heater.s == null) {
            return 4f;
        }
        if (heater.s.val == 0) {
            Debug.Log("registering ambient");
            return ambientDelta();
        } else if(heater.isOvenlike) {
            Debug.Log("registering oven with setting " + 1);
            return delta(heater.temperatureSettings[heater.s.val]);
        } else if (heater.s.numSettings == 4) {
            switch(heater.s.val) {
                case(1):
                return .1f;
                case(2):
                return .5f;
                case(3):
                return 1f;
            } 
        } else if (heater.s.numSettings == 2) {
            switch(heater.s.val) {
                case(1):
                return 4f;
            }
        
        }
        return ambientDelta();
        
    }

    private float restDelta() {
        return delta(maxTemp + 5);
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

    void OnTriggerStay(Collider other) {
        if (heater == null && other.tag == "heater") {
            heater = other.GetComponent<HeatingElement>();
            if (heater != null && (heater.s == null || heater.s.val != 0)) {
                isResting = false;
                restTime = 0;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (heater != null && other.gameObject.tag == "heater") {
            isResting = true;
            heater = null;
        }
        Debug.Log("left area " + other.tag);
    }

    // Following Newton's law of cooling
    private float ambientDelta() {
            return delta(inFridge? 4F : 21F);
    }

    private float delta(float arg) {
        return - k * (temp - arg);
    }

    public float tempInF() {
        return temp * 1.8F + 32;
    }

    public float tempInC() {
        return temp;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(temp);
            stream.SendNext(maxTemp);
        }
        else
        {
            temp = (float)stream.ReceiveNext();
            maxTemp = (float)stream.ReceiveNext();
        }
    }


}
