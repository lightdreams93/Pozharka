using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuizType
{
    START_QUIZ,
    EXAM_QUIZ
}

public class QuizStatistic : MonoBehaviour
{

    [SerializeField] private string _quizKey;
    [SerializeField] private Quiz _quiz;
    [SerializeField] private QuizType _quizType;

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
        PlayerPrefs.SetString(_quizKey, data);

        RestClient.Get<UserData>(Database._database + Auth.localID + ".json", GetUserDataCallback); 

        OnQuizStatisticDisplay?.Invoke(statisticInfo, _quizQuestions);
    }

    private void GetUserDataCallback(RequestException arg1, ResponseHelper arg2, UserData userData)
    {
        try
        {
            if (_quizType == QuizType.EXAM_QUIZ)
            {
                userData.examQuiz.statistics.Add(statisticInfo);
            }

            if (_quizType == QuizType.START_QUIZ)
            {
                userData.startQuiz.statistics.Add(statisticInfo);
            }

            Database.SendToDatabase(userData, Auth.localID);
        }
        catch (Exception)
        {
            Debug.Log("!");
        }
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
