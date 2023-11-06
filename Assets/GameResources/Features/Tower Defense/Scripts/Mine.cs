using UnityEngine;

public class Mine : MonoBehaviour
{
    public float range = 15f;
    public float damage = 50f;
    private string enemyTag = "EnemyTrigger";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= range)
                {
                    enemy.GetComponent<IDamageable>().TakeDamage(damage);
                }
            }
            
            CoolnessScaleController.Instance.AddCoolness(40);

            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
