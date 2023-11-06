using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoolnessScaleController : MonoBehaviour
{
    public static CoolnessScaleController Instance { get; private set; }
    
    public static event EventHandler<OnCoolnessChangedEventArgs> OnCoolnessChanged;
    public class OnCoolnessChangedEventArgs : EventArgs
    {
        public float coolness;
    }
    
    [SerializeField] private int coolnessMax = 1000;
    [SerializeField] private int currentCoolness = 500;

    public bool isDark;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddCoolness(int points)
    {
        currentCoolness += points;
        OnCoolnessChanged?.Invoke(this,new OnCoolnessChangedEventArgs{coolness = GetCoolness()});
    }

    public float GetCoolness()
    {
        return (float)currentCoolness / coolnessMax;
    }

    public bool IsDarkness()
    {
        return isDark;
    }
    


}
