using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizStatisticPanel : MonoBehaviour
{
    [SerializeField] private Text _question;
    [SerializeField] private Text _answer;
    [SerializeField] private Text _rightAnswer;

    public Text Question => _question;
    public Text Answer => _answer;
    public Text RightAnswer => _rightAnswer;
}