using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float range = 15f;
    public float damage = 50f;
    private string enemyTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            List<Transform> enemyList = new(TDManager.instance.enemies);

            foreach (Transform enemy in enemyList)
            {
                if (enemy != null && Vector3.Distance(transform.position, enemy.position) <= range)
                {
                    enemy.GetComponent<IDamageable>().TakeDamage(damage);
                }
            }
            
            CoolnessScaleController.Instance.AddCoolness(40);
            
            SoundManager soundManager = SoundManager.Instance;
            soundManager.PlaySound(soundManager.audioClipRefsSo.Bomb, Camera.main.transform.position);

            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnDestroy()
    {
        TDManager.instance.mines.Remove(transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
