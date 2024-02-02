using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IDamageable
{
    [Header("Tower")]
    public int tier;
    [SerializeField] private GameObject objectToDestroy;

    [Header("Health")]
    public float baseHealth;
    public float currentHealth;

    [Header("Shooting")]
    public float damage = 100f;
    public float bulletSpeed = 17.5f;
    protected Transform target = null;
    public float range = 3.75f;
    public float fireRate = 1f;
    private float fireCountdown = 0.25f;
    private string enemyTag;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject bulletPrefab;
    protected ParticleSystem partSys;

    [Header("Rotation")]
    [SerializeField] protected Transform partToRotate;
    [SerializeField] protected float turnSpeed = 10f;

    [Header("Freeze")]
    [SerializeField] private GameObject ice;
    [ReadOnlyInspector] public bool isFrozen = false;
    private Coroutine freezeCoroutine = null;

    private void Awake()
    {
        GetComponent<SphereCollider>().radius = range;

        partSys = firePoint.GetComponent<ParticleSystem>();
        partSys.Stop();
    }

    private void Start()
    {
        currentHealth = baseHealth;
        enemyTag = TDManager.instance.tags.enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.CompareTag(enemyTag))
        {
            target = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null && other.CompareTag(enemyTag))
        {
            UpdateTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag) && target != null && Vector3.Distance(transform.position, target.position) > range)
        {
            target = null;
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = float.MaxValue;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    protected virtual void Update()
    {
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected virtual void Shoot()
    {
        
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

    /// <summary>
    /// Заморозить турель
    /// </summary>

    private IEnumerator FreezeCoroutine(float duration)
    {
        ice.SetActive(true);
        isFrozen = true;

        yield return new WaitForSeconds(duration);
        
        ice.SetActive(false);
        isFrozen = false;

        freezeCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
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
        TDManager.instance.turrets.Remove(transform);

        Destroy(objectToDestroy);
    }
}
