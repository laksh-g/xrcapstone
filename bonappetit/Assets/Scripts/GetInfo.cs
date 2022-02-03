using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInfo : MonoBehaviour
{
    public GameObject text;

    public void UpdateTextInfo() {
        text.GetComponent<TextInformation>().UpdateSelected(gameObject);
    }
}
