using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public float damage = 15f;

    public float speed = 70f;

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
        Destroy(gameObject);
        IDamageable damageable = target.GetChild(0).GetComponent<IDamageable>();
        damageable.TakeDamage(damage);
    }
}
