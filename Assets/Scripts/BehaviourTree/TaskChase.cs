using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskChase : Node
{
    private Transform _transform;
    private FieldOfView _fov;
    private UnityEngine.AI.NavMeshAgent _agent;

    public TaskChase(FieldOfView fov, UnityEngine.AI.NavMeshAgent agent, Transform transform)
    {
        _transform  = transform;
        _fov        = fov;
        _agent      = agent;
    }

    public override NodeState Evaluate()
    {
        if(_fov.GetState() == GuardStates.ALERTED)
        {
            _agent.enabled = true;
            var lookDir = _fov.GetCurrentTarget().position - _transform.position;
            lookDir     = new Vector3(lookDir.x, 0, lookDir.z);
            _transform.rotation = Quaternion.LookRotation(lookDir);

            Debug.Log("Chasing");
            _agent.speed = 5f;
            _agent.SetDestination(_fov.GetCurrentTarget().position);
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }   
}
