using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform destination;
    public float baseSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        destination = EnemyManager.instance.end;
        agent.SetDestination(destination.position);
        baseSpeed = agent.speed;
    }
}
