using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Tower
{
    [Header("Turret")]
    [SerializeField] private Transform barrel;

    protected override void Update()
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

        base.Update();
    }

    protected override void Shoot()
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
}
