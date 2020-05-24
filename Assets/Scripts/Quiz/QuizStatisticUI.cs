using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuizStatisticUI : MonoBehaviour
{
    [SerializeField] private GameObject _statPanel;
    [SerializeField] private Color _rightColor;
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Text _count;

    private void Start()
    {
        QuizStatistic.OnQuizStatisticDisplay += QuizStatistic_OnQuizStatisticDisplay;
    }

    private void QuizStatistic_OnQuizStatisticDisplay(StatisticInfo data, List<QuizQuestion> quizQuestions)
    {
        //for (int i = 0; i < _statPanel.transform.childCount; i++)
        //{
        //    QuizStatisticPanel panel = _statPanel.transform.GetChild(i).GetComponent<QuizStatisticPanel>();

        //    if (data.statistics[i].answer == data.statistics[i].rightAnswer)
        //    {
        //        _statPanel.transform.GetChild(i).GetComponent<Image>().color = _rightColor;
        //    }
        //    else
        //    {
        //        _statPanel.transform.GetChild(i).GetComponent<Image>().color = _wrongColor;
        //    }


        //    panel.Question.text = quizQuestions[data.statistics[i].currentQuestion].Question;
        //    panel.Answer.text = quizQuestions[data.statistics[i].currentQuestion].Answers[data.statistics[i].answer];
        //    panel.RightAnswer.text = quizQuestions[data.statistics[i].currentQuestion].Answers[data.statistics[i].rightAnswer];
        //}
        List<Statistic> stat = data.statistics.Where(x => x.answer == x.rightAnswer).ToList();
        _count.text = $"{stat.Count}/{data.statistics.Count}";
    }

    private void OnDestroy()
    {
        QuizStatistic.OnQuizStatisticDisplay -= QuizStatistic_OnQuizStatisticDisplay;
    }
}
