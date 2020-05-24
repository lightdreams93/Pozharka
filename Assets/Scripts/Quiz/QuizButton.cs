using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizButton : MonoBehaviour
{
    [SerializeField] private Button _button; 

    private void Start()
    {
        if (!PlayerPrefs.HasKey("QuizStatistic")) return;

        string data = PlayerPrefs.GetString("QuizStatistic");
        StatisticInfo info = JsonUtility.FromJson<StatisticInfo>(data);

        string date = info.currentDate;
        string[] splitedDate = date.Split('/');

        int day = int.Parse(splitedDate[0]);
        int month = int.Parse(splitedDate[1]);
        int year = int.Parse(splitedDate[2]);

        if(NotToday(year, month, day))
        {
            _button.interactable = true;

        }
        else
        {
            _button.interactable = false;
        }
    }

    private bool NotToday(int year, int month, int day)
    {
        if (year >= DateTime.Now.Year)
        {
            if (month == DateTime.Now.Month)
            {
                if (day > DateTime.Now.Day)
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
