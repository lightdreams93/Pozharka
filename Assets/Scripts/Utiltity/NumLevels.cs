using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumLevels : MonoBehaviour
{
    [SerializeField] private GameObject _levelsContainer;
    private void Start()
    {
        for (int i = 0; i < _levelsContainer.transform.childCount; i++)
        {
            _levelsContainer.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }   
    }
}
