using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTaa : Enemy
{
    [Header("Shooting")]
    [SerializeField] private float shootCooldown;
    [SerializeField, ReadOnlyInspector] private float shootTimer;
    [SerializeField] private Vector3 shootForce;
    private int shootDirection;

    [SerializeField] private GameObject bomb;

    protected override void Start()
    {
        base.Start();

        shootTimer = shootCooldown;
    }

    protected override void Update()
    {
        base.Update();

        if (isFrozen)
        {
            shootTimer = shootCooldown;
        }
        else if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        if (Random.Range(0, 2) == 0)
        {
            shootDirection = 1;
        }
        else
        {
            shootDirection = -1;
        }

        float angle = Random.Range(45, 136) * shootDirection;
        //print("angle: " + angle);

        GameObject bombObject = Instantiate(bomb, transform.position + new Vector3(0, 1f, 0), objectToDestroy.transform.rotation);
        bombObject.GetComponent<Rigidbody>().AddForce(Vector3.Scale(Quaternion.Euler(0f, angle, 0f) * bombObject.transform.forward, shootForce) + new Vector3(0f, shootForce.y, 0f), ForceMode.Impulse);
        //print(Vector3.Scale(Quaternion.Euler(0f, angle, 0f) * bombObject.transform.forward, shootForce));
    }

}
