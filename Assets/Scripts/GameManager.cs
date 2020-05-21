using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winPanel;

    public static event Action OnGameWin; 

    public static bool isLevelStarted;

    private TodoList[] _todoLists;

    private int _countTasks;

    public static List<bool> winList;

    private void Start()
    {
        winList = new List<bool>();
        winList.Clear();

        isLevelStarted = false;

        _todoLists = FindObjectsOfType<TodoList>();
        _countTasks = _todoLists.Length;

        Healthbar.OnVictumDie += Healthbar_OnVictumDie;
        TodoList.OnAllTaskDone += TodoList_OnAllTaskDone;
    } 

    public void StartLevel()
    {
        isLevelStarted = true;
    }

    private void TodoList_OnAllTaskDone()
    {
        if (CheckWin())
        {
            _winPanel.SetActive(true);
            OnGameWin?.Invoke();
        }
    }

    private bool CheckWin()
    {
        if (winList.Count == _countTasks)
            return true;

        return false;
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

    public void DisableAllObjects(GameObject container)
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void EnableAllObjects(GameObject container)
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void DestroyGameObject(GameObject obj)
    {
        Destroy(obj);
    }
}
