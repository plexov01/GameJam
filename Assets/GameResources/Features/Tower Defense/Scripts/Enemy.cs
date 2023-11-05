using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform destination;
    public float baseSpeed;
    public int type;

    private Health healthScript;
    private Attack attackScript;

    public float health;
    public float speed;
    public float size;
    private Vector3 sizeVector;
    public float damage;
    public float attackSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        healthScript = transform.GetChild(0).GetComponent<Health>();
        attackScript = transform.GetChild(0).GetComponent<Attack>();

        sizeVector = transform.localScale;
    }

    private void Start()
    {
        destination = EnemyManager.instance.end;
        agent.SetDestination(destination.position);
        agent.speed = baseSpeed;

        health = healthScript.baseHealth;
        speed = baseSpeed;
        size = 1f;
        damage = attackScript.damage;
        attackSpeed = attackScript.attackSpeed;
    }

    private void Update()
    {
        health = healthScript.currentHealth;
    }

    public void UpdateStats()
    {
        healthScript.currentHealth = health;
        agent.speed = speed;
        transform.localScale = sizeVector * size;
        attackScript.damage = damage;
        attackScript.attackSpeed = attackSpeed;
    }
}
