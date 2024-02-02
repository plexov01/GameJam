using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject objectToDestroy;
    public static event EventHandler OnMeteorExploded;
    [SerializeField] private SphereCollider col;
    [SerializeField] private float radius;
    private Rigidbody rb;

    private string projectileTag;

    private void Awake()
    {
        col.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        projectileTag = TDManager.instance.tags.projectile;
    }

    private void Update()
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.tag);
        rb.velocity = Vector3.zero;
        col.enabled = true;
        GetComponent<Animation>().Play();
    }

    public void ExplosionEnd()
    {
        SoundManager soundManager = SoundManager.Instance;
        soundManager.PlaySound(soundManager.audioClipRefsSo.meteor, Camera.main.transform.position);
        OnMeteorExploded?.Invoke(this, EventArgs.Empty);
        Destroy(objectToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("meteor entered tag: " + other.tag + " name: " + other.name);

        if (!other.CompareTag(projectileTag))
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(damage);
            }
            else if (other.transform.parent.GetComponentInChildren<IDamageable>() != null)
            {
                other.transform.parent.GetComponentInChildren<IDamageable>().TakeDamage(damage);
            }
        }
    }
}
