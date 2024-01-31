using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private Transform attackTarget;
    private Coroutine attackCoroutine = null;

    void Update()
    {
        if (attackTarget == null && attackCoroutine != null)
        {
            //print("stop coroutine");
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            enemy.attacking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("MainBase"))
        {
            //print("entered wall trigger");

            attackTarget = other.transform;

            if (attackTarget != null && attackCoroutine == null)
            {
                enemy.attacking = true;
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
    }

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f / enemy.attackSpeed);

        while (attackTarget != null)
        {
            if (!enemy.isFrozen)
            {
                attackTarget.GetComponent<IDamageable>().TakeDamage(enemy.damage);
            }

            yield return new WaitForSeconds(1f / enemy.attackSpeed);
        }

        enemy.attacking = false;
        attackCoroutine = null;
    }
}
