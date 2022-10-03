using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class TaskInvestigate : Node
{
    private int     _currentWaypointIndex = 0;
    private float   _waitTime = 1f; // in seconds
    private float   _waitCounter = 0f;
    private bool    _waiting = false;

    private Transform       _target;
    private NavMeshAgent    _agent;
    private FieldOfView     _fov;


    public TaskInvestigate(NavMeshAgent agent, FieldOfView fov)
    {
        _fov = fov;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        _target = _fov.GetCurrentTarget();

        if(_target != null){
            _agent.SetDestination(_target.position);
            return NodeState.SUCCESS;
        }    
        
        return NodeState.FAILURE;
    }

}