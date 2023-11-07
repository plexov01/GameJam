using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private SphereCollider col;
    [SerializeField] private float radius;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col.radius = radius;
        col.enabled = false;
    }

    void Update()
    {
        if (rb.velocity.y == 0 && transform.position.y < 9f)
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.25f);
        
        SoundManager soundManager = SoundManager.Instance;
			
        soundManager.PlaySound(soundManager.audioClipRefsSo.meteor, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
