using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private Vector3 initPos;
    private MeshRenderer mesh;
    private TicketMaster parent;
    public Material matOn;
    public Material matOff;

    void Start()
    {
        initPos = transform.position;
        mesh = GetComponent<MeshRenderer>();
        parent = GetComponent<Transform>().parent.gameObject.GetComponent<TicketMaster>();
    }

    public void SelectEffectEnter()
    {
        transform.position = new Vector3(initPos.x, initPos.y - 0.01f, initPos.z);
        mesh.material = matOn;
    }

    public void SelectEffectExit()
    {
        transform.position = new Vector3(initPos.x, initPos.y, initPos.z);
        mesh.material = matOff;
        pressButton();
    }

    public void pressButton()
    {
        if (gameObject.tag == "print_button")
        {
            StartCoroutine(parent.printNewTicket());
        }
        else if (gameObject.tag == "reprint_button")
        {
            StartCoroutine(parent.reprintTicket());
        }
    }
}
