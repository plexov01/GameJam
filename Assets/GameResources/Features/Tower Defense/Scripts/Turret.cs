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
    
    

    private void Awake()
    {
        GetComponent<SphereCollider>().radius = range / 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.CompareTag("Enemy"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null)
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
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);

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
        
        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
