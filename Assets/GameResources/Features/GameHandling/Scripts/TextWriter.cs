using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private UITextAssistent assistent;

    private bool isComplete = false;
    

    public void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, UITextAssistent assistent)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
        this.assistent = assistent;
    }

    private void Update()
    {
        if (isComplete) return;
        
        if (uiText != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = timePerCharacter;
                characterIndex++;
                
                if (characterIndex > textToWrite.Length)
                {
                    isComplete = true;
                    assistent.Continue();
                    
                    return;
                }
                
                uiText.text = textToWrite.Substring(0, characterIndex);
            }
        }
    }
}
