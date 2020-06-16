using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Text _logText;

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
        Debug.Log(data);

        if (GameModeSettings.onlineMode)
        {
            _logText.text = "Подождите идет сохранение данных...";
            RestClient.Get<UserData>(Database.databaseUrl + Auth.localID + ".json", GetUserDataCallback);
        }

        OnQuizStatisticDisplay?.Invoke(statisticInfo, _quizQuestions);
    }

    private void GetUserDataCallback(RequestException exception, ResponseHelper response, UserData userData)
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
            _logText.text = "Данные успешно сохранены!";
        }
        catch (Exception)
        {
            _logText.text = exception.Message;
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
        string question = _quiz.QuizQuestionsSelected[_quiz.CurrentQuestion].Question;
        string answerText = _quiz.QuizQuestionsSelected[_quiz.CurrentQuestion].Answers[answer];
        string rightAnswerText = _quiz.QuizQuestionsSelected[_quiz.CurrentQuestion].Answers[rightAnswer];

        statisticInfo.statistics.Add(new Statistic(question, answerText, rightAnswerText, isRightAnswer));
    }

    private string GetDate()
    {
        DateTime dt = DateTime.Now;
        string day = dt.Day.ToString();
        string month = dt.Month.ToString();
        string year = dt.Year.ToString();
        string hours = (dt.Hour < 10) ? $"0{dt.Hour}" : dt.Hour.ToString();
        string minutes = (dt.Minute < 10) ? $"0{dt.Minute}" : dt.Minute.ToString(); ;

        return $"{day}/{month}/{year}/{hours}:{minutes}";
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
    public string currentQuestion;
    public string answer;
    public string rightAnswer;
    public bool right;

    public Statistic(string currentQuestion, string answer, string rightAnswer, bool right)
    {
        this.currentQuestion = currentQuestion;
        this.answer = answer;
        this.rightAnswer = rightAnswer;
        this.right = right;
    }
}
