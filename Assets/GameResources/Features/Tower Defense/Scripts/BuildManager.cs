using System.Collections.Generic;
using System.Linq;
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

    public int turretCount = 0;
    private string turretTag = "Turret";


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            BuildTurret();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            BuildWall();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            DestroyTurrets(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            DestroyTurrets();
        }
    }

    public void BuildTurret()
    {
        buildTurret = true;
    }

    public void BuildWall()
    {
        buildWall = true;
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public GameObject GetWallToBuild()
    {
        return wallToBuild;
    }

    public void DestroyTurrets(int numberToDestroy = 0)
    {
        if (numberToDestroy == 0)
        {
            GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);

            foreach (GameObject turret in turrets)
            {
                turret.GetComponent<IDamageable>().TakeDamage(10000f, turret.transform);
            }

            turretCount = 0;
        }
        else
        {
            if (turretCount >= numberToDestroy)
            {
                List<GameObject> turrets = GameObject.FindGameObjectsWithTag(turretTag).ToList();

                for (int i = 0; i < numberToDestroy; i++)
                {
                    int index = Random.Range(0, turrets.Count);

                    turrets[index].GetComponent<IDamageable>().TakeDamage(10000f, turrets[index].transform);

                    turrets.Remove(turrets[index]);

                    turretCount--;
                }
            }
        }
    }
}
