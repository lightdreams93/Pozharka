using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sprite _disableIcon;
    [SerializeField] private Sprite _enableIcon;

    [SerializeField] private Image _soundImage;

    private void Start()
    {
        DisplayIcon();
    }

    public void ToggleSound()
    {
        AudioListener.pause = !AudioListener.pause;
        DisplayIcon();
    }

    private void DisplayIcon()
    {
        _soundImage.sprite = (AudioListener.pause) ? _disableIcon : _enableIcon;
    }
}
