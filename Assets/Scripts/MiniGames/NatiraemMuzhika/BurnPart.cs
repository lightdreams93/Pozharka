using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BurnPart : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image _image;

    public static event Action OnDragBurn;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
            
    }

    public void OnDrag(PointerEventData eventData)
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a - eventData.delta.magnitude * Time.deltaTime * 0.8f);
        
        if (_image.color.a <= 0)
           _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);

        OnDragBurn?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
