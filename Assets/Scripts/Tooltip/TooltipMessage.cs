using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipMessage : MonoBehaviour
{
    [SerializeField] private Text _tooltipText;
    [SerializeField] private GameObject _tooltipPanel;
 
    private void Start()
    {
        SetTextUI(null);
        Instruments.OnItemClicked += Instruments_OnDisplayDescription;
        TodoList.OnTaskFailed += TodoList_OnTaskFailed;
        TodoList.OnTaskDone += TodoList_OnTaskDone;
    }

    private void TodoList_OnTaskDone(ItemConfig obj)
    {
        SetText("Надеюсь после этого его состояние улучшится!");
    }

    private void TodoList_OnTaskFailed(ItemConfig obj)
    {
        SetText("Ой! Попробуйте еще раз! Это не то что нужно!");
    }
     
    private void OnDestroy()
    {
        Instruments.OnItemClicked -= Instruments_OnDisplayDescription;

        TodoList.OnTaskFailed -= TodoList_OnTaskFailed;
        TodoList.OnTaskDone -= TodoList_OnTaskDone;
    }

    private void Instruments_OnDisplayDescription(string text)
    {
        SetText(text);
    }

    public void SetText(TooltipText text)
    {
        SetTextUI(text.Text);
        StartAnimation();
    }

    public void SetText(string text)
    {
        SetTextUI(text);
        StartAnimation();
    }

    private void StartAnimation()
    {
        _tooltipPanel.SetActive(false);
        _tooltipPanel.SetActive(true);

        _tooltipText.gameObject.SetActive(false);
        _tooltipText.gameObject.SetActive(true);
    }

    private void SetTextUI(string text)
    {
        _tooltipText.text = text;
    }
}
