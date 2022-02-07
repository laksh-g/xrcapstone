using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderReceipt : MonoBehaviour
{
    public Printable orderInfo;

    private TextMeshPro orderNum;

    private TextMeshPro orderDesc;

    private Rigidbody rb;

    void Start()
    {
        orderInfo = GetComponent<Printable>();
        rb = GetComponent<Rigidbody>();

        foreach (Transform child in gameObject.transform)
        {
            GameObject target = child.gameObject;
            if (target.tag == "order_num_label")
            {
                orderNum = target.GetComponent<TextMeshPro>();
                orderNum.text = "Order No. " + orderInfo.orderNum;
            }
            else if (target.tag == "order_desc_label")
            {
                orderDesc = target.GetComponent<TextMeshPro>();
                orderDesc.text = orderInfo.orderString;
            }
        }
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
}
