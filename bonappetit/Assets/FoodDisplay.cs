using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FoodDisplay : MonoBehaviour
{
    List<GameObject[]> highlightObjs;
    List<Color> startColor;
    List<Renderer> currRenders;

    //int itemIdx = 0;

    readonly float blinkTime = 0.1f;
    float elapsed;

    // Start is called before the first frame update
    void Start()
    {
        highlightObjs = new List<GameObject[]>();
        highlightObjs.Add(new GameObject[] { GameObject.Find("Table Bread") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Steak Frites") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Roast Chicken") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("French Onion Soup") });
        highlightObjs.Add(new GameObject[] { GameObject.Find("Crab Cakes") });

        startColor = new List<Color>();
        currRenders = new List<Renderer>();
        for(int itemIdx = 0; itemIdx < highlightObjs.Count; itemIdx++){
            foreach (var obj in highlightObjs[itemIdx])
            {
                if (obj == null)
                    continue;

                if (obj.GetComponent<Renderer>() != null)
                {
                    startColor.Add(obj.GetComponent<Renderer>().material.color);
                    currRenders.Add(obj.GetComponent<Renderer>());
                }

                Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renders)
                {
                    startColor.Add(r.material.color);
                    currRenders.Add(r);
                }
            }
            
            elapsed = blinkTime;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Blink();
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        var interactorObj = args.interactorObject.transform.gameObject;
        Debug.Log("Selected");    
    }

    void Blink()
    {
        elapsed += Time.deltaTime;

        for (int i = 0; i < startColor.Count; i++)
        {
            Color currentColor = Color.Lerp(startColor[i], Color.yellow, Mathf.Cos(elapsed / blinkTime) * 0.7f + 0.3f);
            currRenders[i].material.color = currentColor;
        }
    }
}
