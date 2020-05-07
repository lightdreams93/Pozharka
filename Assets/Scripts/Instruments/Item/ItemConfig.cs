using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item", fileName ="NewItem")]
public class ItemConfig : ScriptableObject
{
    public string guid;
    [SerializeField] private bool _isMiniGame;
    [SerializeField] private Sprite _icon;
    [TextArea(3,5)]
    [SerializeField] private string _description;
    public Sprite Icon => _icon;
    public string Description => _description;
    public bool IsMiniGame => _isMiniGame;
}
