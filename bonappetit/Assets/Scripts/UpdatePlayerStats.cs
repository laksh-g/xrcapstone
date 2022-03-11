using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class UpdatePlayerStats : MonoBehaviour
{
    public GetFinalScore f = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("waitForScore");
        updatePlayerStats();
    }

    private IEnumerator waitForScore() {
        while (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("score")) {
            yield return null;
        }
    }
    void updatePlayerStats() {
        float curr_score = (float) PhotonNetwork.CurrentRoom.CustomProperties["score"];

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
