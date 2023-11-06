using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance = null;

    [HideInInspector] public GameObject turretToBuild;
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

        wallToBuild = wallPrefab;
        mineToBuild = minePrefab;
    }

    public GameObject GetTurretToBuild(int tier)
    {
        switch (tier)
        {
            case 1:
                turretToBuild = turretPrefab_T1;
                break;
            case 2:
                turretToBuild = turretPrefab_T2;
                break;
            case 3:
                turretToBuild = turretPrefab_T3;
                break;
            default:
                break;
        }

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
