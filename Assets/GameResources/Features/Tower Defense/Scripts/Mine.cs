using System;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IDamageable
{
    public static event EventHandler OnMineExploded;

    [SerializeField] private int coolnessCost;
    [SerializeField] protected GameObject objectToDestroy;
    private SphereCollider damageCollider;

    [Header("Health")]
    public float baseHealth;
    public float currentHealth;

    [Header("Explosion")]
    public float range = 15f;
    public float damage = 50f;
    [SerializeField] private SphereCollider triggerCollider;
    private string enemyTag;
    public bool activated = false;
    public bool isReady = false;

    private void Awake()
    {
        damageCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        currentHealth = baseHealth;
        damageCollider.enabled = false;
        enemyTag = TDManager.instance.tags.enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag) && isReady)
        {
            if (!activated)
            {
                activated = true;
                GetComponent<MeshRenderer>().enabled = false;
                damageCollider.radius = range / transform.localScale.x;

                CoolnessScaleController.Instance.AddCoolness(coolnessCost);

                OnMineExploded?.Invoke(this, EventArgs.Empty);
                Death();
            }
            
            if (other.GetComponent<IDamageable>() != null)
            {
                print("mine explosion entered " + other.tag);
                other.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
    }

    private void Ready()
    {
        isReady = true;
        damageCollider.enabled = true;
        //triggerCollider.enabled = false;
        //triggerCollider.radius = 0f;
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
        TDManager.instance.mines.Remove(transform);
        Destroy(objectToDestroy, 0.05f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
