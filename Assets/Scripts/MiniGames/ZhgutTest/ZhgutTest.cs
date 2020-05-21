using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhgutTest : MonoBehaviour
{
    [SerializeField] private UIAnimation _uiAnimation;

    [SerializeField] private TestQuestion[] _testQuestions;

    [SerializeField] private Text _questionText;
    [SerializeField] private Text[] _answersText;

    [SerializeField] private GameObject _game;

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _winPanel;

    [SerializeField] private Healthbar _healthbar;

    public static event Action OnRightAnswer;
    public static event Action OnWrongAnswer;

    private int _currentQuestion;
    private int _yourAnswer;

    private void Start()
    {
        DisplayQuestion();

        Healthbar.OnVictumDie += Healthbar_OnVictumDie;
    }

    private void OnDestroy()
    {
        Healthbar.OnVictumDie -= Healthbar_OnVictumDie;
    }

    private void Healthbar_OnVictumDie()
    {
        //_game.SetActive(false);
        _uiAnimation.ScaleOut(_game.GetComponent<RectTransform>());
    }

    public void StartGame()
    {
        //_game.SetActive(true);
        _uiAnimation.ScaleIn(_game.GetComponent<RectTransform>());
    }

    public void Answering(int answer)
    {
        _yourAnswer = answer;
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        int right = _testQuestions[_currentQuestion].RightIndex;
        if (_yourAnswer == right)
        {
            NextQuestion();
            OnRightAnswer?.Invoke();
        }
        else
        {
            _healthbar.DecreaseHealth(_healthbar.CurrentHealth / _testQuestions.Length * (_currentQuestion + 1));
            NextQuestion();
            OnWrongAnswer?.Invoke();
        }
            
    }

    private void NextQuestion()
    {
        int next = _currentQuestion + 1;
        if (next > _testQuestions.Length - 1)
        {
            //_winPanel.SetActive(true);
            _uiAnimation.ScaleIn(_winPanel.GetComponent<RectTransform>());
            _gamePanel.SetActive(false);
            return;
        }

        _currentQuestion++;
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        _questionText.text = _testQuestions[_currentQuestion].Question;
        for (int i = 0; i < _answersText.Length; i++)
        {
            _answersText[i].text = _testQuestions[_currentQuestion].Answers[i];
        }
    }
}

[System.Serializable]
public class TestQuestion
{
    [TextArea(3,5)]
    [SerializeField] private string _question;
    [SerializeField] private string[] _answers;
    [SerializeField] private int _rightIndex;

    public string Question => _question;
    public string[] Answers => _answers;
    public int RightIndex => _rightIndex;
}
