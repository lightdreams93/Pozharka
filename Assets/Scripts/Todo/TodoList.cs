﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TodoList : MonoBehaviour
{
    [SerializeField] private Todo[] _todoList;

    public Todo[] TodoListArr => _todoList;

    public static event Action<ItemConfig> OnTaskDone;
    public static event Action<ItemConfig> OnTaskFailed;

    public static event Action OnAllTaskDone;
    public UnityEvent OnAllTasksDone;

    private bool _isAllTasksDone;
    public bool IsAllTasksDone => _isAllTasksDone;

    private void OnEnable()
    {
        if(Instruments.OnItemUse == null)
            Instruments.OnItemUse += Instruments_OnItemUse;
    }

    private void Start()
    {
        if(Instruments.OnItemUse == null)
            Instruments.OnItemUse += Instruments_OnItemUse;
    }

    private void OnDisable()
    {
        Instruments.OnItemUse = null;
    }

    private void OnDestroy()
    {
        Instruments.OnItemUse = null;
    }

    private void Instruments_OnItemUse(ItemConfig requireItem, Item item)
    {
        string taskID = GetCurrentTaskItemID();

        if (string.IsNullOrEmpty(taskID)) return;

        if (taskID.Equals(requireItem.guid))
        {
            if (!requireItem.IsMiniGame)
                DoneTask();
            else
                item.OnItemUse?.Invoke();
        }
        else
            OnTaskFailed?.Invoke(requireItem);
    }

    private bool CheckEndGame()
    {
        for (int i = 0; i < _todoList.Length; i++)
        {
            if (!_todoList[i].IsDone) return false;
        }

        return true;
    }

    private string GetCurrentTaskItemID()
    {
        for (int i = 0; i < _todoList.Length; i++)
        {
            if (!_todoList[i].IsDone)
            {
                return _todoList[i].Config.guid;
            }
        }

        return null;
    }

    public void DoneTask()
    {
        for (int i = 0; i < _todoList.Length; i++)
        {
            if (!_todoList[i].IsDone)
            {
                OnTaskDone?.Invoke(_todoList[i].Config);
                _todoList[i].DoneTask();
                if (CheckEndGame())
                {
                    GameManager.winList.Add(_isAllTasksDone);
                    OnAllTaskDone?.Invoke();
                    OnAllTasksDone?.Invoke();
                }
                    
                return;
            }
        }
    }
}

[System.Serializable]
public class Todo
{
    [SerializeField]
    private ItemConfig _config;

    private bool _isDone;
    public bool IsDone => _isDone;

    public ItemConfig Config => _config;

    public void DoneTask()
    {
        _isDone = true;
    }
}


