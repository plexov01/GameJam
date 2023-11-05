using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TDManager : MonoBehaviour
{
    public static TDManager instance = null;

    private BuildManager buildManager;
    private EnemyManager enemyManager;

    private string turretTag = "Turret";
    private string enemyTag = "Enemy";
    private string enemyTriggerTag = "EnemyTrigger";
    private string wallTag = "Wall";

    private Coroutine freezeCoroutine = null;

    [Header("Change enemy stats")]
    private float newHealth;
    
    [Header("Scaling")]
    float modifier = 1f;

    private void Awake()
    {
        instance = this;

        buildManager = GetComponent<BuildManager>();
        enemyManager = GetComponent<EnemyManager>();
    }

    private void Start()
    {
        GameHandler.OnStateChanged += GameHandler_OnStateChanged;
    }
    
    private void OnDestroy()
    {
        GameHandler.OnStateChanged -= GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsSecondOrThirdStageActive())
        {
            StartCoroutine(StartSpawningEnemies(0, 2f));
        }
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
            BuildMine();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Repair();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            StartCoroutine(SpawnEnemies(3));
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            KillAllEnemies();
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            FreezeEnemies(3f);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            ChangeEnemiesStats(0, 99, 25, 2, 25, 3);
        }
    }

    public void BuildTurret()
    {
        buildManager.buildMode = BuildManager.BuildMode.Turret;
    }

    public void DestroyTurrets(int numberToDestroy = 0)
    {
        if (numberToDestroy == 0)
        {
            GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);

            foreach (GameObject turret in turrets)
            {
                turret.GetComponent<IDamageable>().TakeDamage(10000f);
            }

            buildManager.turretCount = 0;
        }
        else
        {
            if (buildManager.turretCount >= numberToDestroy)
            {
                List<GameObject> turrets = GameObject.FindGameObjectsWithTag(turretTag).ToList();

                for (int i = 0; i < numberToDestroy; i++)
                {
                    int index = Random.Range(0, turrets.Count);

                    turrets[index].GetComponent<IDamageable>().TakeDamage(10000f);

                    turrets.Remove(turrets[index]);

                    buildManager.turretCount--;
                }
            }
        }
    }

    public void BuildWall()
    {
        buildManager.buildMode = BuildManager.BuildMode.Wall;
    }

    public void DestroyWalls(int numberToDestroy = 0)
    {
        if (numberToDestroy == 0)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag(wallTag);

            foreach (GameObject wall in walls)
            {
                wall.GetComponent<IDamageable>().TakeDamage(10000f);
            }

            buildManager.wallCount = 0;
        }
        else
        {
            if (buildManager.wallCount >= numberToDestroy)
            {
                List<GameObject> walls = GameObject.FindGameObjectsWithTag(wallTag).ToList();

                for (int i = 0; i < numberToDestroy; i++)
                {
                    int index = Random.Range(0, walls.Count);

                    walls[index].GetComponent<IDamageable>().TakeDamage(10000f);

                    walls.Remove(walls[index]);

                    buildManager.wallCount--;
                }
            }
        }
    }

    public void BuildMine()
    {
        buildManager.buildMode = BuildManager.BuildMode.Mine;
    }

    public void Repair()
    {
        buildManager.buildMode = BuildManager.BuildMode.Repair;
    }
    
    public IEnumerator StartSpawningEnemies(int enemyType = 0, float spawanDelay = 1f)
    {
        while (true)
        {
            modifier += 0.02f;
            
            float coolness = CoolnessScaleController.Instance.GetCoolness();
            if (coolness <= 0.3f)
            {
                modifier += 0.01f;
            }
            
            GameObject mob = Instantiate(enemyManager.enemyPrefabs[enemyType], Vector3.zero, Quaternion.identity, transform.GetChild(0));
            mob.GetComponent<Enemy>().UpgradeEnemy(modifier);

            yield return new WaitForSeconds(spawanDelay);
        }
    }
    

    public IEnumerator SpawnEnemies(int numberToSpawn, int enemyType = 0, float spawanDelay = 1f)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            modifier += 0.02f;
            
            float coolness = CoolnessScaleController.Instance.GetCoolness();
            if (coolness <= 0.3f)
            {
                modifier += 0.01f;
            }
            
            GameObject mob = Instantiate(enemyManager.enemyPrefabs[enemyType], Vector3.zero, Quaternion.identity, transform.GetChild(0));
            mob.GetComponent<Enemy>().UpgradeEnemy(modifier);

            yield return new WaitForSeconds(spawanDelay);
        }
    }

    public IEnumerator SpawnEnemies(List<int> enemyOrder)
    {
        for (int i = 0; i < enemyOrder.Count; i++)
        {
            Instantiate(enemyManager.enemyPrefabs[enemyOrder[i]], Vector3.zero, Quaternion.identity, transform.GetChild(0));

            yield return new WaitForSeconds(1f);
        }
    }

    public void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTriggerTag);

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<IDamageable>().TakeDamage(10000f);
        }
    }

    public void DestroyEverything()
    {
        KillAllEnemies();
        DestroyTurrets();
        DestroyWalls();
    }

    public void FreezeEnemies(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }

        freezeCoroutine = StartCoroutine(FreezeEnemiesCoroutine(duration));
    }

    private IEnumerator FreezeEnemiesCoroutine(float duration)
    {
        print("freeze coroutine");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                //NavMeshAgent agent = enemy.transform.parent.GetComponent<NavMeshAgent>();

                //enemy.transform.parent.GetComponent<NavMeshAgent>().speed = 0;
                //agent.velocity = Vector3.zero;

                enemy.GetComponent<Enemy>().speed = 0f;
            }
        }

        yield return new WaitForSeconds(duration);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                //NavMeshAgent agent = enemy.transform.parent.GetComponent<NavMeshAgent>();

                //agent.speed = enemy.transform.parent.GetComponent<Enemy>().baseSpeed;

                enemy.GetComponent<Enemy>().speed = enemy.GetComponent<Enemy>().baseSpeed;
            }
        }

        freezeCoroutine = null;
    }

    public void IncreaseEnemiesHP(int enemyType, float amount)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTriggerTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.transform.parent.GetComponent<Enemy>().type == enemyType)
            {
                enemy.GetComponent<Health>().currentHealth += amount;
            }
        }
    }

    public void ChangeEnemiesStats(int enemyType = 0, float deltaHealth = 0, float deltaSpeed = 0, float Size = 1, float deltaDamage = 0, float deltaAttackSpeed = 0)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTriggerTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.transform.parent.GetComponent<Enemy>().type == enemyType)
            {
                Enemy stats = enemy.transform.parent.GetComponent<Enemy>();
                stats.health += deltaHealth;
                stats.speed += deltaSpeed;
                stats.size = Size;
                stats.damage += deltaDamage;
                stats.attackSpeed *= deltaAttackSpeed;

                stats.UpdateStats();
            }
        }
    }
}
