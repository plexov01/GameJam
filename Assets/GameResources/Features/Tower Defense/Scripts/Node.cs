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

    private GameObject turret;
    private GameObject wall;
    private GameObject mine;
    private BuildManager buildManager;

    public LayerMask enemyMask;

    public bool path;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        nodeStartColor = Color.white;
        nodeHoverColor = Color.gray;
        nodeUnhoverColor = nodeStartColor;

        if (path)
        {
            pathStartColor = rend.material.GetColor("_FloorColor");
            pathUnhoverColor = pathStartColor;
        }
    }

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    private void OnMouseEnter()
    {
        if (buildManager.buildMode == BuildManager.BuildMode.None) return;

        if (buildManager.buildMode == BuildManager.BuildMode.Turret && transform.CompareTag("NodeForTurret"))
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

        if (buildManager.buildMode == BuildManager.BuildMode.Wall && transform.CompareTag("NodeForWall"))
        {
            if (path)
            {
                rend.material.SetColor("_FloorColor", pathHoverColor);
            }
            else
            {
                //
            }
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Mine && transform.CompareTag("NodeForWall"))
        {
            if (path)
            {
                rend.material.SetColor("_FloorColor", pathHoverColor);
            }
            else
            {
                //
            }
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Repair && (transform.CompareTag("WallBlock") || transform.CompareTag("MainBase")))
        {
            if (path)
            {
                rend.material.SetColor("_FloorColor", pathHoverColor);
            }
            else
            {
                //
            }
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
        if (buildManager.buildMode == BuildManager.BuildMode.None) return;

        if (buildManager.buildMode == BuildManager.BuildMode.Turret && transform.CompareTag("NodeForTurret"))
        {
            if (turret != null || transform.childCount != 0)
            {
                Debug.Log("Can't build turret there!");
                return;
            }
            else
            {
                GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(1);
                turret = Instantiate(turretToBuild, transform.position + new Vector3(0, 2f, 0), transform.rotation, transform);
                buildManager.buildMode = BuildManager.BuildMode.None;
                buildManager.turretCount++;

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

        if (buildManager.buildMode == BuildManager.BuildMode.Wall && transform.CompareTag("NodeForWall"))
        {
            if (wall != null || mine != null || Physics.BoxCast(transform.position, new Vector3(2f, 1f, 2f), Vector3.up, Quaternion.identity, 20f, enemyMask))
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
                wall = Instantiate(wallToBuild, transform.position + new Vector3(0, 2.5f, 0), transform.rotation, EnemyManager.instance.surface.transform);
                buildManager.buildMode = BuildManager.BuildMode.None;
                buildManager.wallCount++;

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

        if (buildManager.buildMode == BuildManager.BuildMode.Mine && transform.CompareTag("NodeForWall"))
        {
            if (wall != null || mine != null)
            {
                Debug.Log("Can't build mine there!");
                print(mine);
                return;
            }
            else
            {
                GameObject mineToBuild = BuildManager.instance.GetMineToBuild();
                mine = Instantiate(mineToBuild, transform.position + new Vector3(0, 0.6f, 0), transform.rotation, EnemyManager.instance.surface.transform);
                buildManager.buildMode = BuildManager.BuildMode.None;

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
                        soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt,Camera.main.transform.position);
                    }
                }
                else
                {
                    SoundManager soundManager = SoundManager.Instance;
                    soundManager.PlaySound(soundManager.audioClipRefsSo.thatsIt,Camera.main.transform.position);
                }
            }
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Repair)
        {
            if (transform.CompareTag("WallBlock") || transform.CompareTag("MainBase"))
            {
                Health health = transform.GetChild(0).GetComponent<Health>();
                health.currentHealth = health.baseHealth;
                buildManager.buildMode = BuildManager.BuildMode.None;

                if (path)
                {
                    rend.material.SetColor("_FloorColor", pathUnhoverColor);
                }
                else
                {
                    rend.material.color = nodeUnhoverColor;
                }
            }
            else
            {
                Debug.Log("Can't repair there!");
            }
        }

        /*if (wall != null && buildManager.buildMode == BuildManager.BuildMode.Wall)
        {
            Debug.Log("Can't build wall there!");
            return;
        }
        else if (transform.CompareTag("NodeForWall"))
        {
            GameObject wallToBuild = BuildManager.instance.GetWallToBuild();
            wall = Instantiate(wallToBuild, transform.position + new Vector3(0, 2.5f, 0), transform.rotation, EnemyManager.instance.surface.transform);
            buildManager.buildMode = BuildManager.BuildMode.None;
            buildManager.wallCount++;
            rend.material.color = startColor;
        }

        if (mine != null && buildManager.buildMode == BuildManager.BuildMode.Mine)
        {
            Debug.Log("Can't build mine there!");
            return;
        }
        else if (transform.CompareTag("NodeForWall"))
        {
            GameObject mineToBuild = BuildManager.instance.GetMineToBuild();
            mine = Instantiate(mineToBuild, transform.position + new Vector3(0, 0.6f, 0), transform.rotation, EnemyManager.instance.surface.transform);
            buildManager.buildMode = BuildManager.BuildMode.None;
            rend.material.color = startColor;
        }

        if (wall != null && buildManager.buildMode == BuildManager.BuildMode.Repair && transform.CompareTag("NodeForWall"))
        {
            print("repair wall");
            buildManager.buildMode = BuildManager.BuildMode.None;
            rend.material.color = startColor;
        }*/
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + new Vector3(0f, 2.5f, 0f), new Vector3(4f, 4f, 4f));
    }*/
}
