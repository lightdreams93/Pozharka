using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDoingDelay : MonoBehaviour
{
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _delay;
    [SerializeField] private TodoList _todoList;

    private bool _isGameStarted;
    private float _currentTime;

    public void StartGame()
    {
        _currentTime = _delay;

        _slider.maxValue = _currentTime;
        _slider.value = _currentTime;

        _isGameStarted = true;
        _gamePanel.SetActive(true);
    }

    private void Update()
    {
        if (!_isGameStarted) return;

        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _slider.value = _currentTime;
        }
        else
        {
            _todoList.DoneTask();
            _isGameStarted = false;
            _gamePanel.SetActive(false);
        }
    }
}
