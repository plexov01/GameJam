using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private bool enteredTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!enteredTrigger && other.CompareTag("EnemyTrigger"))
        {
            enteredTrigger = true;
            print("end game");
        }
    }
}
