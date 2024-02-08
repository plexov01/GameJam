using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    [Header("Wall")]
    [SerializeField] private GameObject objectToDestroy;

    [Header("Health")]
    public float baseHealth;
    public float currentHealth;
    public float iceHealth;

    [Header("Freeze")]
    [SerializeField] private GameObject ice;
    [ReadOnlyInspector] public bool isFrozen = false;
    private Coroutine freezeCoroutine = null;

    private string wallTag;
    private string mainBaseTag;

    private void Start()
    {
        currentHealth = baseHealth;
        wallTag = TDManager.instance.tags.wall;
        mainBaseTag = TDManager.instance.tags.mainBase;
    }

    public void Freeze(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            freezeCoroutine = null;
        }

        freezeCoroutine = StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        ice.SetActive(true);
        isFrozen = true;
        iceHealth = baseHealth;

        yield return new WaitForSeconds(duration);

        ice.SetActive(false);
        isFrozen = false;
        iceHealth = 0;

        freezeCoroutine = null;
    }

    public void TakeDamage(float damage)
    {
        if (isFrozen)
        {
            iceHealth -= damage;

            if (iceHealth <= 0)
            {
                currentHealth += iceHealth;

                if (currentHealth <= 0 && objectToDestroy != null)
                {
                    Death();
                }

                iceHealth = 0;
                isFrozen = false;
                ice.SetActive(false);
                StopCoroutine(freezeCoroutine);
                freezeCoroutine = null;
            }
        }
        else
        {
            currentHealth -= damage;

            if (currentHealth <= 0 && objectToDestroy != null)
            {
                Death();
            }
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
