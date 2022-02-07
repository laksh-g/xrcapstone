using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class GetInfo : MonoBehaviour
{
    GameObject text;

    XRBaseInteractable baseInteractable;

    //public void UpdateTextInfo() {
    //text.GetComponent<TextInformation>().UpdateSelected(gameObject);
    //}

    protected void OnEnable()
    {
        baseInteractable = GetComponent<XRBaseInteractable>();

        baseInteractable.hoverEntered.AddListener(OnHoverEntered);
    }

    protected void OnDisable()
    {
        baseInteractable.hoverEntered.RemoveListener(OnHoverEntered);
    }

    protected virtual void OnHoverEntered(HoverEnterEventArgs args)
    {
        // get left hand controller
        text = args.interactorObject.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        text.GetComponent<TextInformation>().UpdateSelected(gameObject);
    }
}
