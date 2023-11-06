using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    //private NavMeshAgent agent;
    //private Transform destination;
    public float baseSpeed = 10f;
    public int type;

    private Health healthScript;
    private Attack attackScript;

    public float health;
    public float speed;
    public float size;
    private Vector3 sizeVector;
    public float damage;
    public float attackSpeed;

    [Header("Waypoints")]
    private Transform target;
    private int waypointIndex = 0;
    public bool attacking = false;
    public bool reachedEnd = false;

    public bool isFrozen = false;

    private void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();

        healthScript = transform.GetChild(0).GetComponent<Health>();
        attackScript = transform.GetChild(0).GetComponent<Attack>();

        sizeVector = enemy.localScale;
    }

    private void Start()
    {
        //destination = EnemyManager.instance.end;
        //agent.SetDestination(destination.position);
        //agent.speed = baseSpeed;

        target = Waypoints.waypoints[0];

        health = healthScript.baseHealth;
        speed = baseSpeed;
        size = enemy.localScale.x;
        damage = attackScript.damage;
        attackSpeed = attackScript.attackSpeed;

        enemy.LookAt(new Vector3(Waypoints.waypoints[waypointIndex].position.x, transform.position.y, Waypoints.waypoints[waypointIndex].position.z));

    }

    private void Update()
    {
        health = healthScript.currentHealth;

        if (!attacking && !reachedEnd)
        {
            Vector3 dir = target.position - enemy.position;
            enemy.Translate(speed * Time.deltaTime * dir.normalized, Space.World);

            Vector2 transformPosition = new Vector2(enemy.position.x, enemy.position.z);
            Vector2 targetPosition = new Vector2(target.position.x, target.position.z);

            if (Vector2.Distance(transformPosition, targetPosition) <= 0.4f)
            {
                GetNextWaypoint();
            }

            /*if (Vector3.Distance(transform.position, target.position) <= 1f)
            {
                GetNextWaypoint();
            }*/
        }
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            reachedEnd = true;
            return;
        }

        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];

        //Vector3 dir = target.position - enemy.position;
        //Quaternion lookRotation = Quaternion.LookRotation(dir);
        //Vector3 rotation = lookRotation.eulerAngles;
        //enemy.rotation = lookRotation;

        enemy.LookAt(new Vector3(Waypoints.waypoints[waypointIndex].position.x, transform.position.y, Waypoints.waypoints[waypointIndex].position.z));
    }

    public void UpdateStats()
    {
        healthScript.currentHealth = health;
        baseSpeed = speed;
        enemy.localScale = new Vector3(size, size, size);
        attackScript.damage = damage;
        attackScript.attackSpeed = attackSpeed;
    }

    public void UpgradeEnemy(float modifier)
    {
        transform.GetChild(0).GetComponent<Attack>().damage *= modifier;
        transform.GetChild(0).GetComponent<Health>().currentHealth *= modifier;
        transform.GetChild(0).GetComponent<Health>().baseHealth *= modifier;
    }
}
