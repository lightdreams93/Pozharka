using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winPanel;

    public static bool isLevelStarted;

    private void Start()
    {
        isLevelStarted = false;

        Healthbar.OnVictumDie += Healthbar_OnVictumDie;
        TodoList.OnAllTaskDone += TodoList_OnAllTaskDone;
    } 

    public void StartLevel()
    {
        isLevelStarted = true;
    }

    private void TodoList_OnAllTaskDone()
    {
        _winPanel.SetActive(true);
    }

    private void Healthbar_OnVictumDie()
    {
        _gameOverPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        Healthbar.OnVictumDie -= Healthbar_OnVictumDie;
        TodoList.OnAllTaskDone -= TodoList_OnAllTaskDone;
    }

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

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
