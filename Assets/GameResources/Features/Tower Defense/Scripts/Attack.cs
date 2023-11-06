using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 3f;
    public IDamageable damageable;
    public float attackSpeed = 1f;
    public Coroutine attackCoroutine = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            print("entered wall trigger");

            damageable = other.GetComponent<IDamageable>();

            if (damageable != null && attackCoroutine == null)
            {
                if (transform.parent.GetComponent<Enemy>() != null)
                {
                    print(transform.parent.name);
                    transform.parent.GetComponent<Enemy>().attacking = true;
                    attackCoroutine = StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f / attackSpeed);

        while (damageable != null)
        {
            if (!transform.parent.GetComponent<Enemy>().isFrozen)
            {
                damageable.TakeDamage(damage);
            }

            yield return new WaitForSeconds(1f/ attackSpeed);
        }
        attackCoroutine = null;
    }


    private void Update()
    {
        if (damageable == null) 
        {
            StopAllCoroutines();
            attackCoroutine = null;
        }
    }
}
