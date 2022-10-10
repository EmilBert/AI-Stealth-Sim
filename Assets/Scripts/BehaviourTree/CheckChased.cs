using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;


public class CheckChased : Node
{
    public CheckChased(){

    }

    public override NodeState Evaluate()
    {
        return NodeState.RUNNING;
    }
}
