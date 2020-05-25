using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartedScreen : MonoBehaviour
{
    [SerializeField] private string _quizKey;
    [SerializeField] private GameObject _startedScreen;

    public static bool _showedStartedScreen;

    private void Start()
    {
        if (_showedStartedScreen)
            _startedScreen.SetActive(false);
    }

    public void ShowedStartedScreen()
    {
        _showedStartedScreen = true;
    }

    public void LoadQuizLevel()
    {
        if (!PlayerPrefs.HasKey(_quizKey))
        {
            SceneManager.LoadScene(9);
            return;
        }
             
        string data = PlayerPrefs.GetString(_quizKey);
        StatisticInfo info = JsonUtility.FromJson<StatisticInfo>(data);

        string date = info.currentDate; 
        string[] splitedDate = date.Split('/');

        int day = int.Parse(splitedDate[0]);
        int month = int.Parse(splitedDate[1]);
        int year = int.Parse(splitedDate[2]);

        if (NotToday(year, month, day))
        {
            SceneManager.LoadScene(9);
        }
    }


    private bool NotToday(int year, int month, int day)
    {
        if (year <= DateTime.Now.Year)
        {
            if (month == DateTime.Now.Month)
            {
                if (day < DateTime.Now.Day)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}
