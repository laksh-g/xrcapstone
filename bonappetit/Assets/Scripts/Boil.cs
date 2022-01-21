using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boil : MonoBehaviour
{
    [SerializeField]
    private float totalTime = 20.0f;

    private Color initialColor;
    private float elapsed = 0.0f;
    private Transform potTransform;
    private Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        initialColor = GetComponent<Renderer>().material.color;
        currentColor = initialColor;
        potTransform = GameObject.Find("Pot").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - potTransform.position.x) < 0.5 &&
            Mathf.Abs(transform.position.z - potTransform.position.z) < 0.5 &&
            transform.position.y - potTransform.position.y > -0.5)
        {
            elapsed += Time.deltaTime;
            float elapsedPct = elapsed / totalTime;
            currentColor = Color.Lerp(initialColor, Color.red, elapsedPct);
        }
        GetComponent<Renderer>().material.color = currentColor;
    }
}
