using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public float baseHealth;
    public float currentHealth;

    [SerializeField] private GameObject objectToDestroy;
    public GameObject ice;

    private string wallTag = "Wall";
    private string mainBaseTag = "MainBase";

    private void Start()
    {
        currentHealth = baseHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && objectToDestroy != null)
        {
            Death();
        }
    }

    public void Death()
    {
        TDManager.instance.walls.Remove(transform);

        if (transform.CompareTag(wallTag))
        {
            CoolnessScaleController.Instance.AddCoolness(40);
        }
        else if (transform.CompareTag(mainBaseTag))
        {
            CoolnessScaleController.Instance.AddCoolness(200);
            print("gates destoyed");
        }

        Destroy(objectToDestroy);
    }
}
