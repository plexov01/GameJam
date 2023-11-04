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
        
        DontDestroyOnLoad(this);

        currentState = State.WaitingForStart;

        OnStateChanged += GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if (currentState == State.GameOver)
        {
            Time.timeScale = 0;
            Debug.Log("Game Over!");
        }
    }

    private void Update()
    {
        if (currentState == State.SecondStage)
        {
            secondStageTimer += Time.deltaTime;
            if (secondStageTimer >= secondStageTimerMax)
            {
                secondStageTimer = 0f;

                currentState = State.ThirdStage;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public bool IsFirstStageActive()
    {
        return currentState == State.FirstStage;
    }
}
