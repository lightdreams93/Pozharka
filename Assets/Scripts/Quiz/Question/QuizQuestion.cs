using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Quiz/QuizQuestion", fileName ="NewQuizQuestion")]
public class QuizQuestion : ScriptableObject
{
    [TextArea(3, 5)]
    [SerializeField] private string _question;
    [TextArea(3,5)]
    [SerializeField] private string[] _answers = new string[4];

    [SerializeField] private int _rightAnswer;
    public int RightAnswer => _rightAnswer;

    public string Question => _question;

    public string[] Answers => _answers;
}
