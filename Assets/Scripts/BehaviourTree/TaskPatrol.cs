using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class TaskPatrol : Node
{
    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    private Transform _transform;
    private Transform[] _waypoints;
    private NavMeshAgent _agent;

    public TaskPatrol(Transform transform, Transform[] waypoints, NavMeshAgent agent)
    {
        _transform = transform;
        _waypoints = waypoints;
        _agent = agent;
        _agent.SetDestination(waypoints[0].position);
    }

        public override NodeState Evaluate()
    {
        Transform wp = _waypoints[_currentWaypointIndex];
        if (new Vector2(_transform.position.x - wp.position.x, _transform.position.z - wp.position.z).sqrMagnitude < 0.01f)
        {
            if(_waiting) {
                _agent.speed = 0.0f;
                //At waypoint, waiting.
                //Wait for the wait timer to finish.
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _waiting = false;
                }
            }
            else {
                //At waypoint, not waiting.
                //Set new waypoint and walk towards it.
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;
                _agent.speed = GuardBT.speed;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
                Debug.Log("New Destination: " + _waypoints[_currentWaypointIndex].position);
            }
        }
    
        state = NodeState.RUNNING;
        return state;
    }

}