using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictumDieSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _victumDieClip;

    private void Start()
    {
        Healthbar.OnVictumDie += Healthbar_OnVictumDie;
    }

    private void OnDestroy()
    {
        Healthbar.OnVictumDie -= Healthbar_OnVictumDie;
    }

    private void Healthbar_OnVictumDie()
    {
        _audioSource.PlayOneShot(_victumDieClip);   
    }
}
