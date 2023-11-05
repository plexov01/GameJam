using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance = null;

    [HideInInspector] public GameObject turretToBuild;
    [HideInInspector] public GameObject wallToBuild;
    [HideInInspector] public GameObject mineToBuild;

    public GameObject standardTurretPrefab;
    public GameObject wallPrefab;
    public GameObject minePrefab;

    public Node[] walkableNodes;
    public Color lavaColor;

    public enum BuildMode
    {
        None,
        Turret,
        Wall,
        Mine,
        Repair
    }

    public BuildMode buildMode;

    public int turretCount = 0;
    public int wallCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buildMode = BuildMode.None;

        turretToBuild = standardTurretPrefab;
        wallToBuild = wallPrefab;
        mineToBuild = minePrefab;
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public GameObject GetWallToBuild()
    {
        return wallToBuild;
    }

    public GameObject GetMineToBuild()
    {
        return mineToBuild;
    }
}
