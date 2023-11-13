using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [SerializeField] private GameObject impactEffect;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(effect, 1f);

        Destroy(gameObject);
        target.GetComponent<IDamageable>().TakeDamage(damage);
    }
}
