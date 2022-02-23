using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderReceipt : MonoBehaviour
{
    public bool isStuck;

    public int cachedPosition;

    public Printable orderInfo;

    private TextMeshPro orderNum;

    private TextMeshPro orderDesc;

    private Rigidbody rb;

    

    void Start()
    {
        isStuck = true;
        cachedPosition = 0;
        orderInfo = GetComponent<Printable>();
        rb = GetComponent<Rigidbody>();

        float ht = 0f;

        foreach (Transform child in gameObject.transform)
        {
            GameObject target = child.gameObject;
            if (target.tag == "order_num_label")
            {
                orderNum = target.GetComponent<TextMeshPro>();
                orderNum.text = "Order No. " + orderInfo.orderNum;

                orderNum.ForceMeshUpdate();
                ht += orderNum.GetRenderedValues().y;
            }
            else if (target.tag == "order_desc_label")
            {
                orderDesc = target.GetComponent<TextMeshPro>();
                orderDesc.text = orderInfo.orderString;

                orderDesc.ForceMeshUpdate();
                ht += orderDesc.GetRenderedValues().y;
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            GameObject target = child.gameObject;
            if (target.tag == "order_receipt_container") {
                
                if (ht > 0) {
                    ScaleAround(
                        target,
                        new Vector3(0f, 0f, 0.36f),
                        new Vector3(1f, 1f, (1.1f + (ht)))
                    );
                }

                Debug.Log("CARD");
                Debug.Log(ht);
                // z >>
                // Debug.Log(target.GetComponent<Renderer>().bounds.center);
                // // z >>
                // Debug.Log(target.GetComponent<Renderer>().bounds.extents);
                // Debug.Log(target.GetComponent<Renderer>().bounds.max);
                // Debug.Log();
                // Debug.Log(target.GetComponent<Renderer>().bounds.size);
            }
        }
    }

    private void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;
    
        Vector3 C = A - B; // diff from object pivot to desired pivot/origin
    
        float RS = newScale.z / target.transform.localScale.z; // relataive scale factor
    
        // calc final position post-scale
        Vector3 FP = B + C * RS;
    
        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }

    public void SelectEffectEnter()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (!rb.isKinematic)
            rb.isKinematic = true;
    }

    public void SelectEffectExit()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (rb.isKinematic)
            rb.isKinematic = false;
    }

    void OnTriggerEnter (Collider other) {
        if(!isStuck && other.gameObject.tag == "ticket_line") {
            isStuck = true;

            GameObject spawner = GameObject.FindGameObjectsWithTag("ticket_spawner")[0];
            gameObject.transform.position = new Vector3(
                spawner.transform.position.x - (.3f * cachedPosition),
                spawner.transform.position.y,
                spawner.transform.position.z
            );

            gameObject.transform.rotation = spawner.transform.rotation;
        }
    }

    public void Unstick() {
        isStuck = false;
    }
}
