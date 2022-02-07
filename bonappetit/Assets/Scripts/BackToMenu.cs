using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public string scene;

    public void NextScene()
    {
        SceneManager.LoadScene(scene);
    }
}