using UnityEngine;

public class Node : MonoBehaviour
{
    public Color nodeStartColor;
    public Color nodeHoverColor;
    public Color nodeUnhoverColor;

    public Color pathStartColor;
    public Color pathHoverColor;
    public Color pathUnhoverColor;

    public Renderer rend;

    private BuildManager buildManager;

    public LayerMask enemyMask;

    public bool path;

    private string towerNodeTag = "TowerNode";
    private string pathNodeTag = "PathNode";
    private string wallTag = "Wall";
    private string mainBaseTag = "MainBase";

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        nodeStartColor = Color.white;
        nodeHoverColor = Color.gray;
        nodeUnhoverColor = nodeStartColor;

        if (path)
        {
            pathStartColor = rend.material.GetColor("_FloorColor");
            //pathHoverColor = Color.black;
            pathUnhoverColor = pathStartColor;
        }
    }

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    private void OnMouseEnter()
    {
        switch (buildManager.buildMode)
        {
            case BuildManager.BuildMode.None:
                
                return;

            case BuildManager.BuildMode.Tower:
                
                if (transform.CompareTag(towerNodeTag))
                {
                    if (path)
                    {
                        rend.material.SetColor("_FloorColor", pathHoverColor);
                    }
                    else
                    {
                        rend.material.color = nodeHoverColor;
                    }
                }

                break;

            case BuildManager.BuildMode.Wall:
                
                if (transform.CompareTag(pathNodeTag) && path)
                {
                    rend.material.SetColor("_FloorColor", pathHoverColor);
                }

                break;

            case BuildManager.BuildMode.Mine:
                
                if (transform.CompareTag(pathNodeTag) && path)
                {
                    rend.material.SetColor("_FloorColor", pathHoverColor);
                }

                break;

            case BuildManager.BuildMode.Repair:

                if ((transform.CompareTag(wallTag) || transform.CompareTag(mainBaseTag)) && path)
                {
                    rend.material.SetColor("_FloorColor", pathHoverColor);
                }

                break;

            default:

                break;
        }
    }

    private void OnMouseExit()
    {
        if (path)
        {
            rend.material.SetColor("_FloorColor", pathUnhoverColor);
        }
        else
        {
            rend.material.color = nodeUnhoverColor;
        }
    }

    private void OnMouseDown()
    {
        switch (buildManager.buildMode)
        {
            case BuildManager.BuildMode.None:
                
                return;

            case BuildManager.BuildMode.Tower:

                if (transform.CompareTag(towerNodeTag))
                {
                    if (transform.childCount != 0)
                    {
                        Debug.Log("Can't build turret there!");
                        return;
                    }
                    else
                    {
                        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild(1);
                        GameObject tower = Instantiate(towerToBuild, transform.position + new Vector3(0, 2f, 0), transform.rotation, transform);
                        buildManager.buildMode = BuildManager.BuildMode.None;
                        TDManager.instance.turrets.Add(tower.transform.GetChild(0));

                        if (path)
                        {
                            rend.material.SetColor("_FloorColor", pathUnhoverColor);
                        }
                        else
                        {
                            rend.material.color = nodeUnhoverColor;
                        }

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
                }

                break;

            case BuildManager.BuildMode.Wall:

                if (transform.CompareTag(pathNodeTag))
                {
                    if (transform.childCount != 0 || Physics.BoxCast(transform.position, new Vector3(2f, 1f, 2f), Vector3.up, Quaternion.identity, 20f, enemyMask))
                    {
                        Debug.Log("Can't build wall there!");
                        return;
                    }
                    else
                    {
                        /*print(Physics.BoxCast(transform.position + new Vector3(0f, 2.5f, 0f), new Vector3(2f, 2f, 2f), Vector3.up, Quaternion.identity, 20f, enemyMask));
                        RaycastHit[] hits = Physics.BoxCastAll(transform.position + new Vector3(0f, 2.5f, 0f), new Vector3(2f, 2f, 2f), Vector3.up, Quaternion.identity, 20f, enemyMask);
                        print("hits length: " + hits.Length);
                        for (int i = 0; i < hits.Length; i++)
                        {
                            print(hits[i]);
                        }*/

                        GameObject wallToBuild = BuildManager.instance.GetWallToBuild();
                        GameObject wall = Instantiate(wallToBuild, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation, transform);
                        buildManager.buildMode = BuildManager.BuildMode.None;
                        TDManager.instance.walls.Add(wall.transform.GetChild(0));

                        if (path)
                        {
                            rend.material.SetColor("_FloorColor", pathUnhoverColor);
                        }
                        else
                        {
                            rend.material.color = nodeUnhoverColor;
                        }

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
                }

                break;

            case BuildManager.BuildMode.Mine:

                if (transform.CompareTag(pathNodeTag))
                {
                    if (transform.childCount != 0)
                    {
                        Debug.Log("Can't build mine there!");
                        return;
                    }
                    else
                    {
                        GameObject mineToBuild = BuildManager.instance.GetMineToBuild();
                        GameObject mine = Instantiate(mineToBuild, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation, transform);
                        buildManager.buildMode = BuildManager.BuildMode.None;
                        TDManager.instance.mines.Add(mine.transform.GetChild(0));

                        if (path)
                        {
                            rend.material.SetColor("_FloorColor", pathUnhoverColor);
                        }
                        else
                        {
                            rend.material.color = nodeUnhoverColor;
                        }

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
                }

                break;

            case BuildManager.BuildMode.Repair:

                if (transform.CompareTag(wallTag) || transform.CompareTag(mainBaseTag))
                {
                    Wall wall = transform.GetComponent<Wall>();
                    wall.currentHealth = wall.baseHealth;
                    buildManager.buildMode = BuildManager.BuildMode.None;

                    if (path)
                    {
                        rend.material.SetColor("_FloorColor", pathUnhoverColor);
                    }
                    else
                    {
                        rend.material.color = nodeUnhoverColor;
                    }

                    SoundManager soundManager = SoundManager.Instance;

                    soundManager.PlaySound(soundManager.audioClipRefsSo.Upgrade, Camera.main.transform.position);
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

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + new Vector3(0f, 2.5f, 0f), new Vector3(4f, 4f, 4f));
    }*/
}
