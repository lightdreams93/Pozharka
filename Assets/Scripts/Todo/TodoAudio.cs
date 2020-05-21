using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodoAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _taskDone;
    [SerializeField] private AudioClip _taskFailed;
    [SerializeField] private AudioClip _allTaskDone;
   
    void Start()
    {
        TodoList.OnTaskDone += TodoList_OnTaskDone;
        TodoList.OnTaskFailed += TodoList_OnTaskFailed;

        TodoList.OnAllTaskDone += TodoList_OnAllTaskDone;
    }

    private void OnDestroy()
    {
        TodoList.OnTaskDone -= TodoList_OnTaskDone;
        TodoList.OnTaskFailed -= TodoList_OnTaskFailed;

        TodoList.OnAllTaskDone -= TodoList_OnAllTaskDone;
    }

    private void TodoList_OnAllTaskDone()
    {
        _audioSource.PlayOneShot(_allTaskDone);
    }

    private void TodoList_OnTaskFailed(ItemConfig obj)
    {
        _audioSource.PlayOneShot(_taskFailed);
    }

    private void TodoList_OnTaskDone(ItemConfig obj)
    {
        _audioSource.PlayOneShot(_taskDone);
    }
}
