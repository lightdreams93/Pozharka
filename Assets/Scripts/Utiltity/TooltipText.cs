using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Text", fileName ="NewSimpleText")]
public class TooltipText : ScriptableObject
{
    [TextArea(3, 5)]
    [SerializeField] private string _text;

    public string Text => _text;
}
