using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour, IDamageable
{
    public float health = 500;
    [SerializeField] private GameObject objectToDestroy;
    
    private string enemyTag = "Enemy";

    public void TakeDamage(float damage, Transform target)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(objectToDestroy);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnDestroy()
    {
        if (objectToDestroy.CompareTag("Wall"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            print(enemies.Length);

            foreach (GameObject enemy in enemies)
            {
                print(Vector3.Distance(enemy.transform.position, transform.position));

                if (enemy.GetComponent<NavMeshAgent>() != null && Vector3.Distance(enemy.transform.position, transform.position) < 4f)
                {
                    enemy.GetComponent<NavMeshAgent>().speed = enemy.GetComponent<EnemyMovement>().baseSpeed;
                }
            }

            if (EnemyManager.instance != null)
            {
                EnemyManager.instance.UpdateMesh();
            }
        }
    }
}
