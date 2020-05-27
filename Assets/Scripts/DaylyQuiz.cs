using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DaylyQuiz : MonoBehaviour
{
    [SerializeField] private string _quizKey;
    [SerializeField] private int _quizScene;

    public void LoadQuizLevel()
    {
        if (!PlayerPrefs.HasKey(_quizKey))
        {
            SceneManager.LoadScene(_quizScene);
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
            SceneManager.LoadScene(_quizScene);
            return;
        }

        SceneManager.LoadScene(0);
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
