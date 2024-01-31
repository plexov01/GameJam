using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy")]
    public int type;
    [SerializeField] private int coolnessCost;
    [SerializeField] protected GameObject objectToDestroy;
    [HideInInspector] public float size;
    private Transform enemyTransform;
    private EnemyManager enemyManager;

    [Header("Health")]
    public float baseHealth;
    public float currentHealth;

    [Header("Attack")]
    public float damage;
    public float attackSpeed;
    [ReadOnlyInspector] public bool attacking;

    [Header("Movement")]
    public float baseSpeed;
    public float currentSpeed;
    // Waypoints
    private Vector3 target;
    private int nextPathCellIndex = 0;
    [ReadOnlyInspector, SerializeField] private bool reachedEnd = false;

    [Header("Freeze")]
    [SerializeField] private GameObject ice;
    [ReadOnlyInspector] public bool isFrozen = false;
    private Coroutine freezeCoroutine = null;

    protected virtual void Start()
    {
        enemyTransform = objectToDestroy.transform;
        enemyManager = EnemyManager.instance;
        target = new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y);

        currentHealth = baseHealth;
        currentSpeed = baseSpeed;
        size = enemyTransform.localScale.x;

        enemyTransform.LookAt(new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y));
    }

    protected virtual void Update()
    {
        if (!attacking && !isFrozen && !reachedEnd && enemyManager.pathRoute != null)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 dir = target - enemyTransform.position;
        enemyTransform.Translate(currentSpeed * Time.deltaTime * dir.normalized, Space.World);

        Vector2 transformPosition = new Vector2(enemyTransform.position.x, enemyTransform.position.z);
        Vector2 targetPosition = new Vector2(target.x, target.z);

        if (Vector2.Distance(transformPosition, targetPosition) <= 0.1f)
        {
            GetNextPathCell();
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

        enemyTransform.LookAt(new Vector3(enemyManager.pathRoute[nextPathCellIndex].x, transform.position.y, enemyManager.pathRoute[nextPathCellIndex].y));
    }

    public void UpdateStats()
    {
        if (!isFrozen)
        {
            currentSpeed = baseSpeed;
        }

        enemyTransform.localScale = new Vector3(size, size, size);
    }

    public void UpgradeEnemy(float modifier)
    {
        damage *= modifier;
        baseHealth *= modifier;
        currentHealth *= modifier;
    }

    public void Freeze(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            freezeCoroutine = null;
        }

        freezeCoroutine = StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        ice.SetActive(true);
        currentSpeed = 0f;
        isFrozen = true;

        yield return new WaitForSeconds(duration);

        ice.SetActive(false);
        currentSpeed = baseSpeed;
        isFrozen = false;

        freezeCoroutine = null;
    }

    public void UnFreeze()
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            freezeCoroutine = null;
            ice.SetActive(false);
            currentSpeed = baseSpeed;
            isFrozen = false;
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
            CoolnessScaleController.Instance.AddCoolness(+coolnessCost);
        }
        else
        {
            CoolnessScaleController.Instance.AddCoolness(-coolnessCost);
        }

        Destroy(objectToDestroy);
    }
}
