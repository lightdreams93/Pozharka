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

    [SerializeField] private TodoList _todoList;

    public static event Action OnVictumDie; 

    private float _currentHealth;
    private float _currentSpeed;
    private float _pauseSpeed;

    private void Start()
    {
        _currentHealth = _health;
        _currentSpeed = _speed;

        DisplayHealth();

        TodoList.OnTaskDone += TodoList_OnTaskDone;
        TodoList.OnTaskFailed += TodoList_OnTaskFailed;

        TodoList.OnAllTaskDone += TodoList_OnAllTaskDone;
    }

    private void TodoList_OnAllTaskDone()
    {
        _currentSpeed = 0;
    }

    private void TodoList_OnTaskFailed(ItemConfig obj)
    {
        MultiplySpeed();
    }

    private void TodoList_OnTaskDone(ItemConfig obj)
    {
        UndoSpeed();
    }
      
    private void OnDestroy()
    {
        TodoList.OnTaskDone -= TodoList_OnTaskDone;
        TodoList.OnTaskFailed -= TodoList_OnTaskFailed;

        TodoList.OnAllTaskDone -= TodoList_OnAllTaskDone;
    }

    private void Update()
    {
        if (!GameManager.isLevelStarted) return;
        DecreaseHealth();
    }

    public void PauseSpeed()
    {
        _pauseSpeed = _currentSpeed;
        _currentSpeed = _speed/2;
    }

    public void ResumeSpeed()
    {
        _currentSpeed = _pauseSpeed;
    }

    public void MultiplyPauseSpeed()
    {
        _pauseSpeed *= (1  + 1 / _todoList.TodoListArr.Length);
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
            OnVictumDie?.Invoke();
        }

        DisplayHealth();
    }

    public void MultiplySpeed()
    {
        _currentSpeed *= (1 + 1 / (float)_todoList.TodoListArr.Length);
    }

    public void UndoSpeed()
    {
        float speed = _currentSpeed / (1  + 1 / (float)_todoList.TodoListArr.Length);

        if (speed < _speed)
            _currentSpeed = _speed;
        else
            _currentSpeed = speed;
    }

    private void DisplayHealth()
    {
        _healthBar.text = Mathf.Round(_currentHealth).ToString() + "<size=12>%</size>";
    }
}
