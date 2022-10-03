using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckSuspicious : Node
{
    private FieldOfView _fov;
    private GuardStates _state;
    private NodeState _nodeState;
    private Transform _transform;
    private UnityEngine.AI.NavMeshAgent _agent;

    public CheckSuspicious(FieldOfView fov, Transform transform, UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
        _fov = fov;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        _state = _fov.FindSuspiciousTargets();

        if( _state == GuardStates.SUSPICIOUS)
        {
            _agent.enabled = true;
            var lookDir = _fov.GetLastSeenPosition() - _transform.position;
            lookDir = new Vector3(lookDir.x, 0, lookDir.z);
            _transform.rotation = Quaternion.LookRotation(lookDir);
            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            _agent.enabled = false;
            _nodeState = NodeState.FAILURE;
        }
        return _nodeState;
    }
}