using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNode : MonoBehaviour
{
    [HideInInspector] public Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
}
