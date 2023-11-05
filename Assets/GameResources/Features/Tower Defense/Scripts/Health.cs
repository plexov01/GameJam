using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour, IDamageable
{
    public float baseHealth = 500f;
    public float currentHealth = 0f;
    [SerializeField] private GameObject objectToDestroy;
    
    private string enemyTag = "Enemy";
    private string wallTag = "WallBlock";

    private void Awake()
    {
        currentHealth = baseHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (objectToDestroy.CompareTag(wallTag))
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
                print(enemies.Length);

                foreach (GameObject enemy in enemies)
                {
                    if (enemy != null)
                    {
                        if (enemy.transform.GetChild(0).GetComponent<Attack>().damageable == transform.GetComponent<IDamageable>())
                        {
                            enemy.transform.GetComponent<NavMeshAgent>().speed = enemy.transform.GetComponent<Enemy>().speed;
                            enemy.transform.GetChild(0).GetComponent<Attack>().damageable = null;
                        }
                    }
                }
            }

            Destroy(objectToDestroy);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    /*private void OnDestroy()
    {
        if (objectToDestroy.CompareTag(wallTag))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            print(enemies.Length);

            foreach (GameObject enemy in enemies)
            {
                print(Vector3.Distance(enemy.transform.position, transform.position));

                if (enemy.GetComponent<NavMeshAgent>() != null && Vector3.Distance(enemy.transform.position, transform.position) < 4.5f)
                {
                    enemy.GetComponent<NavMeshAgent>().speed = enemy.GetComponent<Enemy>().baseSpeed;
                    enemy.transform.GetChild(0).GetComponent<Attack>().damageable = null;
                }
            }

            *//*if (EnemyManager.instance != null)
            {
                EnemyManager.instance.UpdateMesh();
            }*//*
        }
    }*/
}
