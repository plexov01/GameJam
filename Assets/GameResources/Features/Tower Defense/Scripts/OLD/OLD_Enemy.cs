using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD_Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy")]
    public int type;
    [HideInInspector] public float size;
    [SerializeField] private GameObject objectToDestroy;
    private Transform enemy;

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
    private Transform target;
    private int waypointIndex = 0;
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
        target = Waypoints.waypoints[0];

        currentHealth = baseHealth;
        currentSpeed = baseSpeed;
        size = enemy.localScale.x;

        enemy.LookAt(new Vector3(Waypoints.waypoints[waypointIndex].position.x, transform.position.y, Waypoints.waypoints[waypointIndex].position.z));
    }

    private void Update()
    {
        if (!attacking && !reachedEnd)
        {
            Vector3 dir = target.position - enemy.position;
            enemy.Translate(currentSpeed * Time.deltaTime * dir.normalized, Space.World);

            Vector2 transformPosition = new Vector2(enemy.position.x, enemy.position.z);
            Vector2 targetPosition = new Vector2(target.position.x, target.position.z);

            if (Vector2.Distance(transformPosition, targetPosition) <= 0.4f)
            {
                GetNextWaypoint();
            }
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

        enemy.LookAt(new Vector3(Waypoints.waypoints[waypointIndex].position.x, transform.position.y, Waypoints.waypoints[waypointIndex].position.z));
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
