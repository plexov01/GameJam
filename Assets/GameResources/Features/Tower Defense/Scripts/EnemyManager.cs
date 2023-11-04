using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<int> enemyOrder;

    public Transform end;
    public NavMeshSurface surface;

    private string enemyTag = "EnemyTrigger";

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

    void Start()
    {
        surface.BuildNavMesh();

        //StartCoroutine(UpdateNavMesh());

        StartCoroutine(SpawnEnemies(enemyOrder));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            StartCoroutine(SpawnEnemies(3, 0));
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            KillAllEnemies();
        }
    }

    public IEnumerator SpawnEnemies(int numberToSpawn, int enemyType)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Instantiate(enemyPrefabs[enemyType], Vector3.zero, Quaternion.identity, transform.GetChild(0));

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator SpawnEnemies(List<int> enemyOrder)
    {
        for (int i = 0; i < enemyOrder.Count; i++)
        {
            Instantiate(enemyPrefabs[enemyOrder[i]], Vector3.zero, Quaternion.identity, transform.GetChild(0));

            yield return new WaitForSeconds(1f);
        }
    }

    public void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<IDamageable>().TakeDamage(10000f, enemy.transform);
        }
    }

    public void IncreaseEnemiesHP()
    {

    }

    public void UpdateMesh()
    {
        print("update mesh");
        StartCoroutine(BuildNavMesh());
    }

    private IEnumerator BuildNavMesh()
    {
        yield return new WaitForSeconds(0.5f);

        surface.BuildNavMesh();
    }
}
