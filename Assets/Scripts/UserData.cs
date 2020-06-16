using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string localID;
    public string email;

    public string userName;
    public string userSurname;

    public string keyWord;
    public int timeGame;

    public QuizData startQuiz;
    public QuizData examQuiz;

    public UserData(string userName, string userSurname, string keyWord)
    {
        startQuiz = new QuizData();
        examQuiz = new QuizData();

        this.userName = userName;
        this.userSurname = userSurname;
        this.keyWord = keyWord;
    }
}

[System.Serializable]
public class QuizData
{
    public List<StatisticInfo> statistics;

    public QuizData()
    {
        statistics = new List<StatisticInfo>();
    }
}
