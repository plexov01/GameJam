using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    [SerializeField] private float damage = 3f;

    private IDamageable damageable;

    [SerializeField] private float attackDelay = 1f;

    private Coroutine AttackCoroutine;

    [SerializeField] private bool isBullet;

    private void OnTriggerEnter(Collider other)
    {
        //print("entered trigger");
        damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            //print(damageable.GetTransform().parent.name);
            if (AttackCoroutine == null)
            {
                /*print("target " + GetComponent<Bullet>().target + " other " + other.transform);
                Debug.Log(GetComponent<Bullet>().target.gameObject);
                Debug.Log(other.gameObject);*/
                if (isBullet && GetComponent<Bullet>().target == other.transform.parent)
                {
                    print("bullet");
                    damageable.TakeDamage(damage, GetComponent<Bullet>().target);
                    Destroy(gameObject);
                    return;
                }
                else if (!isBullet)
                {
                    //print("this");
                    transform.parent.GetComponent<NavMeshAgent>().speed = 0;
                    AttackCoroutine = StartCoroutine(AttackCroutine(other.transform));
                }
            }
        }
    }

    private IEnumerator AttackCroutine(Transform target)
    {
        print("attack");
        WaitForSeconds Wait = new WaitForSeconds(attackDelay);

        yield return Wait;

        print(target.parent.name);
        print(damageable);

        if (target.GetComponent<IDamageable>() != null)
        {
            damageable = target.GetComponent<IDamageable>();
        }
        

        while (damageable != null)
        {

            //print("deal " + damage + " damage from " + transform.parent.name + " to " + damageable.GetTransform().parent.name);
            damageable.TakeDamage(damage, target);

            yield return Wait;
        }

        AttackCoroutine = null;
    }
}
