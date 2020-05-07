using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstrumentsCell : MonoBehaviour, IPointerClickHandler
{
    public static event Action<int>OnCellClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        int numCell = transform.GetSiblingIndex();
        OnCellClick?.Invoke(numCell);
    }
}
