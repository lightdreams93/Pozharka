using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Instruments : MonoBehaviour
{
    [SerializeField] private List<Item> _instruments;
    [SerializeField] private GameObject _cellContainer;

    [SerializeField] private GameObject _activeItemObj;
    [SerializeField] private Sprite _defaultActiveCell;

    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;

    [SerializeField] private TodoList _todoList;

    public static event Action<string>OnItemClicked;
    public static event Action<ItemConfig, Item>OnItemUse;

    public static event Action OnPageChanged;

    private int _numPage;

    private ItemConfig _activeItem;

    private void Start()
    {
        EnableArrows();

        DisplayInstruments(_numPage);
        DisplayActiveItem();

        InstrumentsCell.OnCellClick += InstrumentsCell_OnCellClick;

        TodoList.OnTaskDone += TodoList_OnTaskDone; 
    }
 

    private void TodoList_OnTaskDone(ItemConfig item)
    {
        int indexItem = GetIndexActiveItem(_activeItem.guid);
        //_instruments[indexItem].OnItemUse?.Invoke();
        RemoveItem();
    }

    private void OnDestroy()
    {
        InstrumentsCell.OnCellClick -= InstrumentsCell_OnCellClick;

        TodoList.OnTaskDone -= TodoList_OnTaskDone; 
    }

    private void InstrumentsCell_OnCellClick(int numCell)
    { 
        DisplayDescription(numCell);
        SetActiveItem(numCell, _numPage);
        DisplayActiveItem();
    }

    public void NextArrow()
    {
        float pages = (float)_instruments.Count / _cellContainer.transform.childCount;

        int countPages = Mathf.CeilToInt(pages);
        int nextPage = _numPage + 1;

        if (nextPage == countPages - 1)
        {
            _rightArrow.interactable = false;
            _leftArrow.interactable = true; 
        }
        else
        {
            _rightArrow.interactable = true;
            _leftArrow.interactable = true;
        }

        _numPage++;
        DisplayInstruments(_numPage);
        OnPageChanged?.Invoke();
    }

    public void PrevArrow()
    {
        float pages = (float)_instruments.Count / _cellContainer.transform.childCount;

        int countPages = Mathf.CeilToInt(pages);
        int prevPage = _numPage - 1;

        if (prevPage == 0)
        {
            _leftArrow.interactable = false;
            _rightArrow.interactable = true;
        }
        _numPage--;
        DisplayInstruments(_numPage);
        OnPageChanged?.Invoke();
    }

    private void EnableArrows()
    {
        float pages = (float)_instruments.Count / _cellContainer.transform.childCount; 
        if (pages <= 1)
        {
            _leftArrow.interactable = false;
            _rightArrow.interactable = false;
            return;
        }
           
        _leftArrow.interactable = false;
        _rightArrow.interactable = true;
    }
     
    private void SetActiveItem(int numCell, int numPage)
    {
        ResetActiveItem();

        int offset = numPage * _cellContainer.transform.childCount;

        if (numCell + offset > _instruments.Count - 1)
            return;

        _activeItem = _instruments[numCell + offset].Config;

        _instruments[numCell + offset].OnItemActive?.Invoke();
    }

    private void DisplayActiveItem()
    {
        Image icon = _activeItemObj.transform.GetChild(0).GetComponent<Image>();

        if (_activeItem)
            icon.sprite = _activeItem.Icon;
        else
            icon.sprite = _defaultActiveCell;
    }

    public void ResetActiveItem()
    {
        if (!_activeItem) return;

        int indexActive = GetIndexActiveItem(_activeItem.guid);
        _instruments[indexActive].OnItemDisabled?.Invoke();

        _activeItem = null;
        DisplayActiveItem();
    }

    private int GetIndexActiveItem(string guid)
    {
        for (int i = 0; i < _instruments.Count; i++)
        {
            if (guid == _instruments[i].Config.guid)
            {
                return i;
            }
        }
        return -1;
    }

    
    private void DisplayDescription(int itemIndex)
    {
        string text = GetItemDesc(_numPage, itemIndex);
        if (!string.IsNullOrEmpty(text))
            OnItemClicked?.Invoke(text);
    }

    private string GetItemDesc(int numPage, int indexItem)
    {
        int offset = numPage * _cellContainer.transform.childCount;

        if (indexItem + offset > _instruments.Count - 1)
            return null;

        return _instruments[indexItem + offset].Config.Description;
    }

   
    private void DisplayInstruments(int numPage)
    {
        int offset = numPage * _cellContainer.transform.childCount;

        for (int i = 0; i < _cellContainer.transform.childCount; i++)
        {
            Image icon = _cellContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();

            if ((i + offset) < _instruments.Count)
            {
                icon.enabled = true;
                icon.sprite = _instruments[i + offset].Config.Icon;
            }
            else
                icon.enabled = false;
        }
    }

    private Item GetItemByID(string guid)
    {
        for (int i = 0; i < _instruments.Count; i++)
        {
            if (_instruments[i].Config.guid == guid)
            {
                return _instruments[i];
            }
        }
        return null;
    }

    public void UseItem(ItemConfig requireItem)
    {
        Item item = GetItemByID(requireItem.guid);
        OnItemUse?.Invoke(requireItem, item);
    }

    private void RemoveItem()
    {
        int indexItem = GetIndexActiveItem(_activeItem.guid);

        ResetActiveItem();
        _instruments.RemoveAt(indexItem);

        DisplayInstruments(_numPage);
    }
}

[System.Serializable]
public class Item
{
    [SerializeField] private ItemConfig _config;

    public UnityEvent OnItemActive;
    public UnityEvent OnItemUse;
    public UnityEvent OnItemDisabled;

    public ItemConfig Config => _config;
}
