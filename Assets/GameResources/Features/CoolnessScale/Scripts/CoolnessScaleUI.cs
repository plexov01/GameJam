using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolnessScaleUI : MonoBehaviour
{
    
    [SerializeField] private List<CoolnessIconValuePair> coolnessIconValuePairList = new List<CoolnessIconValuePair>();
    
    [Serializable] private struct CoolnessIconValuePair
    {
        public Sprite icon;
        public float sliderValueMin;
    }
    
    private Slider scaleSlider;
    private Image handleImage;
    
    
    private void Awake()
    {
        scaleSlider = GetComponent<Slider>();
        handleImage = scaleSlider.handleRect.GetComponent<Image>();
    }

    private void Start()
    {
        CoolnessScaleController.OnCoolnessChanged += UpdateUI;
    }

    private void UpdateUI(object sender, CoolnessScaleController.OnCoolnessChangedEventArgs e)
    {
        float coolness = (float)e.coolness / e.coolnessMax;
        scaleSlider.value = coolness;

        foreach (var pair in coolnessIconValuePairList)
        {
            if (coolness <= pair.sliderValueMin)
            {
                handleImage.sprite = pair.icon;
                break;
            }
            handleImage.sprite = pair.icon;
        }
        
    }
    
}
