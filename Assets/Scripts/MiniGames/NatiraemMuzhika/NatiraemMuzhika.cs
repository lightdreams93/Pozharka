using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NatiraemMuzhika : MonoBehaviour
{
    [SerializeField] private Slider _sliderBurns;
    [SerializeField] private Text _percentBurnsText;

    [SerializeField] private GameObject _game;

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _winPanel;

    [SerializeField] private GameObject _body;

    [Range(0, 100)]
    [SerializeField] private float _percentBurns;


    private void Start()
    {
        DisableBurns();

        List<int>indexes;
        SetBurns(out indexes);
        EnableBurns(indexes);

        int percent;
        GetPercentBurns(out percent);
        SetSliderValue(percent);
        SetText(percent.ToString());

        BurnPart.OnDragBurn += BurnPart_OnDragBurn;
    }

    private void OnDestroy()
    {
        BurnPart.OnDragBurn -= BurnPart_OnDragBurn;
    }

    private void BurnPart_OnDragBurn()
    {
        int percent;
        GetPercentBurns(out percent);
        SetSliderValue(percent);
        SetText(percent.ToString());
    }

    private void SetText(string value)
    {
        _percentBurnsText.text = $"{value}%";
    }

    private void SetSliderValue(int value)
    {
        _sliderBurns.value = value;
    }

    private void GetPercentBurns(out int percent)
    {
        int totalOpacity = Mathf.RoundToInt(_percentBurns / 100 * _body.transform.childCount);
        float currentOpacity = 0;

        for (int i = 0; i < _body.transform.childCount; i++)
        {
            if (!_body.transform.GetChild(i).gameObject.activeSelf) continue;
            currentOpacity += _body.transform.GetChild(i).GetComponent<Image>().color.a;
        }

        percent = Mathf.RoundToInt((currentOpacity / totalOpacity) * 100);
    }

    private void Update()
    {
        if (CheckWin())
            _winPanel.SetActive(true);
    }

    private bool CheckWin()
    {
        for (int i = 0; i < _body.transform.childCount; i++)
        {
            GameObject partBurn = _body.transform.GetChild(i).gameObject;

            if (!partBurn.activeSelf) continue;
            if (partBurn.GetComponent<Image>().color.a > 0) return false;
        }

        return true;
    }

    public void StartGame()
    {
        _winPanel.SetActive(false);
        _game.SetActive(true);
        _gamePanel.SetActive(true);
    }

    private void DisableBurns()
    {
        for (int i = 0; i < _body.transform.childCount; i++)
        {
            _body.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void EnableBurns(List<int> indexes)
    {
        for (int i = 0; i < indexes.Count; i++)
        {
            _body.transform.GetChild(indexes[i]).gameObject.SetActive(true);
        }
    }

    private void SetBurns(out List<int> indexesBurns)
    {
        int totalBurns = _body.transform.childCount;
        float currentCountBurns = _percentBurns / 100 * totalBurns;

        indexesBurns = new List<int>();

        while (indexesBurns.Count < Mathf.RoundToInt(currentCountBurns))
        {
            int index = Random.Range(0, totalBurns);

            while(CheckList(indexesBurns, index))
                index = Random.Range(0, totalBurns);

            indexesBurns.Add(index);
        }
    }

    private bool CheckList(List<int> nums, int num)
    {
        for (int i = 0; i < nums.Count; i++)
        {
            if (nums[i] == num)
                return true;
        }
        return false;
    }
}
