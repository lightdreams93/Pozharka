using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void FlyInX(RectTransform obj)
    {
        LeanTween.moveX(obj, 0, 0.2f);
    }

    public void FlyOutToLeft(RectTransform obj)
    {
        Vector2 pos = obj.anchoredPosition - Vector2.right * Screen.width;
        LeanTween.moveX(obj, pos.x, 0.2f);
    }

    public void FlyOutToRight(RectTransform obj)
    {
        Vector2 pos = obj.anchoredPosition + Vector2.right * Screen.width;
        LeanTween.moveX(obj, pos.x, 0.2f);
    }

    public void FlyInY(RectTransform obj)
    {
        LeanTween.moveY(obj, 0, 0.2f);
    }

    public void FlyOutToUp(RectTransform obj)
    {
        Vector2 pos = obj.anchoredPosition + Vector2.up * Screen.height;
        LeanTween.moveY(obj, pos.y, 0.2f);
    }

    public void FlyOutToBottom(RectTransform obj)
    {
        Vector2 pos = obj.anchoredPosition - Vector2.up * Screen.height;
        LeanTween.moveY(obj, pos.y, 0.2f);
    }

    public void ScaleIn(RectTransform obj)
    {
        LeanTween.scale(obj, new Vector3(1, 1, 1), 0.5f).setEaseOutBounce();
    }

    public void ScaleIn(RectTransform obj, Action OnCompleteCallback)
    {
        LeanTween.scale(obj, new Vector3(1, 1, 1), 0.5f).setEaseOutBounce().setOnComplete(OnCompleteCallback);
    }

    public void ScaleOut(RectTransform obj)
    {
        LeanTween.scale(obj, Vector3.zero, 0.5f);
    }

    public void ScaleOut(RectTransform obj, Action OnComplete)
    {
        LeanTween.scale(obj, Vector3.zero, 0.5f).setOnComplete(OnComplete);
    }
}
