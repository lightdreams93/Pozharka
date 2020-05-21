using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CprGame : MonoBehaviour
{
    [SerializeField] private UIAnimation _uiAnimation;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _maxCountHeart;
    [SerializeField] private float _maxCountMouth;
    [SerializeField] private float _maxCountCycles;
    [SerializeField] private float _minTempo;

    [SerializeField] private GameObject _game;

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _winPanel;

    [SerializeField] private GameObject _heartPanel;
    [SerializeField] private GameObject _mouthPanel;

    [SerializeField] private Text _countHearts;
    [SerializeField] private Text _countMouth;
    [SerializeField] private Text _countCyclesText;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _tempoText;

    [SerializeField] private float _delayHideBtn;
    [SerializeField] private float _delayShowBtn;

    [SerializeField] private Healthbar healthbar;

    private int _countHeartTouches;
    private int _countMouthTouches;

    private int _countCycles;

    private bool _isGameStart;

    private float _timer;
    private int _tempo;

    private void StartTimer()
    {
        if (!_isGameStart)
        { 
            return;
        }

        _timer += Time.deltaTime;
        DisplayTimer();
    }

    private void ResetTimer()
    {
        _timer = 0;
    }

    private void DisplayTimer()
    {
        int minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = _timer % 60;
        _timerText.text = $"{DisplayNum(minutes)}:{DisplayNum((int)seconds)}";
    }

    private string DisplayNum(int value)
    {
        if (value >= 10)
            return $"{value}";
        else
           return $"0{value}";
    }

    private void Start()
    {
        ResetTimer();
        //ResetPoints(_heartPanel);
        //ResetPoints(_mouthPanel);
        DisplayMouth();
        DisplayHearts();
        DisplayCycles();
        //StartCoroutine(ActivateRandomTask());
        DisplayTempo(_tempo);
        StartCoroutine(CheckTempo());


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

    

    private void DisplayHearts()
    {
        _countHearts.text = _countHeartTouches.ToString();
    }

    private void DisplayMouth()
    {
        _countMouth.text = _countMouthTouches.ToString();
    }

    private void DisplayCycles()
    {
        _countCyclesText.text = $"{_countCycles}/{_maxCountCycles}";
    }

    public void AddHeart()
    {
        if (_countHeartTouches < _maxCountHeart)
        {
            _countHeartTouches++;
            AddCycle();
            DisplayHearts();
            DisplayMouth();
            CheckWin();
            return;
        }
        ResetStats();
        healthbar.DecreaseHealth(10);
    }

    public void AddMouth()
    {
        if (_countHeartTouches < _maxCountHeart)
        {
            healthbar.DecreaseHealth(10);
            ResetStats();
            return;
        }
        _countMouthTouches++;
        DisplayHearts();
        DisplayMouth();
        AddCycle();
        CheckWin();
    }

    private void CheckWin()
    {
        if (_countCycles == _maxCountCycles) 
            StopGame(); 
    }

    private void ResetStats()
    {
        _countHeartTouches = 0;
        _countMouthTouches = 0;
        _countCycles = 0;
        _prevCount = 0;
        DisplayHearts();
        DisplayMouth();
        DisplayCycles();
    }

    private void AddCycle()
    {
        if (_countHeartTouches == _maxCountHeart && _countMouthTouches == _maxCountMouth)
        {
            _countCycles++;
            _countHeartTouches = 0;
            _countMouthTouches = 0;
            _prevCount = 0;
            DisplayMouth();
            DisplayHearts();
            DisplayCycles();
        }
    }
     
    private void ResetPoints(GameObject panel)
    {
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ActivateRandomPanel(GameObject panel)
    {
        int randomIndex = UnityEngine.Random.Range(0, panel.transform.childCount);
        ResetPoints(panel);
        panel.transform.GetChild(randomIndex).gameObject.SetActive(true);
    }

    public void StartGame()
    {
        _isGameStart = true;
         
        _uiAnimation.ScaleIn(_game.GetComponent<RectTransform>(), OnCompleteCallback);
        
        healthbar.PauseSpeed();
    }

    public void StopGame()
    {
        _isGameStart = false;
        _uiAnimation.ScaleIn(_winPanel.GetComponent<RectTransform>());
        _audioSource.Stop();
        healthbar.ResumeSpeed();
    }

    private void OnCompleteCallback()
    {
        _audioSource.Play();
    }

    private IEnumerator ActivateRandomTask()
    {
        while (true)
        {
            while (_isGameStart)
            {
                int randomIndex = UnityEngine.Random.Range(0, 2);
                GameObject randomPanel = (randomIndex > 0) ? _mouthPanel : _heartPanel;
                StartCoroutine(EnableRandomPanel(randomPanel));
                yield return new WaitForSeconds(_delayShowBtn);
            }

            yield return null;
        }
    }

    private IEnumerator EnableRandomPanel(GameObject panel)
    {
        ActivateRandomPanel(panel);
        yield return new WaitForSeconds(_delayHideBtn);
        ResetPoints(panel);
    }


    private void Update()
    {
        StartTimer();
    }

    private int _prevCount;
    private float _prevTimer;

    private IEnumerator CheckTempo()
    {
        while(true)
        {
            if (_countHeartTouches > 0)
            {
                if (_prevCount < _countHeartTouches)
                {
                    if (_prevCount == 0)
                    {
                        _prevTimer = _timer;
                        _prevCount = _countHeartTouches;
                    }
                    else
                    {
                        float lessTime = _timer - _prevTimer;
                        int count = _countHeartTouches - _prevCount;

                        float tempo = (float)count * 60 / lessTime;
                        _tempo = Mathf.RoundToInt(tempo);

                        if (_tempo < _minTempo || _tempo > _minTempo * 2)
                            healthbar.DecreaseHealth(5);

                        DisplayTempo(_tempo);

                        _prevCount = 0;
                    }
                }
                yield return new WaitForSeconds(0.02f);
            }

            yield return null;
        }
    }

    private void DisplayTempo(int temp)
    {
        _tempoText.text = $"{temp} <size=12>уд/мин</size>";
    }
}
