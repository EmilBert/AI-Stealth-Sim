using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;


public class TaskHide : Node
{
    public TaskHide(){
        
    }

    public override NodeState Evaluate()
    {
        return NodeState.RUNNING;
    }
}
