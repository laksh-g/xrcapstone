using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public string dishID;
    public Transform itemFolder = null;


    public string GetCompletionInfo()
    {   string result = "";
        foreach (Transform t in itemFolder) {
            if (t.tag != "occupied") {
                result += "Missing " + t.tag + "\n";
            }
        }
        return result;
    }

    public int GetNumOfMissingComponents() {
        int result = 0;
        foreach (Transform t in itemFolder) {
            if (t.tag != "occupied") {
                result++;
            }
        }
        return result;
    }
}
