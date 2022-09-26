using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class Investigate : Node
{
    private int     _currentWaypointIndex = 0;
    private float   _waitTime = 1f; // in seconds
    private float   _waitCounter = 0f;
    private bool    _waiting = false;

    private Transform       _target;
    private NavMeshAgent    _agent;
    private FieldOfView     _fov;


    public Investigate(Transform target, NavMeshAgent agent, FieldOfView fov)
    {
        _target = target; 
        _agent.SetDestination(_target.position);
        _fov = fov;
    }

    public override NodeState Evaluate()
    {
        return state = NodeState.SUCCESS;
    }

}