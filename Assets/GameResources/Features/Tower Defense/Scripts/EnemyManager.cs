using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<int> enemyOrder;

    private void Awake()
    {
        instance = this;
    }
}
