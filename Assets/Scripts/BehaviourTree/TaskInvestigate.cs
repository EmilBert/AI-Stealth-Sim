using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class TaskInvestigate : Node
{
    private int     _currentWaypointIndex = 0;
    private float   _waitTime = 1f; // in seconds
    private float   _waitCounter = 0f;
    private bool    _waiting = false;

    private Transform           _target;
    private NavMeshAgent        _agent;
    private FieldOfView         _fov;
    private Vector3             _pos;
    private NavMeshObstacle[]   _obstacles;

    public TaskInvestigate(NavMeshAgent agent, FieldOfView fov, NavMeshObstacle[] obstacles)
    {
        _fov = fov;
        _agent = agent;
        _obstacles = obstacles;
    }

    public override NodeState Evaluate()
    {
        foreach (NavMeshObstacle _obstacle in _obstacles)
        {
            _obstacle.enabled = false;
        }
        _agent.enabled = true;
        _target = _fov.GetCurrentTarget();
        _pos = _fov.GetLastSeenPosition();

        _fov.GetCurrentTarget().gameObject.GetComponent<DetectionStatus>().SetDetected(true);
        _agent.SetDestination(_pos);
        Debug.Log(_agent.destination);
        Debug.Log(_agent.remainingDistance);
            
        if(_agent.remainingDistance <= 0.2f){
            Debug.Log("At Target");
            Node root = this;
            while (root.parent != null) root = root.parent;
            root.SetData("timer", 0f);
            return NodeState.FAILURE;
        }
        Debug.Log("Moving towards target");
        return NodeState.RUNNING;
        
    }

}