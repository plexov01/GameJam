using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 16;
    public int gridHeight = 8;
    [SerializeField] private int minPathLength = 35;
    [SerializeField] private int maxPathLength = 50;
    [SerializeField] private int minLoops = 1;
    [SerializeField] private int maxLoops = 4;

    public Transform grid;

    public GridCellObject[] pathCellObjects;
    public GridCellObject[] sceneryCellObjects;

    private PathGenerator pathGenerator;
    public bool addLoops;

    private EnemyManager enemyManager;

    void Start()
    {
        pathGenerator = new PathGenerator(gridWidth, gridHeight);
        enemyManager = EnemyManager.instance;

        int iteration = 0;
        print("Iteration " + iteration);
        List<Vector2Int> pathCells = pathGenerator.GeneratePath(addLoops);
        int pathSize = pathCells.Count;

        while (pathSize < minPathLength || pathSize > maxPathLength || pathGenerator.loopCount < minLoops || pathGenerator.loopCount > maxLoops)
        {
            iteration++;
            print("Iteration " + iteration);
            pathCells = pathGenerator.GeneratePath(addLoops);
            pathSize = pathCells.Count;

            if (iteration >= 50)
            {
                print("Could not generate path with this parameters");
                break;
            }
        }

        print("Path generated at iteration " + iteration);

        StartCoroutine(CreateGrid(pathCells));
    }

    private IEnumerator CreateGrid(List<Vector2Int> pathCells)
    {
        yield return StartCoroutine(LayPathCells(pathCells));
        yield return StartCoroutine(LaySceneryCells(pathCells[pathCells.Count - 1]));
        enemyManager.SetPathCells(pathGenerator.GenerateRoute());
        print("Grid is complete");
        Instantiate(enemyManager.enemyPrefabs[0], new Vector3(enemyManager.pathRoute[0].x, 0.2f, enemyManager.pathRoute[0].y), Quaternion.identity);
    }

    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        foreach (Vector2Int pathCell in pathCells)
        {
            int neighbourValue = pathGenerator.getCellNeighbourValue(pathCell.x, pathCell.y);
            //Debug.Log("Tile " + pathCell.x + ", " + pathCell.y + " neighbour value = " + neighbourValue);

            /*if (neighbourValue == 3 || neighbourValue == 5 || neighbourValue == 10 || neighbourValue == 12)
            {
                print("This is corner. Add waypoint");
            }
            else if (neighbourValue == 2)
            {
                print("This is the end. Add waypoint");
            }*/

            GameObject pathTile = pathCellObjects[neighbourValue].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(pathCell.x, 0f, pathCell.y), Quaternion.identity, grid);
            pathTileCell.transform.Rotate(0f, pathCellObjects[neighbourValue].yRotation, 0f, Space.Self);

            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }

    private IEnumerator LaySceneryCells(Vector2Int endCell)
    {
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (pathGenerator.CellIsEmpty(x, y))
                {
                    int randomSceneryCellIndex;

                    if (Vector2Int.Distance(new Vector2Int(x, y), endCell) < 1.5f)
                    {
                        randomSceneryCellIndex = 0;
                    }
                    else if (Random.Range(0f, 1f) < 0.8f)
                    {
                        randomSceneryCellIndex = 0;
                    }
                    else
                    {
                        randomSceneryCellIndex = Random.Range(1, sceneryCellObjects.Length);
                    }
                    
                    Instantiate(sceneryCellObjects[randomSceneryCellIndex].cellPrefab, new Vector3(x, 0f, y), Quaternion.identity, grid);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        yield return null;
    }
}
