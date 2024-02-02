using UnityEngine;

public class MineTrigger : MonoBehaviour
{
    [SerializeField] private Mine mine;
    private string enemyTag;

    private void Start()
    {
        enemyTag = TDManager.instance.tags.enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("mine explosion entered " + other.tag);
        if (other.CompareTag(enemyTag) && other.GetComponent<IDamageable>() != null && mine.activated)
        {
            other.GetComponent<IDamageable>().TakeDamage(mine.damage);
        }
    }
}
