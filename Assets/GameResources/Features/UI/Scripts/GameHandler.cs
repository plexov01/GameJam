using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    
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

    private float secondStageTimer;
    private float secondStageTimerMax = 180f;


    private void Awake()
    {
        Instance = this;

        currentState = State.WaitingForStart;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.WaitingForStart:
                break;
            case State.FirstStage:
                break;
            case State.SecondStage:
                if (secondStageTimer >= secondStageTimerMax)
                {
                    secondStageTimer = 0f;

                    currentState = State.ThirdStage;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.ThirdStage:
                break;
            case State.GameOver:
                break;
        }
    }

    public void GameOver()
    {
        currentState = State.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartSecondStage()
    {
        currentState = State.SecondStage;
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
