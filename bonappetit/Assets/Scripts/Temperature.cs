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
    private HeatingElement heater = null;

    private float restTime = 0; // temp will continue to rise for a period after coming off heat
    private bool isResting = false;

    private int cachedVal = 0;

    void Update() {
        if (isResting && restTime < 10f) {
            restTime += Time.deltaTime;
        } else {
            isResting = false;
            restTime = 0;
        }

        if (heater != null) {
            if (heater.s.val == 0 && cachedVal > 0) {
                isResting = true;
            } else if (isResting && heater.s.val > 0) {
                isResting = false;
                restTime = 0;
            }
            cachedVal = heater.s.val;
        }
        tempDelta = SetDelta();
        temp += tempDelta * Time.deltaTime;
        maxTemp = System.Math.Max(maxTemp, temp); 
    }

    private float SetDelta() {
        if (isResting) {
            return restDelta();
        } else if (heater != null && heater.s != null) {
            return heaterDelta();
        }
        return ambientDelta();
    
    }

    private float heaterDelta() {
        if (heater.s.val == 0) {
            return ambientDelta();
        } else if(heater.isOvenlike) {
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
        heater = other.GetComponent<HeatingElement>();
        if (heater != null && heater.s.val != 0) {
            isResting = false;
            restTime = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (heater != null) {
            isResting = true;
        }
        heater = null;
    }

    // Following Newton's law of cooling
    private float ambientDelta() {
            return delta(inFridge? 4F : 21F);
    }

    private float delta(float arg) {
        return - k * (temp - arg);
    }

    float tempInF() {
        return temp * 1.8F + 32;
    }

    float tempInC() {
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
