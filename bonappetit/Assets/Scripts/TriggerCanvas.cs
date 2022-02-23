using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerCanvas : MonoBehaviour
{
    public XRNode node;
    public ButtonOption button;

    InputDevice device;
    InputFeatureUsage<bool> inputFeature;
    CanvasGroup canvasGroup;
    bool isPressed;
    bool wasPressed;
    bool isShowing;

    static readonly Dictionary<string, InputFeatureUsage<bool>> availableButtons = new Dictionary<string, InputFeatureUsage<bool>>
    {
        {"triggerButton", CommonUsages.triggerButton },
        {"primary2DAxisClick", CommonUsages.primary2DAxisClick },
        {"primary2DAxisTouch", CommonUsages.primary2DAxisTouch },
        {"menuButton", CommonUsages.menuButton },
        {"gripButton", CommonUsages.gripButton },
        {"secondaryButton", CommonUsages.secondaryButton },
        {"secondaryTouch", CommonUsages.secondaryTouch },
        {"primaryButton", CommonUsages.primaryButton },
        {"primaryTouch", CommonUsages.primaryTouch },
    };

    public enum ButtonOption
    {
        triggerButton,
        primary2DAxisClick,
        primary2DAxisTouch,
        menuButton,
        gripButton,
        secondaryButton,
        secondaryTouch,
        primaryButton,
        primaryTouch
    };

    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(node);
        canvasGroup = GetComponent<CanvasGroup>();
        isShowing = canvasGroup.alpha >= 1;

        string featureLabel = Enum.GetName(typeof(ButtonOption), button);
        availableButtons.TryGetValue(featureLabel, out inputFeature);
    }

    // Update is called once per frame
    void Update()
    {
        device.TryGetFeatureValue(inputFeature, out isPressed);

        bool active = isPressed && !wasPressed;
        if (active) 
        {
            isShowing = !isShowing;
            canvasGroup.alpha = isShowing ? 1 : 0;
        }

        wasPressed = isPressed;
    }
}
