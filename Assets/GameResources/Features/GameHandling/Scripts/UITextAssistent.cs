using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextAssistent : MonoBehaviour
{
    [Serializable]
    public struct MyStruct
    {
        public TextWriter textWriter;
        public TextMeshProUGUI text;
        public TextMeshProUGUI message;
        public float timePerCharacter;
    }

    [SerializeField] private List<MyStruct> list = new List<MyStruct>();
    
    // [SerializeField] private TextWriter textWriter1;
    // [SerializeField] private TextWriter textWriter2;
    // [SerializeField] private TextMeshProUGUI messageText1;
    // [SerializeField] private TextMeshProUGUI messageToWrite1;
    //
    // [SerializeField] private TextMeshProUGUI messageText2;
    // [SerializeField] private TextMeshProUGUI messageToWrite2;
    //
    // [SerializeField] private float timePerCharacter1;
    // [SerializeField] private float timePerCharacter2;

    [SerializeField] private GameObject skipButton;

    private int writerIndex = 0;
    
    
    private void Start()
    {
        MyStruct myStruct = list[writerIndex];

        myStruct.textWriter.AddWriter(myStruct.text,myStruct.message.text,myStruct.timePerCharacter,this);
    }

    public void Continue()
    {
        writerIndex++;
        if (writerIndex >= list.Count)
        {
            skipButton.SetActive(true);
            return;
        }
        
        MyStruct myStruct = list[writerIndex];
        myStruct.textWriter.AddWriter(myStruct.text,myStruct.message.text,myStruct.timePerCharacter,this);
    }
}
