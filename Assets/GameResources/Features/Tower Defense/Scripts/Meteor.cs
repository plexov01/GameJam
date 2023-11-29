using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private SphereCollider col;
    [SerializeField] private float radius;
    private Rigidbody rb;

    private void Awake()
    {
        col.radius = radius;
        col.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.tag);
        rb.velocity = Vector3.zero;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.25f);
        
        SoundManager soundManager = SoundManager.Instance;
			
        soundManager.PlaySound(soundManager.audioClipRefsSo.meteor, Camera.main.transform.position);
        Destroy(objectToDestroy);
    }
}
