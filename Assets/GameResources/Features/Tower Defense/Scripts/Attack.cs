using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    public float damage = 3f;

    public IDamageable damageable;

    public float attackSpeed = 1f;

    private Coroutine AttackCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag("Meteor"))
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(TDManager.instance.meteorDamage);
            }
        }

        if (other.CompareTag("Wall"))
        {
            damageable = other.GetComponent<IDamageable>();
            print(damageable);

            if (damageable != null && AttackCoroutine == null)
            {
                if (transform.parent.GetComponent<Enemy>() != null)
                {
                    transform.parent.GetComponent<Enemy>().attacking = true;
                    NavMeshAgent agent = transform.parent.GetComponent<NavMeshAgent>();
                    agent.velocity = Vector3.zero;
                    agent.speed = 0;
                    AttackCoroutine = StartCoroutine(AttackCroutine());
                }
            }
        }
    }

    private IEnumerator AttackCroutine()
    {
        yield return new WaitForSeconds(1f / attackSpeed);

        while (damageable != null)
        {
            //print("deal " + damage + " damage from " + transform.parent.name + " to " + damageable.GetTransform().parent.name);
            damageable.TakeDamage(damage);

            yield return new WaitForSeconds(1f/ attackSpeed);
        }

        AttackCoroutine = null;
    }
}
