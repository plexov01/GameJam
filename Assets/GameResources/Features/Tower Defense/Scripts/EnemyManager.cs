using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<int> enemyOrder;

    public Transform end;
    //public NavMeshSurface surface;

    private void Awake()
    {
        instance = this;
    }
}
