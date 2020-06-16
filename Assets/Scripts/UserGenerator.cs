using System.Collections; 
using UnityEngine;
using Proyecto26;
using System.Net.Security;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class UserGenerator : MonoBehaviour
{
    public QuizType quizType;
    public int countQuestions;

    public int countResults;

    public Auth auth;
    public QuizQuestion[] quizQuestions;

    public List<QuizQuestion> _generatedQuestions;

    private int _answer;
    private int _currentQuestion;

    private StatisticInfo statisticInfo;
    private QuizData data;

    private int _currentDay = 7;

    private void Start()
    { 
       StartCoroutine(Game());
    }
     
    private IEnumerator Game()
    { 
        for (int i = 0; i < auth.Users.Length; i++)
        {
            data = new QuizData();
            for (int j = 0; j < countResults; j++)
            {
                statisticInfo = new StatisticInfo();

                SelectRandomQuestions();

                int hours = UnityEngine.Random.Range(8, 22);
                int minutes = UnityEngine.Random.Range(0, 59);

                string hoursText = (hours < 10) ? $"0{hours}" : $"{hours}";
                string minutesText = (minutes < 10) ? $"0{minutes}" : $"{minutes}";

                statisticInfo.currentDate = $"{_currentDay + j}/6/2020/{hoursText}:{minutesText}";

                for (int k = 0; k < _generatedQuestions.Count; k++)
                {
                    SetAnswer();
                }

                data.statistics.Add(statisticInfo);
                _currentQuestion = 0;
            }
            auth.SignIn(auth.Users[i].email, "123456");
            yield return new WaitForSeconds(10);

            if (Auth.localID != null)
                RestClient.Get<UserData>(Database.databaseUrl + Auth.localID + ".json", GetUserDataCallback);

            yield return new WaitForSeconds(10);
        }
        Debug.Log("Успех!!");
    }

    private void GetUserDataCallback(RequestException arg1, ResponseHelper arg2, UserData userData)
    {
        if(quizType == QuizType.START_QUIZ)
            userData.startQuiz = data;
        
        if (quizType == QuizType.EXAM_QUIZ)
            userData.examQuiz = data;

        Database.SendToDatabase(userData, Auth.localID);
    }

    private void SelectRandomQuestions()
    {
        _generatedQuestions = new List<QuizQuestion>(); 

        List<int> nums = new List<int>();

        if (countQuestions > quizQuestions.Length) return;

        int randomIndex = UnityEngine.Random.Range(0, quizQuestions.Length);

        while (_generatedQuestions.Count < countQuestions)
        {
            while (ExistNum(nums, randomIndex))
            {
                randomIndex = UnityEngine.Random.Range(0, quizQuestions.Length);
            }
            _generatedQuestions.Add(quizQuestions[randomIndex]);
            nums.Add(randomIndex);
        }
    }

    private bool ExistNum(List<int> nums, int num)
    {
        for (int i = 0; i < nums.Count; i++)
        {
            if (nums[i] == num)
                return true;
        }
        return false;
    }

    public void SetAnswer()
    {
        int answer = UnityEngine.Random.Range(0, 4);
        _answer = answer;

        int rightAnswer = _generatedQuestions[_currentQuestion].RightAnswer;
        bool isRightAnswer = CheckRightAnswer(answer, rightAnswer);

        string question = _generatedQuestions[_currentQuestion].Question;
        string answerText = _generatedQuestions[_currentQuestion].Answers[answer];
        string rightAnswerText = _generatedQuestions[_currentQuestion].Answers[rightAnswer];
        statisticInfo.statistics.Add(new Statistic(question, answerText, rightAnswerText, isRightAnswer));

        NextQuestion();
    }

    private bool CheckRightAnswer(int answer, int rightAnswer)
    {
        if (answer == rightAnswer)
            return true;

        return false;
    }

    private void NextQuestion()
    {
        //if ((_currentQuestion + 1) == countQuestions)
        //{
        //    data.statistics.Add(statisticInfo);
        //    _currentQuestion = 0;
        //}
        _currentQuestion++;
    }

}


[System.Serializable]
public class User
{
    public string userName;
    public string userSurname;
    public string email;
    public string keyWord;
}
