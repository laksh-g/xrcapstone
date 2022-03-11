using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool isShowing;
    TextMeshProUGUI[] texts;
    int index;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        isShowing = canvasGroup.alpha >= 1;

        GameObject panel = transform.Find("Panel").gameObject;
        if (panel != null)
            texts = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
        index = 0;
    }

    public void EnableCanvas()
    {
        isShowing = !isShowing;
        canvasGroup.alpha = isShowing ? 1 : 0;
    }

    public void CycleCanvas(bool forward)
    {
        if (texts == null)
            Debug.LogError("No UI text found");

        texts[index].gameObject.SetActive(false);

        if (forward)
            index = (index + 1) % texts.Length;
        else
            index = (index - 1 + texts.Length) % texts.Length;

        texts[index].gameObject.SetActive(true);
    }
}