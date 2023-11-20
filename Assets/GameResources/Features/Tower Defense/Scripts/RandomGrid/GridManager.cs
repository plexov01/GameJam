using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
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

    [Header("References")]
    public Transform grid;

    public GridCellObject[] pathCellObjects;
    public GridCellObject[] sceneryCellObjects;

    private PathGenerator pathGenerator;
    private List<Transform> pathNodes = new List<Transform>();

    private EnemyManager enemyManager;

    public bool mainScene;
    public GameObject spawnStructure;
    public GameObject mainBase;
    public GameObject endStructure;

    private void Awake()
    {
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

        int iteration = 0;
        //print("Iteration " + iteration);
        List<Vector2Int> pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
        int pathSize = pathCells.Count;

        while (pathSize < minPathLength || pathSize > maxPathLength || pathGenerator.loopCount < minLoops || pathGenerator.loopCount > maxLoops)
        {
            iteration++;
            //print("Iteration " + iteration);
            pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
            pathSize = pathCells.Count;

            if (iteration >= 50)
            {
                //print("Could not generate path with given parameters");

                while (pathSize < minPathLength)
                {
                    iteration++;
                    //print("Iteration " + iteration);
                    pathCells = pathGenerator.GeneratePath(addLoops, minLoops, maxLoops);
                    pathSize = pathCells.Count;
                }

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
        
        BuildManager buildManager = BuildManager.instance;

        foreach (Transform pathNode in grid.GetChild(0))
        {
            buildManager.pathNodes.Add(pathNode.GetComponent<Renderer>());
        }

        TDManager tDManager = TDManager.instance;
        tDManager.spawnPoint = new Vector3(pathCells[0].x, 0.5f, pathCells[0].y);

        if (mainScene)
        {
            spawnStructure.transform.position = new Vector3(pathCells[0].x, 0.5f, pathCells[0].y);
            spawnStructure.SetActive(true);
            endStructure.transform.position = new Vector3(pathCells[pathCells.Count - 1].x + 1.25f, 0.5f, pathCells[pathCells.Count - 1].y);
            endStructure.SetActive(true);
            mainBase.transform.position = new Vector3(pathCells[pathCells.Count - 1].x, 1f, pathCells[pathCells.Count - 1].y);
            mainBase.transform.parent = pathNodes[pathNodes.Count - 1];
            mainBase.SetActive(true);
        }

        print("Grid is complete");
    }

    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        foreach (Vector2Int pathCell in pathCells)
        {
            int neighbourValue = pathGenerator.getCellNeighbourValue(pathCell.x, pathCell.y);
            //Debug.Log("Tile " + pathCell.x + ", " + pathCell.y + " neighbour value = " + neighbourValue);

            GameObject pathTile = pathCellObjects[neighbourValue].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(pathCell.x, 0f, pathCell.y), Quaternion.identity, grid.GetChild(0));
            pathNodes.Add(pathTileCell.transform);
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
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        yield return null;
    }
}
