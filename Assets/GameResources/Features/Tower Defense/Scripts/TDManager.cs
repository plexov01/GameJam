using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TDManager : MonoBehaviour
{
    public static TDManager instance = null;

    private BuildManager buildManager;
    private EnemyManager enemyManager;
    public Vector3 spawnPoint;

    private Coroutine freezeCoroutine = null;
    private Coroutine lavaFloorCoroutine = null;

    [Header("Meteor")]
    [SerializeField] private float meteorHeight;
    [SerializeField] private GameObject meteorPrefab;
    public float meteorDamage = 500f;
    
    [Header("Scaling")]
    float modifier = 1f;
    private float lastStandModifier = 1f;

    [Header("Objects")]
    public List<Transform> enemies = new List<Transform>();
    public List<Transform> turrets = new List<Transform>();
    public List<Transform> walls = new List<Transform>();
    public List<Transform> mines = new List<Transform>();
    public Transform mainBase;

    [Header("Timers")]
    public float freezeTimer = 0f;
    public float lavaTimer = 0f;

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
            StartCoroutine(StartSpawningEnemies(0, 0.5f));
            lastStandModifier = 2f;
        }
    }

    private void Update()
    {
        // Cheats
        if (Input.GetKeyDown(KeyCode.T))
        {
            BuildTower();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            BuildWall();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildMine();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Repair();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeTurretTier(true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeTurretTier(false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Freeze(10f);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LavaFloor(10f);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnMeteor();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SpawnEnemies(10, 0, 0.2f));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeEnemiesStats(0, 400f, 1.5f, 1.5f, 5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShortCircuit();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            FindObjectOfType<GameJam.Features.UI.DarkController>()?.ShowDark();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CoolnessScaleController.Instance.canChangeCoolness = !CoolnessScaleController.Instance.canChangeCoolness;
        }
        
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            bool scaleStatus = CoolnessScaleController.Instance.canChangeCoolness;
            CoolnessScaleController.Instance.canChangeCoolness = true;
            CoolnessScaleController.Instance.AddCoolness(100);
            CoolnessScaleController.Instance.canChangeCoolness = scaleStatus;
        }
        
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            bool scaleStatus = CoolnessScaleController.Instance.canChangeCoolness;
            CoolnessScaleController.Instance.canChangeCoolness = true;
            CoolnessScaleController.Instance.AddCoolness(-100);
            CoolnessScaleController.Instance.canChangeCoolness = scaleStatus;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(SpawnEnemies(1, 0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(SpawnEnemies(1, 1));
        }

        // Timers
        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
        }
        else
        {
            freezeTimer = 0;
        }

        if (lavaTimer > 0)
        {
            lavaTimer -= Time.deltaTime;
        }
        else
        {
            lavaTimer = 0;
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

    public void BuildTower()
    {
        //buildManager.buildMode = BuildManager.BuildMode.Tower;
        buildManager.BuildTower();
    }

    public void DestroyTowers(int numberToDestroy = 0)
    {
        List<Transform> towerList = new(turrets);

        if (numberToDestroy == 0)
        {
            foreach (Transform tower in towerList)
            {
                if (tower != null)
                {
                    tower.GetComponent<IDamageable>().Death();
                }
            }
        }
        else
        {
            if (towerList.Count >= numberToDestroy)
            {
                Shuffle(towerList);

                for (int i = 0; i < numberToDestroy; i++)
                {
                    if (towerList[i] != null)
                    {
                        towerList[i].GetComponent<IDamageable>().Death();
                    }
                }
            }
        }
    }

    public void BuildWall()
    {
        buildManager.BuildWall();
    }

    public void DestroyWalls(int numberToDestroy = 0)
    {
        List<Transform> wallList = new(walls);

        if (numberToDestroy == 0)
        {
            foreach (Transform wall in wallList)
            {
                if (wall != null)
                {
                    wall.GetComponent<IDamageable>().Death();
                }
            }
        }
        else
        {
            if (wallList.Count >= numberToDestroy)
            {
                Shuffle(wallList);

                for (int i = 0; i < numberToDestroy; i++)
                {
                    if (wallList[i] != null)
                    {
                        wallList[i].GetComponent<IDamageable>().Death();
                    }
                }
            }
        }
    }

    public void BuildMine()
    {
        buildManager.BuildMine();
    }

    public void Repair()
    {
        buildManager.Repair();
    }
    
    public IEnumerator StartSpawningEnemies(int enemyType = 0, float spawanDelay = 1f)
    {
        while (true)
        {
            modifier += 0.05f;
            
            float coolness = CoolnessScaleController.Instance.GetCoolness();
            if (coolness <= 0.3f)
            {
                modifier += 0.03f;
            }
            
            GameObject mob = Instantiate(enemyManager.enemyPrefabs[enemyType], spawnPoint, Quaternion.identity, transform.GetChild(0));
            mob.transform.GetChild(0).GetComponent<Enemy>().UpgradeEnemy(modifier * lastStandModifier);
            enemies.Add(mob.transform.GetChild(0));

            yield return new WaitForSeconds(spawanDelay);
        }
    }
    

    public IEnumerator SpawnEnemies(int numberToSpawn, int enemyType = 0, float spawanDelay = 1f)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            modifier += 0.05f;
            
            float coolness = CoolnessScaleController.Instance.GetCoolness();
            if (coolness <= 0.3f)
            {
                modifier += 0.03f;
            }
            
            GameObject mob = Instantiate(enemyManager.enemyPrefabs[enemyType], spawnPoint, Quaternion.identity, transform.GetChild(0));
            mob.transform.GetChild(0).GetComponent<Enemy>().UpgradeEnemy(modifier * lastStandModifier);
            enemies.Add(mob.transform.GetChild(0));

            yield return new WaitForSeconds(spawanDelay);
        }
    }

    /*public IEnumerator SpawnEnemies(List<int> enemyOrder)
    {
        for (int i = 0; i < enemyOrder.Count; i++)
        {
            Instantiate(enemyManager.enemyPrefabs[enemyOrder[i]], Vector3.zero, Quaternion.identity, transform.GetChild(0));

            yield return new WaitForSeconds(1f);
        }
    }*/

    public void KillAllEnemies()
    {
        List<Transform> enemyList = new(enemies);

        foreach (Transform enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.GetComponent<IDamageable>().Death();
            }
        }
    }

    public void DestroyEverything()
    {
        KillAllEnemies();
        DestroyTowers();
        DestroyWalls();
    }

    public void Freeze(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }

        freezeCoroutine = StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        float coolness = CoolnessScaleController.Instance.GetCoolness();
        freezeTimer = duration;
        
        if (coolness < 0.35f)
        {
            print("freeze towers");

            List<Transform> turretList = new(turrets);

            foreach (Transform turret in turretList)
            {
                if (turret != null)
                {
                    turret.GetComponent<Turret>().Freeze(duration);
                }
            }
            
        }
        else if (coolness < 0.65f)
        {
            print("freeze walls");

            List<Transform> wallList = new(walls);
            wallList.Add(mainBase);
            
            foreach (Transform wall in wallList)
            {
                if (wall != null)
                {
                    Wall stats = wall.GetComponent<Wall>();

                    stats.currentHealth = stats.baseHealth * 2;
                    stats.ice.SetActive(true);
                }
            }

            yield return new WaitForSeconds(duration);

            foreach (Transform wall in wallList)
            {
                if (wall != null)
                {
                    Wall stats = wall.GetComponent<Wall>();

                    if (stats.currentHealth >= stats.baseHealth)
                    {
                        stats.currentHealth = stats.baseHealth;
                    }
                    else
                    {
                        stats.currentHealth = stats.baseHealth * 0.65f;
                    }

                    stats.ice.SetActive(false);
                }
            }
        }
        else
        {
            print("freeze enemies");

            List<Transform> enemyList = new(enemies);

            foreach (Transform enemy in enemyList)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().Freeze(duration);
                }
            }
        }

        freezeCoroutine = null;
    }

    /*public void IncreaseEnemiesHP(int enemyType, float amount)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTriggerTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.transform.parent.GetComponent<Enemy>().type == enemyType)
            {
                enemy.GetComponent<Health>().currentHealth += amount;
            }
        }
    }*/

    public void ChangeEnemiesStats(int enemyType = 0, float deltaHealth = 0, float speedDivider = 0, float sizeMultiplier = 1, float deltaDamage = 0, float deltaAttackSpeed = 0)
    {
        List<Transform> enemyList = new(enemies);

        foreach (Transform enemy in enemyList)
        {
            if (enemy != null && enemy.GetComponent<EnemyRat>().type == enemyType)
            {
                EnemyRat stats = enemy.GetComponent<EnemyRat>();
                stats.currentHealth += deltaHealth;

                if (stats.baseSpeed / speedDivider > 1f)
                {
                    stats.baseSpeed /= speedDivider;
                }

                if (stats.size * sizeMultiplier < 1.5f)
                {
                    stats.size *= sizeMultiplier;
                }

                stats.damage += deltaDamage;
                stats.attackSpeed += deltaAttackSpeed;

                stats.UpdateStats();
            }
        }
    }

    public void LavaFloor(float duration, float damage = 10f, float damageRate = 0.5f)
    {
        if (lavaFloorCoroutine != null)
        {
            StopCoroutine(lavaFloorCoroutine);
        }
        
        lavaFloorCoroutine = StartCoroutine(LavaFloorCoroutine(duration, damage, damageRate));
    }

    public IEnumerator LavaFloorCoroutine(float duration, float damage = 5f, float damageRate = 0.5f)
    {
        float timer = 0;

        foreach (Renderer node in buildManager.pathNodes)
        {
            node.material.SetFloat("_SmoothSpawn", 0f);
            //node.pathUnhoverColor = Color.black;
        }

        while (timer < duration)
        {
            yield return new WaitForSeconds(damageRate);
            timer += damageRate;

            List<Transform> enemyList = new(enemies);

            foreach (Transform enemy in enemyList)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<IDamageable>().TakeDamage(damage);
                }
            }

            List<Transform> wallList = new(walls);

            foreach (Transform wall in wallList)
            {
                if (wall != null)
                {
                    wall.GetComponent<IDamageable>().TakeDamage(damage);
                }
            }
        }

        foreach (Renderer node in buildManager.pathNodes)
        {
            node.material.SetFloat("_SmoothSpawn", 1f);
            //node.pathUnhoverColor = node.pathStartColor;
        }

        lavaFloorCoroutine = null;
    }

    public void SpawnMeteor()
    {
        float coolness = CoolnessScaleController.Instance.GetCoolness();

        List<Transform> friendList = new List<Transform>();
        Transform aim;

        if (coolness < Random.Range(0f, 1f))
        {
            Debug.Log("Мыши");
            friendList.AddRange(walls);
            friendList.AddRange(turrets);
            friendList.AddRange(mines);
            friendList.Add(mainBase);

            aim = friendList[Random.Range(0, friendList.Count)];
            Instantiate(meteorPrefab, new Vector3(aim.position.x, aim.position.y + meteorHeight, aim.position.z), Quaternion.identity);
        }
        else if (enemies.Count != 0)
        {
            aim = enemies[Random.Range(0, enemies.Count)];
            Instantiate(meteorPrefab, new Vector3(aim.position.x, aim.position.y + meteorHeight, aim.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(meteorPrefab, new Vector3(spawnPoint.x, spawnPoint.y + meteorHeight, spawnPoint.z), Quaternion.identity);
        }
    }

    public void ChangeTurretTier(bool upgrade)
    {
        // SoundManager soundManager = SoundManager.Instance;
        // soundManager.PlaySound(soundManager.audioClipRefsSo.stopRats,Camera.main.transform.position);

        List<Transform> turretList = new(turrets);

        if (upgrade)
        {
            List<Transform> tier1Turrets = new List<Transform>();
            List<Transform> tier2Turrets = new List<Transform>();

            foreach (Transform turret in turretList)
            {
                if (turret != null)
                {
                    if (turret.GetComponent<Turret>().tier == 1)
                    {
                        tier1Turrets.Add(turret);
                    }

                    if (turret.GetComponent<Turret>().tier == 2)
                    {
                        tier2Turrets.Add(turret);
                    }
                }
            }

            if (tier1Turrets.Count > 0 && tier2Turrets.Count > 0)
            {
                if (Random.Range(0, 100) < 70)
                {
                    Shuffle(tier1Turrets);

                    Vector3 position = tier1Turrets[0].position;
                    Transform node = tier1Turrets[0].parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(2);
                    turrets.Remove(tier1Turrets[0]);
                    Destroy(tier1Turrets[0].parent.gameObject);
                    GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                    turrets.Add(newTurret.transform.GetChild(0));
                }
                else
                {
                    Shuffle(tier2Turrets);

                    Vector3 position = tier2Turrets[0].position;
                    Transform node = tier2Turrets[0].parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(3);
                    turrets.Remove(tier2Turrets[0]);
                    Destroy(tier2Turrets[0].parent.gameObject);
                    GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                    turrets.Add(newTurret.transform.GetChild(0));
                }
            }
            else if (tier1Turrets.Count > 0 && tier2Turrets.Count == 0)
            {
                Shuffle(tier1Turrets);

                Vector3 position = tier1Turrets[0].position;
                Transform node = tier1Turrets[0].parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(2);
                turrets.Remove(tier1Turrets[0]);
                Destroy(tier1Turrets[0].parent.gameObject);
                GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                turrets.Add(newTurret.transform.GetChild(0));
            }
            else if (tier1Turrets.Count == 0 && tier2Turrets.Count > 0)
            {
                Shuffle(tier2Turrets);

                Vector3 position = tier2Turrets[0].position;
                Transform node = tier2Turrets[0].parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(3);
                turrets.Remove(tier2Turrets[0]);
                Destroy(tier2Turrets[0].parent.gameObject);
                GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                turrets.Add(newTurret.transform.GetChild(0));
            }
        }
        else
        {
            List<Transform> tier2Turrets = new List<Transform>();
            List<Transform> tier3Turrets = new List<Transform>();

            foreach (Transform turret in turretList)
            {
                if (turret != null)
                {
                    if (turret.GetComponent<Turret>().tier == 2)
                    {
                        tier2Turrets.Add(turret);
                    }

                    if (turret.GetComponent<Turret>().tier == 3)
                    {
                        tier3Turrets.Add(turret);
                    }
                }
            }

            if (tier2Turrets.Count > 0 && tier3Turrets.Count > 0)
            {
                if (Random.Range(0, 100) < 70)
                {
                    Shuffle(tier2Turrets);

                    Vector3 position = tier2Turrets[0].position;
                    Transform node = tier2Turrets[0].parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(1);
                    turrets.Remove(tier2Turrets[0]);
                    Destroy(tier2Turrets[0].parent.gameObject);
                    GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                    turrets.Add(newTurret.transform.GetChild(0));
                }
                else
                {
                    Shuffle(tier3Turrets);

                    Vector3 position = tier3Turrets[0].position;
                    Transform node = tier3Turrets[0].parent.parent;

                    GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(2);
                    turrets.Remove(tier3Turrets[0]);
                    Destroy(tier3Turrets[0].parent.gameObject);
                    GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                    turrets.Add(newTurret.transform.GetChild(0));
                }
            }
            else if (tier2Turrets.Count > 0 && tier3Turrets.Count == 0)
            {
                Shuffle(tier2Turrets);

                Vector3 position = tier2Turrets[0].position;
                Transform node = tier2Turrets[0].parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(1);
                turrets.Remove(tier2Turrets[0]);
                Destroy(tier2Turrets[0].parent.gameObject);
                GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                turrets.Add(newTurret.transform.GetChild(0));
            }
            else if (tier2Turrets.Count == 0 && tier3Turrets.Count > 0)
            {
                Shuffle(tier3Turrets);

                Vector3 position = tier3Turrets[0].position;
                Transform node = tier3Turrets[0].parent.parent;

                GameObject turretToBuild = BuildManager.instance.GetTowerToBuild(2);
                turrets.Remove(tier3Turrets[0]);
                Destroy(tier3Turrets[0].parent.gameObject);
                GameObject newTurret = Instantiate(turretToBuild, position + new Vector3(0, 0, 0), transform.rotation, node);
                turrets.Add(newTurret.transform.GetChild(0));
            }
        }
    }

    public void ShortCircuit()
    {
        float coolness = CoolnessScaleController.Instance.GetCoolness();

        List<Transform> wallList = new(walls);

        foreach (Transform wall in wallList)
        {
            if (wall != null && Random.Range(0f, 1f) > coolness)
            {
                wall.GetComponent<IDamageable>().Death();
            }
        }

        List<Transform> turretList = new(turrets);

        foreach (Transform turret in turretList)
        {
            if (turret != null && Random.Range(0f, 1f) > coolness)
            {
                turret.GetComponent<IDamageable>().Death();
            }
        }

        List<Transform> mineList = new(mines);

        foreach (Transform mine in mineList)
        {
            if (mine != null && Random.Range(0f, 1f) > coolness)
            {
                Destroy(mine.parent.gameObject);
            }
        }

        List<Transform> enemyList = new(enemies);

        foreach (Transform enemy in enemyList)
        {
            if (enemy != null && Random.Range(0f, 1f) < coolness)
            {
                enemy.GetComponent<IDamageable>().Death();
            }
        }
    }
}
