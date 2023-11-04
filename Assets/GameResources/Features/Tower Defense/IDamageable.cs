using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Transform target);
    Transform GetTransform();
}