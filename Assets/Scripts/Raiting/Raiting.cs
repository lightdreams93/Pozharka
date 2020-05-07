using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raiting : MonoBehaviour
{
    [SerializeField] TodoList _todoList;

    [SerializeField] private GameObject _stars;

    [SerializeField] private Sprite _activeStar;
    [SerializeField] private Sprite _disabledStar;

    private float _countStars;

    private void Start()
    {
        _countStars = _stars.transform.childCount;

        TodoList.OnTaskFailed += TodoList_OnTaskFailed;
        TodoList.OnAllTaskDone += TodoList_OnAllTaskDone;
    }

    private void TodoList_OnAllTaskDone()
    {
        DisplayStars();
    }

    private void OnDestroy()
    {
        TodoList.OnTaskFailed -= TodoList_OnTaskFailed;
        TodoList.OnAllTaskDone -= TodoList_OnAllTaskDone;
    }

    private void TodoList_OnTaskFailed(ItemConfig obj)
    {
        DecreaseStar();
    }

    private void DecreaseStar()
    {
        float temp = _countStars - 1 / (float)_todoList.TodoListArr.Length;
        if (temp > 0)
            _countStars = _countStars - 1 / (float)_todoList.TodoListArr.Length;
        else
            _countStars = 0;
    }

    private void DisplayStars()
    {
        for (int i = 0; i < _stars.transform.childCount; i++)
        {
            GameObject starObj = _stars.transform.GetChild(i).gameObject;
            Image star = starObj.transform.GetChild(1).GetComponent<Image>();
            
            if (i < Mathf.FloorToInt(_countStars))
            {
                starObj.transform.GetChild(0).gameObject.SetActive(true);
                star.sprite = _activeStar;
            }
            else
            {
                starObj.transform.GetChild(0).gameObject.SetActive(false);
                star.sprite = _disabledStar;
            }
        }
    }
}
