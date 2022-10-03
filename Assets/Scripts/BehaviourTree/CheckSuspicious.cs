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

    public CheckSuspicious(FieldOfView fov)
    {
        _fov = fov;
    }

    public override NodeState Evaluate()
    {
        _state = _fov.FindVisibleTargets();

        if( _state == GuardStates.SUSPICIOUS)
        {
            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            _nodeState = NodeState.FAILURE;
        }
        return _nodeState;
    }
}