using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform currentAttackTarget;
    [SerializeField] private Transform newAttackTarget;
    private Coroutine attackCoroutine = null;

    private string wallTag;
    private string mainBaseTag;
    private string mineTag;

    //[SerializeField] private int numberOfAvailableTargets = 0;

    private void Start()
    {
        TDManager tDManager = TDManager.instance;
        wallTag = tDManager.tags.wall;
        mainBaseTag = tDManager.tags.mainBase;
        mineTag = tDManager.tags.mine;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.tag);
        if (ValidTarget(other))
        {
            //numberOfAvailableTargets++;
            if (other.CompareTag(wallTag) || other.CompareTag(mainBaseTag))
            {
                print("entered wall trigger");
                newAttackTarget = other.transform;
            }
            else
            {
                print("entered landMine trigger");
                newAttackTarget = other.transform.parent.GetChild(0);
            }

            if (currentAttackTarget == null)
            {
                currentAttackTarget = newAttackTarget;
            }
            else if (currentAttackTarget.CompareTag(wallTag) && newAttackTarget.CompareTag(mineTag))
            {
                currentAttackTarget = newAttackTarget;
            }

            if (currentAttackTarget != null && attackCoroutine == null)
            {
                enemy.attacking = true;
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //print(other.tag);
        if (currentAttackTarget == null && ValidTarget(other))
        {
            if (other.CompareTag(wallTag) || other.CompareTag(mainBaseTag))
            {
                currentAttackTarget = other.transform;
            }
            else
            {
                currentAttackTarget = other.transform.parent.GetChild(0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("exit trigger " + other.tag);
        //currentAttackTarget = null;
        /*if (ValidTarget(other))
        {
            numberOfAvailableTargets--;

            if (numberOfAvailableTargets == 0)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
                enemy.attacking = false;
            }
        }*/
    }

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f / enemy.attackSpeed);

        while (currentAttackTarget != null)
        {
            if (!enemy.isFrozen)
            {
                currentAttackTarget.GetComponent<IDamageable>().TakeDamage(enemy.damage);
            }

            yield return new WaitForSeconds(1f / enemy.attackSpeed);
        }

        enemy.attacking = false;
        attackCoroutine = null;
    }

    private bool ValidTarget(Collider collider)
    {
        return (collider.CompareTag(wallTag) || collider.CompareTag(mainBaseTag) || collider.CompareTag(mineTag));
    }
}
