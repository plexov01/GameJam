using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    private Vector3 spawnPoint;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<int> enemyOrder;

    public Transform end;
    public NavMeshSurface surface;
    [SerializeField] private GameObject path;
    private bool pathState = true;
    //[SerializeField] private GameObject wall;
    //private bool wallState = false;

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

        spawnPoint = transform.position;
    }

    void Start()
    {
        path.SetActive(pathState);
        //wall.SetActive(wallState);

        surface.BuildNavMesh();

        //StartCoroutine(UpdateNavMesh());

        StartCoroutine(SpawnEnemies(enemyOrder));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            print("path");
            pathState = !pathState;
            path.SetActive(pathState);
            //surface.BuildNavMesh();
        }

        /*if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            print("wall");
            wallState = !wallState;
            wall.SetActive(wallState);
            surface.BuildNavMesh();
        }*/

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            print("build navMesh");
            surface.BuildNavMesh();
            surface.BuildNavMesh();
        }
    }

    public IEnumerator SpawnEnemies(List<int> enemyOrder)
    {
        for (int i = 0; i < enemyOrder.Count; i++)
        {
            Instantiate(enemyPrefabs[enemyOrder[i]], Vector3.zero, Quaternion.identity, transform);

            yield return new WaitForSeconds(1f);
        }

        /*yield return new WaitForSeconds(5f);

        for (int i = 0; i < enemyOrder.Count; i++)
        {
            Instantiate(enemyPrefabs[enemyOrder[i]], Vector3.zero, Quaternion.identity, transform);

            yield return new WaitForSeconds(1f);
        }*/
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
