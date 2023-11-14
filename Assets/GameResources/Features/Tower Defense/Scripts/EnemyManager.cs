using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    private GameObject enemyInstance;

    //public List<int> enemyOrder;

    public List<Vector2Int> pathRoute;
    //private int nextPathCellIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //enemyInstance = Instantiate(enemyPrefabs[0], new Vector3(0, 0.2f, 0), Quaternion.identity);
        //nextPathCellIndex = 1;
    }

    private void Update()
    {
        /*if (pathCells != null && pathCells.Count > 1)
        {
            Vector3 currentPos = enemyInstance.transform.position;
            Vector3 nextPos = new Vector3(pathCells[nextPathCellIndex].x, 0.2f, pathCells[nextPathCellIndex].y);
            enemyInstance.transform.position = Vector3.MoveTowards(currentPos, nextPos, 2 * Time.deltaTime);

            if (Vector3.Distance(currentPos, nextPos) < 0.05f)
            {
                nextPathCellIndex++;
            }
        }*/
    }

    public void SetPathCells(List<Vector2Int> pathCells)
    {
        this.pathRoute = pathCells;
    }

}
