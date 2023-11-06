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
    private string wallTag = "WallBlock";

    private Coroutine freezeCoroutine = null;
    private Coroutine lavaFloorCoroutine = null;

    [Header("Meteor")]
    [SerializeField] private Transform[] nodesForMeteor;
    [SerializeField] private float meteorHeight;
    [SerializeField] private GameObject meteorPrefab;
    public float meteorDamage = 500f;
    
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
        if (GameHandler.Instance.IsSecondStageActive())
        {
            StartCoroutine(StartSpawningEnemies(0, 2f));
        }
        if (GameHandler.Instance.IsThirdStateActive())
        {
            StartCoroutine(StartSpawningEnemies(0, 1f));
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
            CoolnessScaleController.Instance.AddCoolness(-100);
            //Repair();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            CoolnessScaleController.Instance.AddCoolness(100);
            StartCoroutine(SpawnEnemies(1));
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            FreezeEnemies(3f);
            //LavaFloor(3f);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            ChangeTurretTier(true);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            ChangeTurretTier(false);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            ChangeEnemiesStats(0, 50, 1.5f, 1.5f, 5, 3);
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
        }
        else
        {
            if (buildManager.turretCount >= numberToDestroy)
            {
                List<GameObject> turrets = GameObject.FindGameObjectsWithTag(turretTag).ToList();

                Shuffle(turrets);

                for (int i = 0; i < numberToDestroy; i++)
                {
                    turrets[i].GetComponent<IDamageable>().TakeDamage(10000f);
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
                wall.transform.GetChild(0).GetComponent<IDamageable>().TakeDamage(10000f);
            }
        }
        else
        {
            if (buildManager.wallCount >= numberToDestroy)
            {
                List<GameObject> walls = GameObject.FindGameObjectsWithTag(wallTag).ToList();

                Shuffle(walls);

                for (int i = 0; i < numberToDestroy; i++)
                {
                    walls[i].transform.GetChild(0).GetComponent<IDamageable>().TakeDamage(10000f);
                }
            }
        }
    }

    private void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
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
            
            GameObject mob = Instantiate(enemyManager.enemyPrefabs[enemyType], transform.position, Quaternion.identity, transform.GetChild(0));
            mob.transform.GetChild(0).GetComponent<Enemy>().UpgradeEnemy(modifier);

            yield return new WaitForSeconds(spawanDelay);
        }
    }
    

    public IEnumerator SpawnEnemies(int numberToSpawn, int enemyType = 0, float spawanDelay = 1f)
    {
        SoundManager soundManager = SoundManager.Instance;

        int rndm = Random.Range(0, 2);
        switch (rndm)
        {
            case 0:
                soundManager.PlaySound(soundManager.audioClipRefsSo.attackRats,Camera.main.transform.position);
                break;
            case 1:
                soundManager.PlaySound(soundManager.audioClipRefsSo.lesgo,Camera.main.transform.position);
                break;
        }
        
        
        
        for (int i = 0; i < numberToSpawn; i++)
        {
            modifier += 0.02f;
            
            float coolness = CoolnessScaleController.Instance.GetCoolness();
            if (coolness <= 0.3f)
            {
                modifier += 0.01f;
            }
            
            GameObject mob = Instantiate(enemyManager.enemyPrefabs[enemyType], transform.position, Quaternion.identity, transform.GetChild(0));
            mob.transform.GetChild(0).GetComponent<Enemy>().UpgradeEnemy(modifier);

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
        float coolness = CoolnessScaleController.Instance.GetCoolness();
        
        List<GameObject> aims = new List<GameObject>();
        if (coolness<0.35f)
        {
            aims = GameObject.FindGameObjectsWithTag("Turret").ToList();
            foreach (GameObject aim in aims)
            {
                Turret turret = aim.GetComponentInChildren<Turret>();
                turret.Freeze(duration);
            }
            
        }else if (coolness < 0.65f)
        {
            print("freeze walls");
            aims = GameObject.FindGameObjectsWithTag("WallBlock").ToList();
            aims.AddRange(GameObject.FindGameObjectsWithTag("MainBase").ToList());
            
            foreach (GameObject aim in aims)
            {
                Health health = aim.GetComponentInChildren<Health>();

                health.currentHealth = health.baseHealth * 2;
                health.ice.SetActive(true);
            }

            yield return new WaitForSeconds(duration);

            foreach (GameObject aim in aims)
            {
                Health health = aim.GetComponentInChildren<Health>();

                if (health.currentHealth > health.baseHealth)
                {
                    health.currentHealth = health.baseHealth;
                }
                else
                {
                    health.currentHealth = health.baseHealth * 0.65f;
                }

                health.ice.SetActive(false);
            }

        }
        else
        {
            aims = GameObject.FindGameObjectsWithTag("Enemy").ToList();
            print("freeze coroutine");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().isFrozen = true;
                    enemy.GetComponent<Enemy>().speed = 0f;
                    enemy.GetComponentInChildren<Health>().ice.SetActive(true);
                }
            }
            
            yield return new WaitForSeconds(duration);
            
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().isFrozen = false;
                    enemy.GetComponent<Enemy>().speed = enemy.GetComponent<Enemy>().baseSpeed;
                    enemy.GetComponentInChildren<Health>().ice.SetActive(false);
                }
            }
            
            freezeCoroutine = null;
        }
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

    public void ChangeEnemiesStats(int enemyType = 0, float deltaHealth = 0, float speedDivider = 0, float sizeMultiplier = 1, float deltaDamage = 0, float deltaAttackSpeed = 0)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTriggerTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.transform.parent.GetComponent<Enemy>().type == enemyType)
            {
                Enemy stats = enemy.transform.parent.GetComponent<Enemy>();
                stats.health += deltaHealth;

                if (stats.speed / speedDivider > 4f)
                {
                    stats.speed /= speedDivider;
                }

                if (stats.size * sizeMultiplier < 5f)
                {
                    stats.size *= sizeMultiplier;
                }

                stats.damage += deltaDamage;
                stats.attackSpeed += deltaAttackSpeed;

                stats.UpdateStats();
            }
        }
        
        SoundManager soundManager = SoundManager.Instance;
        soundManager.PlaySound(soundManager.audioClipRefsSo.increaseThePressure,Camera.main.transform.position);
    }

    public void LavaFloor(float duration, float damage = 5f, float damageRate = 0.5f)
    {
        if (lavaFloorCoroutine != null)
        {
            StopCoroutine(lavaFloorCoroutine);
        }

        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }
        
        
        lavaFloorCoroutine = StartCoroutine(LavaFloorCoroutine(duration, damage, damageRate));
    }

    public IEnumerator LavaFloorCoroutine(float duration, float damage = 5f, float damageRate = 0.5f)
    {
        float timer = 0;

        foreach (Node node in buildManager.walkableNodes)
        {
            node.rend.material.color = buildManager.lavaColor;
            node.unhoverColor = buildManager.lavaColor;
        }

        while (timer < duration)
        {
            yield return new WaitForSeconds(damageRate);
            timer += damageRate;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTriggerTag);

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<IDamageable>().TakeDamage(damage);
                }
            }

            GameObject[] walls = GameObject.FindGameObjectsWithTag(wallTag);

            foreach (GameObject wall in walls)
            {
                if (wall != null)
                {
                    wall.transform.GetChild(0).GetComponent<IDamageable>().TakeDamage(damage);
                }
            }
        }

        foreach (Node node in buildManager.walkableNodes)
        {
            node.rend.material.color = node.startColor;
            node.unhoverColor = node.startColor;
        }
    }

    public void SpawnMeteor(Vector3 position)
    {
        Transform node = nodesForMeteor[Random.Range(0, nodesForMeteor.Length)];

        Vector3 spawnPosition = position;
        // node.position + new Vector3(0, meteorHeight, 0);
        
        position.y += meteorHeight;
        GameObject meteor = Instantiate(meteorPrefab, spawnPosition, transform.rotation);
    }

    public void ChangeTurretTier(bool upgrade)
    {
        // SoundManager soundManager = SoundManager.Instance;
        // soundManager.PlaySound(soundManager.audioClipRefsSo.stopRats,Camera.main.transform.position);
        
        List<GameObject> turrets = GameObject.FindGameObjectsWithTag(turretTag).ToList();

        if (upgrade)
        {
            List<GameObject> tier1Turrets = new List<GameObject>();
            List<GameObject> tier2Turrets = new List<GameObject>();

            for (int i = 0; i < turrets.Count; i++)
            {
                if (turrets[i] != null)
                {
                    if (turrets[i].GetComponent<Turret>().tier == 1)
                    {
                        tier1Turrets.Add(turrets[i]);
                    }
                    
                    if (turrets[i].GetComponent<Turret>().tier == 2)
                    {
                        tier2Turrets.Add(turrets[i]);
                    }
                }
            }

            if (tier1Turrets.Count > 0 && tier2Turrets.Count > 0)
            {
                int chooseTurret = Random.Range(0, 100);

                if (chooseTurret < 70)
                {
                    Shuffle(tier1Turrets);

                    Vector3 position = tier1Turrets[0].transform.position;
                    Transform node = tier1Turrets[0].transform.parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(2);
                    Destroy(tier1Turrets[0].transform.parent.gameObject);
                    Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                }
                else
                {
                    Shuffle(tier2Turrets);

                    Vector3 position = tier2Turrets[0].transform.position;
                    Transform node = tier2Turrets[0].transform.parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(3);
                    Destroy(tier2Turrets[0].transform.parent.gameObject);
                    Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                }
            }
            else if (tier1Turrets.Count > 0 && tier2Turrets.Count == 0)
            {
                Shuffle(tier1Turrets);

                Vector3 position = tier1Turrets[0].transform.position;
                Transform node = tier1Turrets[0].transform.parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(2);
                Destroy(tier1Turrets[0].transform.parent.gameObject);
                Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
            }
            else if (tier1Turrets.Count == 0 && tier2Turrets.Count > 0)
            {
                Shuffle(tier2Turrets);

                Vector3 position = tier2Turrets[0].transform.position;
                Transform node = tier2Turrets[0].transform.parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(3);
                Destroy(tier2Turrets[0].transform.parent.gameObject);
                Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
            }
        }
        else
        {
            List<GameObject> tier2Turrets = new List<GameObject>();
            List<GameObject> tier3Turrets = new List<GameObject>();

            for (int i = 0; i < turrets.Count; i++)
            {
                if (turrets[i] != null)
                {
                    if (turrets[i].GetComponent<Turret>().tier == 2)
                    {
                        tier2Turrets.Add(turrets[i]);
                    }

                    if (turrets[i].GetComponent<Turret>().tier == 3)
                    {
                        tier3Turrets.Add(turrets[i]);
                    }
                }
            }

            if (tier2Turrets.Count > 0 && tier3Turrets.Count > 0)
            {
                int chooseTurret = Random.Range(0, 100);

                if (chooseTurret < 70)
                {
                    Shuffle(tier2Turrets);

                    Vector3 position = tier2Turrets[0].transform.position;
                    Transform node = tier2Turrets[0].transform.parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(1);
                    Destroy(tier2Turrets[0].transform.parent.gameObject);
                    Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                }
                else
                {
                    Shuffle(tier3Turrets);

                    Vector3 position = tier3Turrets[0].transform.position;
                    Transform node = tier3Turrets[0].transform.parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(2);
                    Destroy(tier3Turrets[0].transform.parent.gameObject);
                    Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                }
            }
            else if (tier2Turrets.Count > 0 && tier3Turrets.Count == 0)
            {
                Shuffle(tier2Turrets);

                Vector3 position = tier2Turrets[0].transform.position;
                Transform node = tier2Turrets[0].transform.parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(1);
                Destroy(tier2Turrets[0].transform.parent.gameObject);
                Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
            }
            else if (tier2Turrets.Count == 0 && tier3Turrets.Count > 0)
            {
                Shuffle(tier3Turrets);

                Vector3 position = tier3Turrets[0].transform.position;
                Transform node = tier3Turrets[0].transform.parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTurretToBuild(2);
                Destroy(tier3Turrets[0].transform.parent.gameObject);
                Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
            }
        }
    }
}
