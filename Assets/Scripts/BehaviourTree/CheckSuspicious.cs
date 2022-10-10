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

    public CheckSuspicious(FieldOfView fov, Transform transform)
    {
        _fov = fov;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        _state = _fov.FindSuspiciousTargets();

        if(_state == GuardStates.SUSPICIOUS)
        {
            _fov.GetCurrentTarget().gameObject.GetComponent<DetectionStatus>().SetDetected(true);
            var lookDir = _fov.GetLastSeenPosition() - _transform.position;
            lookDir = new Vector3(lookDir.x, 0, lookDir.z);
            _transform.rotation = Quaternion.LookRotation(lookDir);
            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            _nodeState = NodeState.FAILURE;
        }
        return _nodeState;
    }
}