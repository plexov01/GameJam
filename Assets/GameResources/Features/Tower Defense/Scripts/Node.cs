using UnityEngine;

public class Node : MonoBehaviour
{
    public Color startColor;
    public Color hoverColor;
    public Color unhoverColor;
    public Renderer rend;

    private GameObject turret;
    private GameObject wall;
    private GameObject mine;
    private BuildManager buildManager;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        unhoverColor = startColor;
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
            rend.material.color = hoverColor;
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Wall && transform.CompareTag("NodeForWall"))
        {
            rend.material.color = hoverColor;
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Mine && transform.CompareTag("NodeForWall"))
        {
            rend.material.color = hoverColor;
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Repair && (transform.CompareTag("WallBlock") || transform.CompareTag("MainBase")))
        {
            rend.material.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (rend.material.color != unhoverColor)
        {
            rend.material.color = unhoverColor;
        }
    }

    private void OnMouseDown()
    {
        if (buildManager.buildMode == BuildManager.BuildMode.None) return;

        if (buildManager.buildMode == BuildManager.BuildMode.Turret && transform.CompareTag("NodeForTurret"))
        {
            if (turret != null)
            {
                Debug.Log("Can't build turret there!");
                return;
            }
            else
            {
                GameObject turrentToBuild = BuildManager.instance.GetTurretToBuild();
                turret = Instantiate(turrentToBuild, transform.position + new Vector3(0, 0, 0), transform.rotation);
                buildManager.buildMode = BuildManager.BuildMode.None;
                buildManager.turretCount++;
                rend.material.color = unhoverColor;
            }
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Wall && transform.CompareTag("NodeForWall"))
        {
            if (wall != null || mine != null)
            {
                Debug.Log("Can't build wall there!");
                return;
            }
            else
            {
                GameObject wallToBuild = BuildManager.instance.GetWallToBuild();
                wall = Instantiate(wallToBuild, transform.position + new Vector3(0, 2.5f, 0), transform.rotation, EnemyManager.instance.surface.transform);
                buildManager.buildMode = BuildManager.BuildMode.None;
                buildManager.wallCount++;
                rend.material.color = unhoverColor;
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
                rend.material.color = unhoverColor;
            }
        }

        if (buildManager.buildMode == BuildManager.BuildMode.Repair)
        {
            if (transform.CompareTag("WallBlock") || transform.CompareTag("MainBase"))
            {
                Health health = transform.GetChild(0).GetComponent<Health>();
                health.currentHealth = health.baseHealth;
                buildManager.buildMode = BuildManager.BuildMode.None;
                rend.material.color = unhoverColor;
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
}
