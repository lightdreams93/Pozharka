using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizStatistic : MonoBehaviour
{
    [SerializeField] private Quiz _quiz;
    private List<Statistic> _stat;

    private StatisticInfo statisticInfo;

    public static event Action<StatisticInfo, List<QuizQuestion>> OnQuizStatisticDisplay;

    private List<QuizQuestion> _quizQuestions;

    private void Start()
    {
        _quizQuestions = _quiz.QuizQuestionsSelected;

        statisticInfo = new StatisticInfo();
        statisticInfo.currentDate = GetDate();
        Quiz.OnAnswerSelected += Quiz_OnAnswerSelected;
        Quiz.OnQuizCompleted += Quiz_OnQuizCompleted;
    }

    private void Quiz_OnQuizCompleted()
    {
        string data = JsonUtility.ToJson(statisticInfo);
        Debug.Log(data);
        PlayerPrefs.SetString("QuizStatistic", data);

        OnQuizStatisticDisplay?.Invoke(statisticInfo, _quizQuestions);
    }

    //private void Quiz_OnQuizInit(List<QuizQuestion> quizQuestions)
    //{
    //    _quizQuestions = quizQuestions;

    //    statisticInfo = new StatisticInfo();
    //    statisticInfo.currentDate = GetDate();
    //}

    private void Quiz_OnAnswerSelected(int answer, int rightAnswer, bool isRightAnswer)
    {
        statisticInfo.statistics.Add(new Statistic(_quiz.CurrentQuestion, answer, rightAnswer));
    }

    private string GetDate()
    {
        DateTime dt = DateTime.Now;
        string day = dt.Day.ToString();
        string month = dt.Month.ToString();
        string year = dt.Year.ToString();

        return $"{day}/{month}/{year}";
    }

    private void OnDestroy()
    { 
        Quiz.OnAnswerSelected -= Quiz_OnAnswerSelected;
        Quiz.OnQuizCompleted -= Quiz_OnQuizCompleted;
    }
}

[System.Serializable]
public class StatisticInfo
{
    public string currentDate;
    public List<Statistic> statistics;

    public StatisticInfo()
    {
        statistics = new List<Statistic>();
    }
}

[System.Serializable]
public class Statistic
{
    public int currentQuestion;
    public int answer;
    public int rightAnswer;

    public Statistic(int currentQuestion, int answer, int rightAnswer)
    {
        this.currentQuestion = currentQuestion;
        this.answer = answer;
        this.rightAnswer = rightAnswer;
    }
}
