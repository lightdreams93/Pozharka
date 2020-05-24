using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private GameObject _quizPanel;
    [SerializeField] private GameObject _statPanel;

    [SerializeField] private Text _questionText;
    [SerializeField] private Button[] _answersBtn;

    [SerializeField] private Text _currentQuestionsIndex;

    private void Start()
    {
        Quiz.OnQuizInit += Quiz_OnQuizInit;
        Quiz.OnAnswerSelected += Quiz_OnAnswerSelected;
        Quiz.OnCurrentQuestionChanged += Quiz_OnCurrentQuestionChanged;
        Quiz.OnQuizCompleted += Quiz_OnQuizCompleted;
    }

    private void Quiz_OnQuizCompleted()
    {
        _statPanel.SetActive(true);
        _quizPanel.SetActive(false);
    }

    private void Quiz_OnQuizInit(List<QuizQuestion> quizQuestions)
    {
        DisplayQuestion(0, quizQuestions);
        DisplayAnswers(0, quizQuestions);
        DisplayCount(0, quizQuestions.Count);
    }

    private void Quiz_OnCurrentQuestionChanged(int currentQuestion, List<QuizQuestion> quizQuestions)
    {
        for (int i = 0; i < _answersBtn.Length; i++)
        {
            _answersBtn[i].interactable = true;
        }
        ResetHightlight();
        DisplayQuestion(currentQuestion, quizQuestions);
        DisplayAnswers(currentQuestion, quizQuestions);
        DisplayCount(currentQuestion, quizQuestions.Count);
    }

    private void Quiz_OnAnswerSelected(int answer, int rightAnswer, bool isRightAnswer)
    {
        for (int i = 0; i < _answersBtn.Length; i++)
        {
            _answersBtn[i].interactable = false;
        }
        HighlightAnswers(answer, rightAnswer, isRightAnswer);
    }

    private void OnDestroy()
    {
        Quiz.OnQuizInit -= Quiz_OnQuizInit;
        Quiz.OnAnswerSelected -= Quiz_OnAnswerSelected;
        Quiz.OnCurrentQuestionChanged -= Quiz_OnCurrentQuestionChanged;
        Quiz.OnQuizCompleted -= Quiz_OnQuizCompleted;
    }

    private void DisplayQuestion(int numQuestion, List<QuizQuestion> quizQuestions)
    {
        _questionText.text = quizQuestions[numQuestion].Question.ToUpper();
    }

    private void DisplayAnswers(int numQuestion, List<QuizQuestion> quizQuestions)
    {
        for (int i = 0; i < quizQuestions[numQuestion].Answers.Length; i++)
        {
            _answersBtn[i].transform.GetChild(0).GetComponent<Text>().text = quizQuestions[numQuestion].Answers[i].ToUpper();
        }
    }

    private void HighlightAnswers(int answer, int rightAnswer, bool isRightAnswer)
    {
        if (isRightAnswer)
        {
            _answersBtn[answer].GetComponent<Image>().color = Color.green;
        }
        else
        {
            _answersBtn[answer].GetComponent<Image>().color = Color.red;
            _answersBtn[rightAnswer].GetComponent<Image>().color = Color.green;
        }
    }

    private void ResetHightlight()
    {
        for (int i = 0; i < _answersBtn.Length; i++)
        {
            _answersBtn[i].GetComponent<Image>().color = new Color32(36, 91, 102, 255);
        }
    }

    private void DisplayCount (int currentQuestion, int maxQuestions)
    {
        _currentQuestionsIndex.text = $"{currentQuestion + 1}/{maxQuestions}";
    }
}
