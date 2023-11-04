using UnityEngine;

public class Node : MonoBehaviour
{
    private Color startColor;
    public Color hoverColor;
    private Renderer rend;

    private GameObject turret;
    private GameObject wall;
    private BuildManager buildManager;


    private void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    private void OnMouseEnter()
    {
        if (buildManager.buildTurret && transform.CompareTag("NodeForTurret"))
        { 
            rend.material.color = hoverColor;
        }
        else if (buildManager.buildWall && transform.CompareTag("NodeForWall"))
        {
            rend.material.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (rend.material.color != startColor)
        {
            rend.material.color = startColor;
        }
    }

    private void OnMouseDown()
    {
        if (turret != null)
        {
            Debug.Log("Can't build turret there!");
            return;
        }
        else if (buildManager.buildTurret && transform.CompareTag("NodeForTurret"))
        {
            GameObject turrentToBuild = BuildManager.instance.GetTurretToBuild();
            turret = Instantiate(turrentToBuild, transform.position + new Vector3(0, 0, 0), transform.rotation);
            buildManager.buildTurret = false;
            buildManager.turretCount++;
        }

        if (wall != null)
        {
            Debug.Log("Can't build wall there!");
            return;
        }
        else if (buildManager.buildWall && transform.CompareTag("NodeForWall"))
        {
            GameObject wallToBuild = BuildManager.instance.GetWallToBuild();
            wall = Instantiate(wallToBuild, transform.position + new Vector3(0, 2.5f, 0), transform.rotation, EnemyManager.instance.surface.transform);
            buildManager.buildWall = false;
            //EnemyManager.instance.UpdateMesh();
        }
    }
}
