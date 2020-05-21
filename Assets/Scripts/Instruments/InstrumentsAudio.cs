using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentsAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioClip _pageChangeClip;

    private void Start()
    {
        InstrumentsCell.OnCellClick += InstrumentsCell_OnCellClick;
        Instruments.OnPageChanged += Instruments_OnPageChanged;
    }

    private void Instruments_OnPageChanged()
    {
        _audioSource.PlayOneShot(_pageChangeClip);
    }

    private void InstrumentsCell_OnCellClick(int obj)
    {
        _audioSource.PlayOneShot(_clickClip);
    }

    private void OnDestroy()
    {
        InstrumentsCell.OnCellClick -= InstrumentsCell_OnCellClick;
        Instruments.OnPageChanged -= Instruments_OnPageChanged;
    }
}
