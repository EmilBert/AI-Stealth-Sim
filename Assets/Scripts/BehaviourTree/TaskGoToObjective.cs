using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskGoToObjective : Node
{
    private NavMeshAgent _agent;
    private Transform _target;
    float timer = 0f;
    readonly float pathUpdate = 0.1f;

    public TaskGoToObjective(Transform target, NavMeshAgent agent) {
        _agent = agent;
        _target = target;
        _agent.SetDestination(_target.position);
    }

    public override NodeState Evaluate()
    {
        if (timer >= pathUpdate) 
        {
            timer = 0f;
            _agent.autoRepath = true;
        }
        else
        {
            timer += Time.deltaTime;
            _agent.autoRepath = false;
        }
        return NodeState.RUNNING;
    }

}
