using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerCanvas : MonoBehaviour
{

    InputDevice device;
    CanvasGroup canvasGroup;
    bool isPressed;
    bool wasPressed;
    bool isShowing;

    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        canvasGroup = GetComponent<CanvasGroup>();
        isShowing = canvasGroup.alpha >= 1;
    }

    // Update is called once per frame
    void Update()
    {
        device.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);

        bool active = isPressed && !wasPressed;
        if (active) 
        {
            isShowing = !isShowing;
            canvasGroup.alpha = isShowing ? 1 : 0;
        }

        wasPressed = isPressed;
    }
}
