using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class CheckSuspicious : Node
{
    private FieldOfView _fov;
    private GuardStates _state;
    private NodeState _nodeState;
    private Transform _transform;
    private NavMeshAgent _agent;
    private NavMeshObstacle[] _obstacles;

    public CheckSuspicious(FieldOfView fov, Transform transform, NavMeshAgent agent, NavMeshObstacle[] obstacles)
    {
        _fov = fov;
        _transform = transform;
        _agent = agent;
        _obstacles = obstacles;
    }

    public override NodeState Evaluate()
    {
        _state = _fov.FindSuspiciousTargets();

        if(_state == GuardStates.SUSPICIOUS)
        {
            _fov.GetCurrentTarget().gameObject.GetComponent<DetectionStatus>().SetDetected(true, _fov.gameObject);
            var lookDir = _fov.GetLastSeenPosition() - _transform.position;
            lookDir = new Vector3(lookDir.x, 0, lookDir.z);
            _transform.rotation = Quaternion.LookRotation(lookDir);

            foreach (NavMeshObstacle _obstacle in _obstacles)
            {
                _obstacle.enabled = false;
            }
            _agent.enabled = true;
            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            if (_fov.GetCurrentTarget() != null) _fov.GetCurrentTarget().gameObject.GetComponent<DetectionStatus>().SetDetected(false, _fov.gameObject);

            _nodeState = NodeState.FAILURE;
        }
        return _nodeState;
    }
}