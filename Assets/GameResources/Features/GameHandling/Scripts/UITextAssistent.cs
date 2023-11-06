using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextAssistent : MonoBehaviour
{
    [SerializeField] private TextWriter textWriter;
    [SerializeField] private TextMeshProUGUI messageText1;
    [SerializeField] private TextMeshProUGUI messageToWrite1;
    
    [SerializeField] private TextMeshProUGUI messageText2;
    [SerializeField] private TextMeshProUGUI messageToWrite2;


    private void Start()
    {
        textWriter.AddWriter(messageText1, messageToWrite1.text,1f);
        textWriter.AddWriter(messageText2, messageToWrite2.text,1f);
    }
}
