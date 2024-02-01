using UnityEngine;

public class TriggerForMeteor : MonoBehaviour
{
    [SerializeField] private Transform health;
    private IDamageable damageable = null;

    private void Awake()
    {
        if (health != null)
        {
            damageable = health.GetComponent<IDamageable>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            if (damageable != null)
            {
                //damageable.TakeDamage(TDManager.instance.meteorDamage);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
