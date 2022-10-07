using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskChase : Node
{
    private Transform _transform;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private NavMeshObstacle[] _obstacles;

    public TaskChase(FieldOfView fov, NavMeshAgent agent, Transform transform, NavMeshObstacle[] obstacles)
    {
        _transform  = transform;
        _fov        = fov;
        _agent      = agent;
        _obstacles = obstacles;
    }

    public override NodeState Evaluate()
    {
        if(_fov.GetState() == GuardStates.ALERTED)
        {
            foreach (NavMeshObstacle _obstacle in _obstacles)
            {
                _obstacle.enabled = false;
            }
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
