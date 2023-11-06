using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
    }
}
