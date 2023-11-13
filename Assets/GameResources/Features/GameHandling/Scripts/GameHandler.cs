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
    
    public enum Ending
    {
        Ending_S,
        Ending_A,
        Ending_B,
        Ending_C,
        Ending_D,
    }

    public static event EventHandler OnStateChanged;

    private State currentState;
    private Ending ending = Ending.Ending_B;

    private float secondStageTimer;
    [SerializeField] private float secondStageTimerMax = 180f;

    private bool wasAnyExtremePointReached = false;
    private bool hasImmortality = true;
    private bool canFinishGame = true;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
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
        if (IsThirdStateActive()) return;
        
        if (e.coolness > 0.05f && e.coolness < 0.95)
        {
            hasImmortality = true;
        }
        else if (hasImmortality)
        {
            StartCoroutine(GetImmortality());
            wasAnyExtremePointReached = true;
        }
        else if (e.coolness < 0.05f)
        {
            ending = Ending.Ending_C;
            FinishGame();
        }
        else if (e.coolness > 0.95f)
        {
            ending = Ending.Ending_D;
            FinishGame();
        }
    }

    private IEnumerator GetImmortality()
    {
        hasImmortality = false;
        
        canFinishGame = false;
        
        Debug.Log("Immortality!");

        yield return new WaitForSeconds(3f);

        canFinishGame = true;
    }

    private void OnDestroy()
    {
        OnStateChanged -= GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if (currentState == State.WaitingForStart)
        {
            Time.timeScale = 1;
            Reset();
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

                ending = wasAnyExtremePointReached ? Ending.Ending_A : Ending.Ending_S;
                
                currentState = State.ThirdStage;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    

    public void FinishGame()
    {
        if(!canFinishGame) return;
        
        Debug.Log("Game is over! Ending is " + ending);
        ChangeState(State.GameOver);
        Time.timeScale = 0f;
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ChangeState(State.FirstStage);
        Reset();
        SceneLoader.LoadScene(SceneLoader.Scene.TowerDefense);
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
    
    public bool IsSecondStageActive()
    {
        return currentState is State.SecondStage;
    }
    
    public bool IsThirdStateActive()
    {
        return currentState is State.ThirdStage;
    }
    
    public bool IsGameOver()
    {
        return currentState is State.GameOver;
    }

    public Ending GetEnding()
    {
        return ending;
    }

    private void Reset()
    {
        ending = Ending.Ending_B;
        wasAnyExtremePointReached = false;
        hasImmortality = true;
        canFinishGame = true;
        secondStageTimer = 0f;
    }
}
