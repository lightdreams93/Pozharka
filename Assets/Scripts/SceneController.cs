using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int numScene)
    {
        SceneManager.LoadScene(numScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
