using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskGoToObjective : Node
{
    private NavMeshAgent _agent;
    private Transform _target;

    public TaskGoToObjective(Transform target, NavMeshAgent agent) {
        _agent = agent;
        _target = target;
        _agent.SetDestination(_target.position);
    }

    public override NodeState Evaluate()
    {
        state = NodeState.RUNNING;
        return state;
    }
}
