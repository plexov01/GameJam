using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    [SerializeField] private GameObject brokenTowerNode;
    private string towerNodeTag = "TowerNode";
    private string brokenTowerNodeTag = "BrokenTowerNode";
    private string towerTag = "Tower";

    public float damage = 100f;

    private void Update()
    {
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(towerNodeTag) && other.transform.childCount == 0)
        {
            GetComponent<SphereCollider>().enabled = false;
            print("hit node");
            other.transform.tag = brokenTowerNodeTag;
            other.transform.GetComponent<MeshRenderer>().enabled = false;
            Instantiate(brokenTowerNode, other.transform);
            print("cool animation");
            Destroy(gameObject);
        }
        else if (other.CompareTag(towerTag))
        {
            print("hit tower");
            other.transform.parent.GetComponentInChildren<IDamageable>().TakeDamage(damage);
            print("cool animation");
            Destroy(gameObject);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        print("cool animation");
        Destroy(gameObject);
    }*/
}
