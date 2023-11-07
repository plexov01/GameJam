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
        GameHandler.OnStateChanged += GameHandlerOnOnStateChanged;
    }

    private void GameHandlerOnOnStateChanged(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsThirdStateActive())
        {
            Hide();
        }
    }

    private void Start()
    {
        CoolnessScaleController.OnCoolnessChanged += UpdateUI;
        UpdateUI(CoolnessScaleController.Instance.GetCoolness());
    }

    private void UpdateUI(object sender, CoolnessScaleController.OnCoolnessChangedEventArgs e)
    { 
        scaleSlider.value = e.coolness;

        foreach (var pair in coolnessIconValuePairList)
        {
            if (e.coolness <= pair.sliderValueMin)
            {
                handleImage.sprite = pair.icon;
                break;
            }
            handleImage.sprite = pair.icon;
        }
        
    }

    private void UpdateUI(float coolness)
    {
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
    
    private void OnDestroy()
    {
        CoolnessScaleController.OnCoolnessChanged -= UpdateUI;
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    
}
