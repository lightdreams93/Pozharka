using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartedScreen : MonoBehaviour
{
    [SerializeField] private string _quizKey;
    [SerializeField] private GameObject _startedScreen;

    public static bool _showedStartedScreen;

    private void Start()
    {
        if (_showedStartedScreen)
            _startedScreen.SetActive(false);
    }

    public void ShowedStartedScreen()
    {
        _showedStartedScreen = true;
    }

   
}
