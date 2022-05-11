using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : BaseState
{
    private NavMeshAgent navMeshAgent;
    private bool isAgentExists = false;

    protected override void Awake()
    {
        base.Awake();
        isAgentExists = TryGetComponent<NavMeshAgent>(out navMeshAgent);
        if(!isAgentExists)
        {
            Debug.LogError($"NavMeshAgent not set to the WalkState of {name} enemy");
        }
    }

    protected void Update()
    {
        if (isAgentExists && navMeshAgent.isOnNavMesh)
        {
            SetNewDestination();
        }
    }

    protected override void OnDisable()
    {
        if(isAgentExists && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
        base.OnDisable();
    }

    private void SetNewDestination()
    {
        var newDestination = ShrineController.Instance.shrineCollider.ClosestPoint(transform.position);
        navMeshAgent.SetDestination(newDestination);
    }

    private void OnEnable()
    {
        if (isAgentExists)
        {
            navMeshAgent.isStopped = false;
        }
        IsCompleted = true;
    }
}
