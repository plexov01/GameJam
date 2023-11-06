using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target = null;

    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    private string enemyTag = "Enemy";
    public Transform partToRotate;
    [SerializeField] private float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] private Transform barrel;

    public GameObject impactEffect;

    public int tier;

    private bool isFrozen = false;
    
    private void Awake()
    {
        GetComponent<SphereCollider>().radius = range / 4;
        // _basefireRate = fireRate;
        tempfireRate = fireRate;
        tempturnSpeed = turnSpeed;
    }

    private void Start()
    {
        fireCountdown = 0.25f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.CompareTag("EnemyTrigger"))
        {
            target = other.transform.parent;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null && other.CompareTag("EnemyTrigger"))
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
    }

    private void OnTriggerExit(Collider other)
    {
        print("exit trigger: " + other.tag);
        if (other.CompareTag("EnemyTrigger"))
        {
            print("exit trigger");
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
            barrel.localEulerAngles = new Vector3(0, 0, 0);
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation * Quaternion.Euler(0, -90f, 0), Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);

        barrel.localEulerAngles = new Vector3(0, 0, -30f);

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

        GameObject effect = Instantiate(impactEffect, firePoint.position, partToRotate.rotation * Quaternion.Euler(90f, 0, 0));

        Destroy(effect, 1f);

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
