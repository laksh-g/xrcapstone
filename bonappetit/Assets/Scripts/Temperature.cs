using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class Temperature : MonoBehaviour, IPunObservable
{
    [SerializeField]
    public float temp;

    public float tempDelta;

    [SerializeField]
    public float maxTemp;
    public bool inFridge;
    public float k = .25F; // heat transfer coefficient * surface area
    public HeatingElement heater = null;
    // Start is called before the first frame update
    void Start()
    {
       temp = 21;
       inFridge = false; 
    }

    void Update() {
        if (heater != null && heater.s != null && heater.s.val != 0) {
            tempDelta = heater.tempDelta;
        } else {
            tempDelta = ambientDelta();
        }
        temp += tempDelta * Time.deltaTime;
        maxTemp = System.Math.Max(maxTemp, temp); 
    }

    void OnTriggerEnter(Collider other) {
        heater = other.GetComponent<HeatingElement>();
    }

    private void OnTriggerExit(Collider other)
    {
        
        heater = null;
    }

    // Following Newton's law of cooling
    private float ambientDelta() {
            return - k * (temp - (inFridge? 4F : 21F));
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
