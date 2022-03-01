using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveFire : MonoBehaviour
{
    public GameObject fire;
    public Transform lowTransform;
    public Transform midTransform;
    public Transform highTransform;
    public Material lowFireMaterial;
    public Material highFireMaterial;


    private HeatingElement element;
    private MeshRenderer fireMesh;
    private int cachedSetting;

    private Vector3 lowPos;
    private Vector3 midPos;
    private Vector3 highPos;
    // Start is called before the first frame update
    void Start()
    {
        fireMesh = fire.GetComponent<MeshRenderer>();
        element = GetComponentInParent<HeatingElement>();
        cachedSetting = -1;
        lowPos = new Vector3(fire.transform.position.x, lowTransform.position.y, fire.transform.position.z);
        midPos = new Vector3(fire.transform.position.x, midTransform.position.y, fire.transform.position.z);
        highPos = new Vector3(fire.transform.position.x, highTransform.position.y, fire.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(element.s.val != cachedSetting) {
            cachedSetting = element.s.val;
            switch(cachedSetting) {
                case(0):
                    fireMesh.enabled = false;
                break;
                case(1):
                fireMesh.material = lowFireMaterial;
                fire.transform.position = lowPos;
                fireMesh.enabled = true;
                break;
                case(2):
                fireMesh.material = highFireMaterial;
                fire.transform.position = midPos;
                fireMesh.enabled = true;
                break;
                case(3):
                fireMesh.material = highFireMaterial;
                fire.transform.position = highPos;
                fireMesh.enabled = true;
                break;
            }
        }
    }
}
