using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleScreen : MonoBehaviour
{
    public Scale s;
    private TextMeshPro t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (s.reading >= 1000f) {
            t.SetText("{0:2}kg", s.reading);
        } else {
            t.SetText("{0:2}g", s.reading);
        }
    }
}
