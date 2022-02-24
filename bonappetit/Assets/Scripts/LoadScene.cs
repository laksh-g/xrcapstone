using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    public void loadSceme()
    {
        PhotonNetwork.LoadLevel(SceneName);
    }
}
