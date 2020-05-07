using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public static event Action<int> OnCardStartDrag;
    public static event Action<int> OnCardDrag;
    public static event Action<int> OnCardEndDrag;

    public static event Action<int, int> OnCardDrop;

    private static int dragCardIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragCardIndex = transform.GetSiblingIndex();
        OnCardStartDrag?.Invoke(dragCardIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnCardDrag?.Invoke(transform.GetSiblingIndex());
    }

    public void OnDrop(PointerEventData eventData)
    { 
        OnCardDrop?.Invoke(dragCardIndex, transform.GetSiblingIndex());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnCardEndDrag?.Invoke(transform.GetSiblingIndex());
    }
}
