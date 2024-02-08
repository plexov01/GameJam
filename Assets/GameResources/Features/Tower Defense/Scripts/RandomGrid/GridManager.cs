using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager instance = null;

    [Header("Grid parameters")]
    public int gridWidth = 16;
    public int gridHeight = 8;
    [SerializeField] private int rightExtend = 0;

    [Header("Path parameters")]
    [SerializeField] private int minPathLength = 35;
    [SerializeField] private int maxPathLength = 50;
    [SerializeField] private int minLoops = 1;
    [SerializeField] private int maxLoops = 4;
    public bool addLoops;
    [SerializeField] private int maxAttempts = 1000000;

    [Header("References")]
    public Transform grid;

    public GridCellObject[] pathCellObjects;
    public GridCellObject[] sceneryCellObjects;

    public PathGenerator pathGenerator;
    private List<Transform> pathNodes = new List<Transform>();

    private EnemyManager enemyManager;
    private BuildManager buildManager;

    public bool mainScene;
    public GameObject spawnStructure;
    public GameObject mainBase;
    public GameObject endStructure;

    private void Awake()
    {
        instance = this;

        if (mainScene)
        {
            spawnStructure.SetActive(false);
            endStructure.SetActive(false);
            mainBase.SetActive(false);
        }
    }

    void Start()
    {
        pathGenerator = new PathGenerator(gridWidth, gridHeight);
        enemyManager = EnemyManager.instance;
        buildManager = BuildManager.instance;

        int iteration = 0;
        List<Vector2Int> pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
        int pathSize = pathCells.Count;

        if (addLoops)
        {
            while (pathSize < minPathLength || pathSize > maxPathLength || pathGenerator.loopCount < minLoops || pathGenerator.loopCount > maxLoops)
            {
                iteration++;
                pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                pathSize = pathCells.Count;

                if (iteration >= maxAttempts)
                {
                    //print("Could not generate path with given parameters");
                    print("ÓÂÛ Jokerge");

                    while (pathSize < minPathLength)
                    {
                        iteration++;
                        pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                        pathSize = pathCells.Count;

                        if (iteration >= 2 * maxAttempts)
                        {
                            print("ÓÂÛ Jokerge");
                            iteration++;
                            pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                            break;
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            while (pathSize < minPathLength || pathSize > maxPathLength)
            {
                iteration++;
                pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                pathSize = pathCells.Count;

                if (iteration >= maxAttempts)
                {
                    //print("Could not generate path with given parameters");
                    print("ÓÂÛ Jokerge");

                    while (pathSize < minPathLength)
                    {
                        iteration++;
                        pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                        pathSize = pathCells.Count;

                        if (iteration >= 2 * maxAttempts)
                        {
                            print("ÓÂÛ Jokerge");
                            iteration++;
                            pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                            break;
                        }
                    }

                    break;
                }
            }
        }

        print("Path of length " + pathCells.Count + " generated at iteration " + iteration);

        StartCoroutine(CreateGrid(pathCells));
    }

    private IEnumerator CreateGrid(List<Vector2Int> pathCells)
    {
        Tuple<List<Vector2Int>, List<Vector2Int>> route = pathGenerator.GenerateRoute();
        List<Vector2Int> routeCells = route.Item1;
        buildManager.SetRoute(route);
        enemyManager.SetPathCells(routeCells);

        if (mainScene)
        {
            spawnStructure.transform.position = new Vector3(pathCells[0].x, 0.5f, pathCells[0].y);
            TDManager tDManager = TDManager.instance;
            tDManager.spawnPoint = new Vector3(pathCells[0].x, 0.5f, pathCells[0].y);
            endStructure.transform.position = new Vector3(pathCells[pathCells.Count - 1].x + 1.25f, 0.5f, pathCells[pathCells.Count - 1].y);
            mainBase.transform.position = new Vector3(pathCells[pathCells.Count - 1].x, 1f, pathCells[pathCells.Count - 1].y);
        }

        yield return StartCoroutine(LayPathCells(routeCells));
        yield return StartCoroutine(LaySceneryCells(pathCells[pathCells.Count - 1]));

        if (mainScene)
        {
            spawnStructure.SetActive(true);
            endStructure.SetActive(true);
            mainBase.transform.parent = pathNodes[pathNodes.Count - 1];
            mainBase.SetActive(true);
        }

        print("Grid is complete");
    }

    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        for (int i = 0; i < pathCells.Count - 2; i++)
        {
            int neighbourValue = pathGenerator.getCellNeighbourValue(pathCells[i].x, pathCells[i].y);
            //Debug.Log("Tile " + pathCells[i].x + ", " + pathCells[i].y + " neighbour value = " + neighbourValue);

            GameObject pathTile = pathCellObjects[neighbourValue].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(pathCells[i].x, 0f, pathCells[i].y), Quaternion.identity, grid.GetChild(0));
            pathNodes.Add(pathTileCell.transform);
            buildManager.pathNodes.Add(pathTileCell.GetComponent<Renderer>());

            pathTileCell.transform.Rotate(0f, pathCellObjects[neighbourValue].yRotation, 0f, Space.Self);

            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }

    private IEnumerator LaySceneryCells(Vector2Int endCell)
    {
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridWidth + rightExtend; x++)
            {
                if (pathGenerator.CellIsEmpty(x, y))
                {
                    int randomSceneryCellIndex;

                    if (Vector2Int.Distance(new Vector2Int(x, y), endCell) < 1.5f)
                    {
                        randomSceneryCellIndex = 0;
                    }
                    else if (sceneryCellObjects.Length == 1 || Random.Range(0f, 1f) < 0.8f)
                    {
                        randomSceneryCellIndex = 0;
                    }
                    else
                    {
                        randomSceneryCellIndex = Random.Range(1, sceneryCellObjects.Length);
                    }
                    
                    Instantiate(sceneryCellObjects[randomSceneryCellIndex].cellPrefab, new Vector3(x, 0f, y), Quaternion.identity, grid.GetChild(1));
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }

        yield return null;
    }
}
