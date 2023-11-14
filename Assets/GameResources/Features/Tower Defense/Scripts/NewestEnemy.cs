using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewestEnemy : MonoBehaviour, IDamageable
{
    [Header("Enemy")]
    public int type;
    [HideInInspector] public float size;
    [SerializeField] private GameObject objectToDestroy;
    private Transform enemy;
    private EnemyManager enemyManager;

    [Header("Health")]
    public float baseHealth;
    public float currentHealth;

    [Header("Attack")]
    public float damage;
    public float attackSpeed;
    public bool attacking;

    [Header("Movement")]
    public float baseSpeed;
    public float currentSpeed;

    [Header("Waypoints")]
    private Vector3 target;
    private int nextPathCellIndex = 0;
    public bool reachedEnd = false;

    [Header("Freeze")]
    public bool isFrozen = false;
    public GameObject ice;

    private void Awake()
    {
        enemy = objectToDestroy.transform;
    }

    private void Start()
    {
        enemyManager = EnemyManager.instance;
        target = new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y);

        currentHealth = baseHealth;
        currentSpeed = baseSpeed;
        size = enemy.localScale.x;

        enemy.LookAt(new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y));
    }

    private void Update()
    {
        if (!attacking && !reachedEnd && enemyManager.pathRoute != null)
        {
            Vector3 dir = target - enemy.position;
            enemy.Translate(currentSpeed * Time.deltaTime * dir.normalized, Space.World);

            Vector2 transformPosition = new Vector2(enemy.position.x, enemy.position.z);
            Vector2 targetPosition = new Vector2(target.x, target.z);

            if (Vector2.Distance(transformPosition, targetPosition) <= 0.1f)
            {
                GetNextPathCell();
            }
        }
    }

    private void GetNextPathCell()
    {
        if (nextPathCellIndex >= enemyManager.pathRoute.Count - 1)
        {
            reachedEnd = true;
            print("Reached end");
            return;
        }

        nextPathCellIndex++;
        target = new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y);

        enemy.LookAt(new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y));
    }

    public void UpdateStats()
    {
        baseSpeed = currentSpeed;
        enemy.localScale = new Vector3(size, size, size);
    }

    public void UpgradeEnemy(float modifier)
    {
        damage *= modifier;
        baseHealth *= modifier;
        currentHealth *= modifier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            TakeDamage(TDManager.instance.meteorDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && objectToDestroy != null)
        {
            Death();
        }
    }

    public void Death()
    {
        TDManager.instance.enemies.Remove(transform);

        if (CoolnessScaleController.Instance.isDark)
        {
            CoolnessScaleController.Instance.AddCoolness(+5);
        }
        else
        {
            CoolnessScaleController.Instance.AddCoolness(-5);
        }

        Destroy(objectToDestroy);
    }
}
