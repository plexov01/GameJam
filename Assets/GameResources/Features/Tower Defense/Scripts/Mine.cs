using UnityEngine;

public class Mine : MonoBehaviour
{
    public float range = 15f;
    public float damage = 50f;
    private string enemyTag = "EnemyTrigger";

    private void OnTriggerEnter(Collider other)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                enemy.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}