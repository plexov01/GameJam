using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    [Header("Turret")]
    public int tier;
    [SerializeField] private GameObject objectToDestroy;

    [Header("Health")]
    public float baseHealth;
    public float currentHealth;

    [Header("Shooting")]
    public float damage = 100f;
    public float bulletSpeed = 17.5f;
    private Transform target = null;
    public float range = 3.75f;
    public float fireRate = 1f;
    private float fireCountdown = 0.25f;
    private string enemyTag = "Enemy";
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    private ParticleSystem partSys;

    [Header("Rotation")]
    [SerializeField] private Transform partToRotate;
    [SerializeField] private Transform barrel;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Freeze")]
    public bool isFrozen = false;
    public GameObject ice;

    private void Awake()
    {
        GetComponent<SphereCollider>().radius = range;
        // _basefireRate = fireRate;
        tempfireRate = fireRate;
        tempturnSpeed = turnSpeed;

        partSys = firePoint.GetComponent<ParticleSystem>();
        partSys.Stop();
    }

    private void Start()
    {
        currentHealth = baseHealth;
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

    private void Update()
    {
        if (isFrozen)
        {
            target = null;
            return;
        }

        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation * Quaternion.Euler(0, -90f, 0), Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);

        Vector3 dir2 = target.position - barrel.position;
        Quaternion lookRotation2 = Quaternion.LookRotation(dir2.normalized);
        Vector3 barrelRotation = Quaternion.Lerp(barrel.rotation, lookRotation2, Time.deltaTime * turnSpeed).eulerAngles;
        barrel.localRotation = Quaternion.Euler(0, 0, barrelRotation.z);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletGameObject.transform.parent = transform;
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        bullet.damage = damage;
        bullet.speed = bulletSpeed;

        partSys.Play();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
    private Coroutine freezeCoroutine = null;
    private float tempfireRate;
    private float tempturnSpeed;

    public void Freeze(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            freezeCoroutine = null;
            fireRate = tempfireRate;
            turnSpeed = tempturnSpeed;
            fireCountdown = 1f / fireRate;
            isFrozen = false;
        }

        freezeCoroutine = StartCoroutine(FreezeTurretCoroutine(duration));
    }

    /// <summary>
    /// Заморозить турель
    /// </summary>

    public IEnumerator FreezeTurretCoroutine(float time)
    {
        ice.SetActive(true);

        isFrozen = true;
        tempfireRate = fireRate;
        tempturnSpeed = turnSpeed;

        fireRate = 0;
        turnSpeed = 0;

        yield return new WaitForSeconds(time);
        fireRate = tempfireRate;
        turnSpeed = tempturnSpeed;
        fireCountdown = 1f / fireRate;
        isFrozen = false;

        ice.SetActive(false);
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
