using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdatePlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        updatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updatePlayerStats() {
        TextMeshProUGUI curr_score_text = GameObject.Find("Final Score").GetComponent<TextMeshProUGUI>();
        float curr_score = float.Parse(curr_score_text.text);

        float high_score = PlayerPrefs.GetFloat("HighScore");

        if (curr_score > high_score) {
            PlayerPrefs.SetFloat("HighScore", curr_score);
        }

        float games_played = PlayerPrefs.GetFloat("TotalGamesPlayed");
        PlayerPrefs.SetFloat("TotalGamesPlayed", games_played + 1);
        
        float games_score = PlayerPrefs.GetFloat("TotalGamesScore");
        PlayerPrefs.SetFloat("TotalGamesScore", games_score + curr_score);
    }
}
