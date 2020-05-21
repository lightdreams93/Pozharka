using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Text _healthBar;
    [SerializeField] private float _health;

    [SerializeField] private float _speed = 1;
      
    public static event Action OnVictumDie; 

    private float _currentHealth;
    private float _currentSpeed;
    private float _pauseSpeed;

    public float CurrentHealth => _currentHealth;

    private void Start()
    {
        _currentHealth = _health;
        _currentSpeed = _speed;

        DisplayHealth();

        TodoList.OnTaskFailed += TodoList_OnTaskFailed;
        GameManager.OnGameWin += GameManager_OnGameWin;
    }

    private void GameManager_OnGameWin()
    {
        _currentSpeed = 0;
    }

    private void TodoList_OnTaskFailed(ItemConfig obj)
    {
        DecreaseHealth(10); 
    }
      
    private void OnDestroy()
    {
        TodoList.OnTaskFailed -= TodoList_OnTaskFailed;
        GameManager.OnGameWin -= GameManager_OnGameWin;
    }

    private void Update()
    {
        if (!GameManager.isLevelStarted) return;
        DecreaseHealth();
    }

    public void PauseSpeed()
    {
        _pauseSpeed = _currentSpeed;
        _currentSpeed = 0;
    }

    public void ResumeSpeed()
    {
        _currentSpeed = _pauseSpeed;
    }

    public void DecreaseHealth(float value)
    {  
        float temp = _currentHealth - value;

        if (temp >= 0)
        {
            _currentHealth -= value;
        }
        else
        {
            _currentHealth = 0;
            OnVictumDie?.Invoke();
        }

        DisplayHealth();
    }

    private void DecreaseHealth()
    {
        if (_currentHealth >= 0)
        {
            _currentHealth -= Time.deltaTime * _currentSpeed;
        }
        else
        {
            _currentHealth = 0;
            _currentSpeed = 0;
            OnVictumDie?.Invoke();
        }

        DisplayHealth();
    }

    private void DisplayHealth()
    {
        _healthBar.text = Mathf.Round(_currentHealth).ToString() + "<size=12>%</size>";
    }
}
