using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    //public List<int> enemyOrder;

    public List<Vector2Int> pathRoute;

    private void Awake()
    {
        instance = this;
    }

    public void SetPathCells(List<Vector2Int> pathCells)
    {
        pathRoute = pathCells;
    }

}
