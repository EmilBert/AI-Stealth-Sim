using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;


public class CheckChased : Node
{
    DetectionStatus _status;
    public CheckChased(DetectionStatus status){
        _status = status;
    }

    public override NodeState Evaluate()
    {
        return _status.GetDetections().Count == 0 ? NodeState.FAILURE : NodeState.SUCCESS;
    }
}
