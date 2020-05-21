using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhgutTestAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _rightAnswer;
    [SerializeField] private AudioClip _wrongAnswer;

    private void Start()
    {
        ZhgutTest.OnRightAnswer += ZhgutTest_OnRightAnswer;
        ZhgutTest.OnWrongAnswer += ZhgutTest_OnWrongAnswer;
    }

    private void OnDestroy()
    {
        ZhgutTest.OnRightAnswer -= ZhgutTest_OnRightAnswer;
        ZhgutTest.OnWrongAnswer -= ZhgutTest_OnWrongAnswer;
    }

    private void ZhgutTest_OnWrongAnswer()
    {
        _audioSource.PlayOneShot(_wrongAnswer);    
    }

    private void ZhgutTest_OnRightAnswer()
    {
        _audioSource.PlayOneShot(_rightAnswer);
    }
}
