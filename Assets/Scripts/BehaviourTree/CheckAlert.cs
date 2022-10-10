using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckAlert : Node
{
    private FieldOfView _fov;
    private GuardStates _state;
    private NodeState   _nodeState;

    public CheckAlert(FieldOfView fov)
    {
        _fov = fov;
    }

    public override NodeState Evaluate()
    {
        _state = _fov.FindVisibleTargets();

        if( _state == GuardStates.ALERTED)
        {
            _fov.GetCurrentTarget().gameObject.GetComponent<DetectionStatus>().SetDetected(true, _fov.gameObject);
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