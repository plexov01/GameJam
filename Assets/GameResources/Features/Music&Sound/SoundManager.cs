using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }    
    
    [SerializeField] public AudioClipRefsSO audioClipRefsSo;


    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = .15f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    
}
