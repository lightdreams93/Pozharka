using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    
    private void Update()
    {
        if (!GameManager.isLevelStarted) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _pausePanel.SetActive(!_pausePanel.activeSelf);
            if(_pausePanel.activeSelf)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
