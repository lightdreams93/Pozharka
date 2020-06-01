using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreDependenses : MonoBehaviour
{
    [SerializeField] private int _countDependenses;

    private int _currentCount;

    public void ActivateObject(GameObject obj)
    {
        if (_currentCount < _countDependenses) return;
        obj.SetActive(true);
    }

    public void DeactivateObject(GameObject obj)
    {
        if (_currentCount < _countDependenses) return;
        obj.SetActive(false);
    }

    public void Increase()
    {
        _currentCount++;
    }
}
