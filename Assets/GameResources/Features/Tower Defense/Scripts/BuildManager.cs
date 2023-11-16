using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance = null;

    [HideInInspector] public GameObject towerToBuild;
    [HideInInspector] public GameObject wallToBuild;
    [HideInInspector] public GameObject mineToBuild;

    public GameObject turretPrefab_T1;
    public GameObject turretPrefab_T2;
    public GameObject turretPrefab_T3;
    public GameObject wallPrefab;
    public GameObject minePrefab;

    public Node[] walkableNodes;
    public Color lavaColor;

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

    [SerializeField] private GameObject dragTurret;
    private GameObject dragTurretInstance;
    [SerializeField] private GameObject dragWall;
    private GameObject dragWallInstance;
    [SerializeField] private GameObject dragMine;
    private GameObject dragMineInstance;
    [SerializeField] private GameObject dragRepair;
    private GameObject dragRepairInstance;

    private RaycastHit hit;

    private string towerNodeTag = "TowerNode";
    private string pathNodeTag = "PathNode";
    private string wallTag = "Wall";
    private string mainBaseTag = "MainBase";

    public LayerMask enemyMask;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buildMode = BuildMode.None;

        wallToBuild = wallPrefab;
        mineToBuild = minePrefab;

        mainCamera = Camera.main;

        illegalPlaneInstance = Instantiate(illegalLocationPlane, Vector3.zero, Quaternion.identity);
        illegalPlaneInstance.SetActive(false);
        legalPlaneInstance = Instantiate(legalLocationPlane, Vector3.zero, Quaternion.identity);
        legalPlaneInstance.SetActive(false);

        dragTurretInstance = Instantiate(dragTurret, Vector3.zero, Quaternion.identity);
        dragTurretInstance.SetActive(false);
        dragWallInstance = Instantiate(dragWall, Vector3.zero, Quaternion.identity);
        dragWallInstance.SetActive(false);
        dragMineInstance = Instantiate(dragMine, Vector3.zero, Quaternion.identity);
        dragMineInstance.SetActive(false);
        dragRepairInstance = Instantiate(dragRepair, Vector3.zero, Quaternion.identity);
        dragRepairInstance.SetActive(false);
    }

    public GameObject GetTowerToBuild(int tier)
    {
        switch (tier)
        {
            case 1:
                towerToBuild = turretPrefab_T1;
                break;
            case 2:
                towerToBuild = turretPrefab_T2;
                break;
            case 3:
                towerToBuild = turretPrefab_T3;
                break;
            default:
                break;
        }

        return towerToBuild;
    }

    public GameObject GetWallToBuild()
    {
        return wallToBuild;
    }

    public GameObject GetMineToBuild()
    {
        return mineToBuild;
    }

    private void ResetDraggables()
    {
        dragTurretInstance.SetActive(false);
        dragWallInstance.SetActive(false);
        dragMineInstance.SetActive(false);
        dragRepairInstance.SetActive(false);
        legalPlaneInstance.SetActive(false);
        illegalPlaneInstance.SetActive(false);
    }

    public void BuildTower()
    {
        ResetDraggables();
        buildMode = BuildMode.Tower;
        objectToBuild = dragTurret;
        dragTurretInstance.SetActive(true);
    }

    public void BuildWall()
    {
        ResetDraggables();
        buildMode = BuildMode.Wall;
        objectToBuild = dragWall;
        dragWallInstance.SetActive(true);
    }

    public void BuildMine()
    {
        ResetDraggables();
        buildMode = BuildMode.Mine;
        objectToBuild = dragMine;
        dragMineInstance.SetActive(true);
    }

    public void Repair()
    {
        ResetDraggables();
        buildMode = BuildMode.Repair;
        objectToBuild = dragRepair;
        dragRepairInstance.SetActive(true);
    }

    private void Update()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;

        if (Physics.Raycast(ray, out hit1, 100, layerMask))
        {
            Debug.Log(hit1.transform.gameObject.name);
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

                        dragTurretInstance.SetActive(true);
                        //objectToBuild.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
                        dragTurretInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                        dragTurretInstance.transform.position = new Vector3(hit.transform.position.x, 0.5f, hit.transform.position.z);

                        if (hit.transform.CompareTag(towerNodeTag) && hit.transform.childCount == 0)
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    case BuildMode.Wall:

                        dragWallInstance.SetActive(true);
                        dragWallInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                        dragWallInstance.transform.position = new Vector3(hit.transform.position.x, 0.7f, hit.transform.position.z);

                        /*Collider[] hitColliders = Physics.OverlapBox(hit.transform.position, new Vector3(0.5f, 5f, 0.5f), Quaternion.identity, enemyMask);

                        for (int i = 0; i < hitColliders.Length; i++)
                        {
                            print("overlap: " + i + " " + hitColliders[i]);
                        }*/

                        if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0 && Physics.OverlapBox(hit.transform.position, new Vector3(0.55f, 5f, 0.55f), Quaternion.identity, enemyMask).Length == 0)
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    case BuildMode.Mine:

                        dragMineInstance.SetActive(true);
                        dragMineInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                        dragMineInstance.transform.position = new Vector3(hit.transform.position.x, 0.5f, hit.transform.position.z);

                        if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0)
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            illegalPlaneInstance.SetActive(true);
                            legalPlaneInstance.SetActive(false);
                        }

                        break;

                    case BuildMode.Repair:

                        dragRepairInstance.SetActive(true);
                        dragRepairInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                        dragRepairInstance.transform.position = new Vector3(hit.transform.position.x, 0.7f, hit.transform.position.z);

                        if (hit.transform.CompareTag(wallTag) || hit.transform.CompareTag(mainBaseTag))
                        {
                            legalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
                            legalPlaneInstance.SetActive(true);
                            illegalPlaneInstance.SetActive(false);
                        }
                        else
                        {
                            illegalPlaneInstance.transform.position = new Vector3(hit.transform.position.x, 0.25f, hit.transform.position.z);
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

                var mousePos = Input.mousePosition;
                mousePos.z = mainCamera.transform.position.y;
                //Debug.Log(mainCamera.ScreenToWorldPoint(mousePos));
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

                switch (buildMode)
                {
                    case BuildMode.None:

                        break;

                    case BuildMode.Tower:

                        dragTurretInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        dragTurretInstance.transform.position = worldPos;

                        break;

                    case BuildMode.Wall:

                        dragWallInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        dragWallInstance.transform.position = worldPos;

                        break;

                    case BuildMode.Mine:

                        dragMineInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        dragMineInstance.transform.position = worldPos;

                        break;

                    case BuildMode.Repair:

                        dragRepairInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        dragRepairInstance.transform.position = worldPos;

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
                                dragTurretInstance.SetActive(false);
                                GameObject turret = Instantiate(GetTowerToBuild(1), new Vector3(hit.transform.position.x, 0.2f, hit.transform.position.z), Quaternion.identity, hit.transform);
                                TDManager.instance.turrets.Add(turret.transform.GetChild(0));
                                objectToBuild = null;
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't build turret there.");
                            }

                            break;

                        case BuildMode.Wall:

                            if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0 && Physics.OverlapBox(hit.transform.position, new Vector3(0.55f, 5f, 0.55f), Quaternion.identity, enemyMask).Length == 0)
                            {
                                dragWallInstance.SetActive(false);
                                GameObject wall = Instantiate(GetWallToBuild(), new Vector3(hit.transform.position.x, 0.7f, hit.transform.position.z), Quaternion.identity, hit.transform);
                                TDManager.instance.walls.Add(wall.transform.GetChild(0));
                                objectToBuild = null;
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't build wall there.");
                            }

                            break;

                        case BuildMode.Mine:

                            if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0)
                            {
                                dragMineInstance.SetActive(false);
                                GameObject mine = Instantiate(GetMineToBuild(), new Vector3(hit.transform.position.x, 0.5f, hit.transform.position.z), Quaternion.identity, hit.transform);
                                TDManager.instance.mines.Add(mine.transform.GetChild(0));
                                objectToBuild = null;
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't build mine there.");
                            }

                            break;

                        case BuildMode.Repair:

                            if (hit.transform.CompareTag(wallTag) || hit.transform.CompareTag(mainBaseTag))
                            {
                                Wall wall = hit.transform.GetComponent<Wall>();
                                wall.currentHealth = wall.baseHealth;
                                objectToBuild = null;
                                buildMode = BuildMode.None;
                            }
                            else
                            {
                                Debug.Log("Can't repair there!");
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
