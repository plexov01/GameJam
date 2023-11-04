using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameHandler Instance { get; private set; }
    
    public enum State
    {
        WaitingForStart,
        FirstStage,
        SecondStage,
        ThirdStage,
        GameOver,
    }

    public static event EventHandler OnStateChanged;

    private State currentState;


    private void Awake()
    {
        Instance = this;

        currentState = State.WaitingForStart;
    }

    private void Update()
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }



    public bool IsGameOver()
    {
        return currentState == State.GameOver;
    }
    
    public bool IsFirstStageActive()
    {
        return currentState == State.FirstStage;
    }
}
