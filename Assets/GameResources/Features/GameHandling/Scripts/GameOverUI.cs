using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject endingPanel_S;
    [SerializeField] private GameObject endingPanel_A;
    [SerializeField] private GameObject endingPanel_B;
    [SerializeField] private GameObject endingPanel_C;
    [SerializeField] private GameObject endingPanel_D;
    
    private void Start()
    {
        GameHandler.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void OnDestroy()
    {
        GameHandler.OnStateChanged -= GameHandler_OnStateChanged;
    }


    private void Show()
    {
        gameOverPanel.SetActive(true);
        GameHandler.Ending ending = GameHandler.Instance.GetEnding();
        
        Debug.Log("Showing the ending!");

        switch (ending)
        {
            case GameHandler.Ending.Ending_S:
                endingPanel_S.SetActive(true);
                break;
            case GameHandler.Ending.Ending_A:
                endingPanel_A.SetActive(true);
                break;
            case GameHandler.Ending.Ending_B:
                endingPanel_B.SetActive(true);
                break;
            case GameHandler.Ending.Ending_C:
                endingPanel_C.SetActive(true);
                break;
            case GameHandler.Ending.Ending_D:
                endingPanel_D.SetActive(true);
                break;
        }
    }
    
    private void Hide()
    {
        gameOverPanel.SetActive(false);
        endingPanel_S.SetActive(false);
        endingPanel_A.SetActive(false);
        endingPanel_B.SetActive(false);
        endingPanel_C.SetActive(false);
        endingPanel_D.SetActive(false);
        
    }
}
