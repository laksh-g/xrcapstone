using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float total_games = PlayerPrefs.GetFloat("TotalGamesPlayed");
        string avg_score_text = "";

        if (total_games == 0) {
            avg_score_text = "0";
        } else {
            avg_score_text = (PlayerPrefs.GetFloat("TotalGamesScore") / total_games).ToString();
        }

        foreach (Transform child in gameObject.transform)
        {
            GameObject target = child.gameObject;
            TextMeshProUGUI tmp = null;
            Debug.Log(target.name);
            if (target.name == "Highest Score") {
                // get highest score
                tmp = target.GetComponent<TextMeshProUGUI>();
                tmp.text = PlayerPrefs.GetFloat("HighScore").ToString();
            } else if (target.name == "Total Games") {
                // get total games
                tmp = target.GetComponent<TextMeshProUGUI>();
                tmp.text = total_games.ToString();
            } else if (target.name == "Average Score") {
                tmp = target.GetComponent<TextMeshProUGUI>();
                tmp.text = avg_score_text;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}