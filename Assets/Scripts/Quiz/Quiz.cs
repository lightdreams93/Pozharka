using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    [SerializeField] private QuizQuestion[] _quizQuestions;
    [SerializeField] private int _countQuestions;
    [SerializeField] private float _delay;

    private int _currentQuestion;
    private int _answer;

    private List<QuizQuestion> _quizQuestionsSelected;

    public List<QuizQuestion> QuizQuestionsSelected => _quizQuestionsSelected;

    public int CurrentQuestion => _currentQuestion;

    public static event Action<List<QuizQuestion>> OnQuizInit;
    public static event Action<int, int, bool> OnAnswerSelected;
    public static event Action<int, List<QuizQuestion>> OnCurrentQuestionChanged;
    public static event Action OnQuizCompleted;

    private void Start()
    {
        SelectRandomQustions(out _quizQuestionsSelected);
    } 

    private void SelectRandomQustions(out List<QuizQuestion> quizQuestions)
    {
        quizQuestions = new List<QuizQuestion>();
        List<int> nums = new List<int>();

        if (_countQuestions > _quizQuestions.Length) return;

        int randomIndex = UnityEngine.Random.Range(0, _quizQuestions.Length);

        while (quizQuestions.Count < _countQuestions)
        {
            while (ExistNum(nums, randomIndex))
            {
                randomIndex = UnityEngine.Random.Range(0, _quizQuestions.Length);
            }
            quizQuestions.Add(_quizQuestions[randomIndex]);
            nums.Add(randomIndex);
        }
         
        OnQuizInit?.Invoke(_quizQuestionsSelected);
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

    public void SetAnswer(int answer)
    {
        _answer = answer;

        int rightAnswer = _quizQuestionsSelected[_currentQuestion].RightAnswer;
        bool isRightAnswer = CheckRightAnswer(answer, rightAnswer);

        OnAnswerSelected?.Invoke(_answer, rightAnswer, isRightAnswer);

        StartCoroutine(NextQuestion());
    }

    private bool CheckRightAnswer(int answer, int rightAnswer)
    {
        if (answer == rightAnswer)
            return true;

        return false;
    }

    private IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(_delay);
        if ((_currentQuestion + 1) == _countQuestions)
        {
            OnQuizCompleted?.Invoke();
            yield break;
        }
        _currentQuestion++;
        OnCurrentQuestionChanged?.Invoke(_currentQuestion, _quizQuestionsSelected);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
