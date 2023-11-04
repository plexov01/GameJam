using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance = null;

    [HideInInspector] public GameObject turretToBuild;
    [HideInInspector] public GameObject wallToBuild;

    public GameObject standardTurretPrefab;
    public GameObject wallPrefab;

    public bool buildTurret;
    public bool buildWall;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        turretToBuild = standardTurretPrefab;
        wallToBuild = wallPrefab;
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public GameObject GetWallToBuild()
    {
        return wallToBuild;
    }
}
