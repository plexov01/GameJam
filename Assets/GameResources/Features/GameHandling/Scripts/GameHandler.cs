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

    private bool hasImmortality = true;
    private bool canLose = true;


    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(this);

        currentState = State.WaitingForStart;

        
    }

    private void Start()
    {
        OnStateChanged += GameHandler_OnStateChanged;
        CoolnessScaleController.OnCoolnessChanged += CoolnessScaleController_OnCoolnessChanged;
    }

    private void CoolnessScaleController_OnCoolnessChanged(object sender, CoolnessScaleController.OnCoolnessChangedEventArgs e)
    {
        if (e.coolness > 0.05f && e.coolness < 0.95)
        {
            hasImmortality = true;
        }
        else if (hasImmortality)
        {
            StartCoroutine(GetImmortality());
        }
    }

    private IEnumerator GetImmortality()
    {
        hasImmortality = false;
        
        canLose = false;

        yield return new WaitForSeconds(3f);

        canLose = true;
    }

    private void OnDestroy()
    {
        OnStateChanged -= GameHandler_OnStateChanged;
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
    
    public bool IsSecondOrThirdStageActive()
    {
        return currentState is State.SecondStage or State.ThirdStage;
    }
}
