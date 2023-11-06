using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip firstStageSong;
    [SerializeField] private AudioClip secondStageSong;
    [SerializeField] private AudioClip thirdStageSong;


    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameHandler.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsSecondStageActive())
        {
            audioSource.Pause();
            audioSource.clip = secondStageSong;
            audioSource.Play();
        } else if (GameHandler.Instance.IsThirdStateActive())
        {
            audioSource.Pause();
            audioSource.clip = thirdStageSong;
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
            audioSource.clip = firstStageSong;
            audioSource.Play();
        }
    }
    
}
