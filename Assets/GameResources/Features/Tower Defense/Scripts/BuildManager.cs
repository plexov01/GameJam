using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance = null;

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
    public Buildable mine;
    public Buildable repair;

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
        mine.dragInstance = Instantiate(mine.dragPrefab, Vector3.zero, Quaternion.identity);
        mine.dragInstance.SetActive(false);
        repair.dragInstance = Instantiate(repair.dragPrefab, Vector3.zero, Quaternion.identity);
        repair.dragInstance.SetActive(false);
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

    public GameObject GetWallToBuild()
    {
        return wall.buildPrefab;
    }

    public GameObject GetMineToBuild()
    {
        return mine.buildPrefab;
    }

    private void ResetDraggables()
    {
        turret.dragInstance.SetActive(false);
        wall.dragInstance.SetActive(false);
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

                        //dragTurretInstance.SetActive(true);
                        //objectToBuild.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
                        //turret.dragInstance.transform.localScale = new Vector3(1f, 1f, 1f);
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

                        //dragWallInstance.SetActive(true);
                        //wall.dragInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                        wall.dragInstance.transform.position = new Vector3(hit.transform.position.x, wall.dragOffsetY, hit.transform.position.z);

                        /*Collider[] hitColliders = Physics.OverlapBox(hit.transform.position, new Vector3(0.5f, 5f, 0.5f), Quaternion.identity, enemyMask);

                        for (int i = 0; i < hitColliders.Length; i++)
                        {
                            print("overlap: " + i + " " + hitColliders[i]);
                        }*/

                        if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0 && Physics.OverlapBox(hit.transform.position, new Vector3(0.55f, 5f, 0.55f), Quaternion.identity, enemyMask).Length == 0)
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

                    case BuildMode.Mine:

                        //dragMineInstance.SetActive(true);
                        //mine.dragInstance.transform.localScale = new Vector3(1f, 1f, 1f);
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

                //var mousePos = Input.mousePosition;
                //mousePos.z = mainCamera.transform.position.y;
                //Debug.Log(mainCamera.ScreenToWorldPoint(mousePos));
                //Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out planeHit, 100, planeMask);

                switch (buildMode)
                {
                    case BuildMode.None:

                        break;

                    case BuildMode.Tower:

                        //turret.dragInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        //turret.dragInstance.transform.position = worldPos;
                        turret.dragInstance.transform.position = new Vector3(planeHit.point.x, 0, planeHit.point.z);

                        break;

                    case BuildMode.Wall:

                        //wall.dragInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        //wall.dragInstance.transform.position = worldPos;
                        wall.dragInstance.transform.position = new Vector3(planeHit.point.x, 0, planeHit.point.z);

                        break;

                    case BuildMode.Mine:

                        //mine.dragInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        //mine.dragInstance.transform.position = worldPos;
                        mine.dragInstance.transform.position = new Vector3(planeHit.point.x, 0, planeHit.point.z);

                        break;

                    case BuildMode.Repair:

                        //repair.dragInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        //repair.dragInstance.transform.position = worldPos;
                        //repair.dragInstance.transform.position = new Vector3(planeHit.point.x, 0, planeHit.point.z);

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
                                buildMode = BuildMode.None;

                                if (GameHandler.Instance.IsFirstStageActive())
                                {
                                    if (Random.value < 0.5f)
                                    {
                                        SoundManager soundManager = SoundManager.Instance;
                                        soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt, Camera.main.transform.position);
                                    }
                                }
                                else
                                {
                                    SoundManager soundManager = SoundManager.Instance;
                                    soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt, Camera.main.transform.position);
                                }
                            }
                            else
                            {
                                Debug.Log("Can't place turret there.");
                            }

                            break;

                        case BuildMode.Wall:

                            if (hit.transform.CompareTag(pathNodeTag) && hit.transform.childCount == 0 && Physics.OverlapBox(hit.transform.position, new Vector3(0.55f, 5f, 0.55f), Quaternion.identity, enemyMask).Length == 0)
                            {
                                wall.dragInstance.SetActive(false);
                                GameObject wallObject = Instantiate(GetWallToBuild(), new Vector3(hit.transform.position.x, wall.buildOffsetY, hit.transform.position.z), Quaternion.identity, hit.transform);
                                TDManager.instance.walls.Add(wallObject.transform.GetChild(0));
                                objectToBuild = null;
                                buildMode = BuildMode.None;

                                if (GameHandler.Instance.IsFirstStageActive())
                                {
                                    if (Random.value < 0.5f)
                                    {
                                        SoundManager soundManager = SoundManager.Instance;
                                        soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt, Camera.main.transform.position);
                                    }
                                }
                                else
                                {
                                    SoundManager soundManager = SoundManager.Instance;
                                    if (Random.value < 0.5f)
                                    {
                                        soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt, Camera.main.transform.position);
                                    }
                                    else
                                    {
                                        soundManager.PlaySound(soundManager.audioClipRefsSo.stopRats, Camera.main.transform.position);
                                    }

                                }
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
                                buildMode = BuildMode.None;

                                if (GameHandler.Instance.IsFirstStageActive())
                                {
                                    if (Random.value < 0.5f)
                                    {
                                        SoundManager soundManager = SoundManager.Instance;
                                        soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt, Camera.main.transform.position);
                                    }
                                }
                                else
                                {
                                    SoundManager soundManager = SoundManager.Instance;
                                    soundManager.PlaySound(soundManager.audioClipRefsSo.stopRats, Camera.main.transform.position);
                                }
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
                                buildMode = BuildMode.None;

                                SoundManager soundManager = SoundManager.Instance;

                                soundManager.PlaySound(soundManager.audioClipRefsSo.Upgrade, Camera.main.transform.position);
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
