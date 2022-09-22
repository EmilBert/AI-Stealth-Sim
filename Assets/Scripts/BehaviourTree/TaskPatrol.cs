using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TaskPatrol : Node
{
    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    private Transform _transform;
    private Transform[] _waypoints;
    private Agent _agent;

    public TaskPatrol(Transform transform, Transform[] waypoints, Agent agent)
    {
        _transform = transform;
        _waypoints = waypoints;
        _agent = agent;
        _agent.GiveGoal(waypoints[0].position, 1);
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                //_transform.position = wp.position;
                Debug.Log(wp.position);
                _agent.GiveGoal(wp.position, 1);
                _waitCounter = 0f;
                _waiting = true;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
        }
    
        state = NodeState.RUNNING;
        return state;
    }

}