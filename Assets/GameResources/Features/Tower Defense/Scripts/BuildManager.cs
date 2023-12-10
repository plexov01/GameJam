using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance = null;
    
    public static event EventHandler<OnConstructionPlacedEventArgs> OnConstructionPlaced;
    public class OnConstructionPlacedEventArgs : EventArgs {
        public BuildMode buildMode;
    }

    public GameObject turretPrefab_T2;
    public GameObject turretPrefab_T3;

    public List<Renderer> pathNodes;

    public enum BuildMode
    {
        None,
        Tower,
        Wall,
        Mine,
        Repair
    }

    public BuildMode buildMode;

    private Camera mainCamera;
    public GameObject objectToBuild = null;
    [SerializeField] private GameObject illegalLocationPlane;
    private GameObject illegalPlaneInstance;
    [SerializeField] private GameObject legalLocationPlane;
    private GameObject legalPlaneInstance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask planeMask;

    private RaycastHit hit;
    private RaycastHit planeHit;

    private string towerNodeTag = "TowerNode";
    private string pathNodeTag = "PathNode";
    private string wallTag = "Wall";
    private string mainBaseTag = "MainBase";

    public LayerMask enemyMask;

    [SerializeField] private float planeOffsetY = 0.25f;

    [System.Serializable]
    public struct Buildable
    {
        public GameObject buildPrefab;
        public float buildOffsetY;
        public GameObject dragPrefab;
        public float dragOffsetY;
        [HideInInspector] public GameObject dragInstance;
    }

    public Buildable turret;
    public Buildable wall;
    public Buildable miniWall;
    public Buildable mine;
    public Buildable repair;

    private List<Vector2Int> routeCells;
    private List<Vector2Int> routeDirections;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buildMode = BuildMode.None;

        mainCamera = Camera.main;

        illegalPlaneInstance = Instantiate(illegalLocationPlane, Vector3.zero, Quaternion.identity);
        illegalPlaneInstance.SetActive(false);
        legalPlaneInstance = Instantiate(legalLocationPlane, Vector3.zero, Quaternion.identity);
        legalPlaneInstance.SetActive(false);

        turret.dragInstance = Instantiate(turret.dragPrefab, Vector3.zero, Quaternion.identity);
        turret.dragInstance.SetActive(false);
        wall.dragInstance = Instantiate(wall.dragPrefab, Vector3.zero, Quaternion.identity);
        wall.dragInstance.SetActive(false);
        miniWall.dragInstance = Instantiate(miniWall.dragPrefab, Vector3.zero, Quaternion.identity);
        miniWall.dragInstance.SetActive(false);
        mine.dragInstance = Instantiate(mine.dragPrefab, Vector3.zero, Quaternion.identity);
        mine.dragInstance.SetActive(false);
        repair.dragInstance = Instantiate(repair.dragPrefab, Vector3.zero, Quaternion.identity);
        repair.dragInstance.SetActive(false);
    }

    public void SetRoute(Tuple<List<Vector2Int>, List<Vector2Int>> route)
    {
        routeCells = route.Item1;
        routeDirections = route.Item2;
    }

    public GameObject GetTowerToBuild(int tier)
    {
        switch (tier)
        {
            case 1:
                return turret.buildPrefab;
            case 2:
                return turretPrefab_T2;
            case 3:
                return turretPrefab_T3;
            default:
                return null;
        }
    }

    public GameObject GetWallToBuild(int type)
    {
        switch (type)
        {
            case 0:
                return wall.buildPrefab;
            case 1:
                return miniWall.buildPrefab;
            default:
                return null;
        }
    }

    public GameObject GetMineToBuild()
    {
        return mine.buildPrefab;
    }

    private void ResetDraggables()
    {
        turret.dragInstance.SetActive(false);
        wall.dragInstance.SetActive(false);
        miniWall.dragInstance.SetActive(false);
        mine.dragInstance.SetActive(false);
        repair.dragInstance.SetActive(false);
        legalPlaneInstance.SetActive(false);
        illegalPlaneInstance.SetActive(false);
    }

    public void BuildTower()
    {
        ResetDraggables();
        buildMode = BuildMode.Tower;
        objectToBuild = turret.dragPrefab;
        turret.dragInstance.SetActive(true);
    }

    public void BuildWall()
    {
        ResetDraggables();
        buildMode = BuildMode.Wall;
        objectToBuild = wall.dragPrefab;
        wall.dragInstance.SetActive(true);
    }

    public void BuildMine()
    {
        ResetDraggables();
        buildMode = BuildMode.Mine;
        objectToBuild = mine.dragPrefab;
        mine.dragInstance.SetActive(true);
    }

    public void Repair()
    {
        ResetDraggables();
        buildMode = BuildMode.Repair;
        objectToBuild = repair.dragPrefab;
        //repair.dragInstance.SetActive(true);
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {    
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit1;

            if (Physics.Raycast(ray, out hit1, 100, layerMask))
            {
                Debug.Log(hit1.transform.gameObject.name);
            }
        }*/

        if (objectToBuild != null)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, groundMask))
            {
                switch (buildMode)
                {
                    case BuildMode.None:

                        break;

                    case BuildMode.Tower:

                        turret.dragInstance.transform.position = new Vector3(hit.transform.position.x, turret.dragOffsetY, hit.transform.position.z);

                        if (hit.transform.CompareTag(towerNodeTag) && hit.transform.childCount == 0)
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    case BuildMode.Wall:

                        Vector3 rotation = Vector3.zero;

                        if (hit.transform.CompareTag(pathNodeTag))
                        {
                            for (int i = 0; i < routeCells.Count; i++)
                            {
                                if (routeCells[i] == new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z))
                                {
                                    if (routeDirections[i] == Vector2Int.right)
                                    {
                                        rotation.y = -90f;
                                    }
                                    else if (routeDirections[i] == Vector2Int.left)
                                    {
                                        rotation.y = 90f;
                                    }
                                    else if (routeDirections[i] == Vector2Int.up)
                                    {
                                        rotation.y = 180f;
                                    }
                                    else if (routeDirections[i] == Vector2Int.down)
                                    {
                                        rotation.y = 0f;
                                    }

                                    break;
                                }
                            }

                            if (hit.transform.rotation.y != 0)
                            {
                                wall.dragInstance.SetActive(true);
                                miniWall.dragInstance.SetActive(false);
                                wall.dragInstance.transform.position = new Vector3(hit.transform.position.x, wall.dragOffsetY, hit.transform.position.z);
                            }
                            else
                            {
                                wall.dragInstance.SetActive(false);
                                miniWall.dragInstance.SetActive(true);
                                miniWall.dragInstance.transform.position = new Vector3(hit.transform.position.x, miniWall.dragOffsetY, hit.transform.position.z);
                                miniWall.dragInstance.transform.eulerAngles = rotation;
                            }

                            if (hit.transform.childCount == 0 && Physics.OverlapBox(hit.transform.position, new Vector3(0.55f, 5f, 0.55f), Quaternion.identity, enemyMask).Length == 0
                                && hit.transform != pathNodes[0].transform && hit.transform != pathNodes[1].transform)
                            {
                                legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                                legalPlaneInstance.SetActive(true);
                                illegalPlaneInstance.SetActive(false);
                            }
                            else
                            {
                                illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                                illegalPlaneInstance.SetActive(true);
                                legalPlaneInstance.SetActive(false);
                            }
                        }
                        else if (hit.transform.CompareTag(wallTag) || hit.transform.CompareTag(mainBaseTag))
                        {
                            for (int i = 0; i < routeCells.Count; i++)
                            {
                                if (routeCells[i] == new Vector2Int((int)hit.transform.parent.parent.position.x, (int)hit.transform.parent.parent.position.z))
                                {
                                    if (routeDirections[i] == Vector2Int.right)
                                    {
                                        rotation.y = -90f;
                                    }
                                    else if (routeDirections[i] == Vector2Int.left)
                                    {
                                        rotation.y = 90f;
                                    }
                                    else if (routeDirections[i] == Vector2Int.up)
                                    {
                                        rotation.y = 180f;
                                    }
                                    else if (routeDirections[i] == Vector2Int.down)
                                    {
                                        rotation.y = 0f;
                                    }

                                    break;
                                }
                            }

                            if (hit.transform.parent.parent.rotation.y != 0)
                            {
                                wall.dragInstance.SetActive(true);
                                miniWall.dragInstance.SetActive(false);
                                wall.dragInstance.transform.position = new Vector3(hit.transform.parent.parent.position.x, wall.dragOffsetY, hit.transform.parent.parent.position.z);
                            }
                            else
                            {
                                wall.dragInstance.SetActive(false);
                                miniWall.dragInstance.SetActive(true);
                                miniWall.dragInstance.transform.position = new Vector3(hit.transform.parent.parent.position.x, miniWall.dragOffsetY, hit.transform.parent.parent.position.z);
                                miniWall.dragInstance.transform.eulerAngles = rotation;
                            }

                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.parent.parent.position.x, planeOffsetY, hit.transform.parent.parent.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            wall.dragInstance.transform.position = new Vector3(hit.transform.position.x, wall.dragOffsetY, hit.transform.position.z);
                            wall.dragInstance.SetActive(true);
                            miniWall.dragInstance.SetActive(false);
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    case BuildMode.Mine:

                        mine.dragInstance.transform.position = new Vector3(hit.transform.position.x, mine.dragOffsetY, hit.transform.position.z);

                        if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0)
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    case BuildMode.Repair:

                        //dragRepairInstance.SetActive(true);
                        //repair.dragInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                        //repair.dragInstance.transform.position = new Vector3(hit.transform.position.x, repair.dragOffsetY, hit.transform.position.z);

                        if (hit.transform.CompareTag(wallTag) || hit.transform.CompareTag(mainBaseTag))
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY + 1f, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, planeOffsetY, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    default:

                        break;
                }
            }
            else
            {
                illegalPlaneInstance.SetActive(false);
                legalPlaneInstance.SetActive(false);

                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out planeHit, 100, planeMask);

                switch (buildMode)
                {
                    case BuildMode.None:

                        break;

                    case BuildMode.Tower:

                        turret.dragInstance.transform.position = new Vector3(planeHit.point.x, 0f, planeHit.point.z);
                        break;

                    case BuildMode.Wall:

                        miniWall.dragInstance.SetActive(false);
                        wall.dragInstance.SetActive(true);
                        wall.dragInstance.transform.position = new Vector3(planeHit.point.x, 0f, planeHit.point.z);
                        break;

                    case BuildMode.Mine:

                        mine.dragInstance.transform.position = new Vector3(planeHit.point.x, 0.5f, planeHit.point.z);
                        break;

                    case BuildMode.Repair:

                        break;

                    default:

                        break;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                illegalPlaneInstance.SetActive(false);
                legalPlaneInstance.SetActive(false);

                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, groundMask))
                {
                    switch (buildMode)
                    {
                        case BuildMode.None:

                            break;

                        case BuildMode.Tower:

                            if (hit.transform.CompareTag(towerNodeTag) && hit.transform.childCount == 0)
                            {
                                turret.dragInstance.SetActive(false);
                                GameObject turretObject = Instantiate(GetTowerToBuild(1), new Vector3(hit.transform.position.x, turret.buildOffsetY, hit.transform.position.z), Quaternion.identity, hit.transform);
                                TDManager.instance.turrets.Add(turretObject.transform.GetChild(0));
                                objectToBuild = null;
                                
                                OnConstructionPlaced?.Invoke(this, new OnConstructionPlacedEventArgs() {
                                    buildMode = buildMode
                                });
                                
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't place turret there.");
                            }

                            break;

                        case BuildMode.Wall:

                            if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0 && Physics.OverlapBox(hit.transform.position, new Vector3(0.55f, 5f, 0.55f), Quaternion.identity, enemyMask).Length == 0
                                && hit.transform != pathNodes[0].transform && hit.transform != pathNodes[1].transform)
                            {
                                wall.dragInstance.SetActive(false);
                                miniWall.dragInstance.SetActive(false);

                                GameObject wallObject;

                                if (hit.transform.rotation.y != 0)
                                {
                                    wallObject = Instantiate(GetWallToBuild(0), new Vector3(hit.transform.position.x, wall.buildOffsetY, hit.transform.position.z), new Quaternion(0, 0, 0, 0), hit.transform);
                                }
                                else
                                {
                                    Vector3 rotation = Vector3.zero;

                                    for (int i = 0; i < routeCells.Count; i++)
                                    {
                                        if (routeCells[i] == new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z))
                                        {
                                            if (routeDirections[i] == Vector2Int.right)
                                            {
                                                rotation.y = -90f;
                                            }
                                            else if (routeDirections[i] == Vector2Int.left)
                                            {
                                                rotation.y = 90f;
                                            }
                                            else if (routeDirections[i] == Vector2Int.up)
                                            {
                                                rotation.y = 180f;
                                            }
                                            else if (routeDirections[i] == Vector2Int.down)
                                            {
                                                rotation.y = 0f;
                                            }

                                            break;
                                        }
                                    }

                                    wallObject = Instantiate(GetWallToBuild(1), new Vector3(hit.transform.position.x, miniWall.buildOffsetY, hit.transform.position.z), new Quaternion(0, 0, 0, 0), hit.transform);
                                    wallObject.transform.eulerAngles = rotation;
                                }

                                TDManager.instance.walls.Add(wallObject.transform.GetChild(0));
                                objectToBuild = null;
                                
                                OnConstructionPlaced?.Invoke(this, new OnConstructionPlacedEventArgs() {
                                    buildMode = buildMode
                                });
                                
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't place wall there.");
                            }

                            break;

                        case BuildMode.Mine:

                            if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0)
                            {
                                mine.dragInstance.SetActive(false);
                                GameObject mineObject = Instantiate(GetMineToBuild(), new Vector3(hit.transform.position.x, mine.buildOffsetY, hit.transform.position.z), Quaternion.identity, hit.transform);
                                TDManager.instance.mines.Add(mineObject.transform.GetChild(0));
                                objectToBuild = null;
                                
                                OnConstructionPlaced?.Invoke(this, new OnConstructionPlacedEventArgs() {
                                    buildMode = buildMode
                                });
                                
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't place mine there.");
                            }

                            break;

                        case BuildMode.Repair:

                            if (hit.transform.CompareTag(wallTag) || hit.transform.CompareTag(mainBaseTag))
                            {
                                Wall wall = hit.transform.GetComponent<Wall>();
                                wall.currentHealth = wall.baseHealth;
                                objectToBuild = null;
                                
                                OnConstructionPlaced?.Invoke(this, new OnConstructionPlacedEventArgs() {
                                    buildMode = buildMode
                                });
                                
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't repair there.");
                            }

                            break;

                        default:

                            break;
                    }
                }
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (hit.transform != null)
        {
            Gizmos.DrawWireCube(hit.transform.position, new Vector3(1f, 1f, 1f));
        }
    }*/
}
