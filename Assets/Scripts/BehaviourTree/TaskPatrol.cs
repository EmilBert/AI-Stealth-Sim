using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskPatrol : Node
{
    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    private Transform           _transform;
    private Transform[]         _waypoints;
    private NavMeshAgent        _agent;
    private NavMeshObstacle[]   _obstacles;
    private Node root;

    public TaskPatrol(Transform[] waypoints, Transform transform, NavMeshAgent agent, NavMeshObstacle[] obstacles)
    {
        root = this;
        while (root.parent != null) root = root.parent;
        root.SetData("currentWaypointIndex", 0);
        _waypoints = waypoints;
        _transform = transform;
        _agent = agent;
        _obstacles = obstacles;
        // _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    public override NodeState Evaluate()
    {
        _agent.enabled = false;
        foreach (NavMeshObstacle _obstacle in _obstacles)
        {
            _obstacle.enabled = true;
        }
        Transform wp = _waypoints[(int)root.GetData("currentWaypointIndex")];
        wp.position = new Vector3(wp.position.x, 0.0f, wp.position.z);
        if (new Vector2(_transform.position.x - wp.position.x, _transform.position.z - wp.position.z).sqrMagnitude < 0.01f)
        {
            if(_waiting) {
                //_agent.speed = 0.0f;
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
                //_agent.speed = GuardBT.speed;

                root.SetData("currentWaypointIndex", ((int)root.GetData("currentWaypointIndex") + 1) % _waypoints.Length);
                //_agent.SetDestination(_waypoints[_currentWaypointIndex].position);
                //Debug.Log("New Destination: " + _waypoints[_currentWaypointIndex].position);
            }
        }
        else {
            _transform.LookAt(_waypoints[(int)root.GetData("currentWaypointIndex")].position);
            _transform.position = Vector3.MoveTowards(_transform.position, wp.position, 
                Mathf.Min(GuardBT.speed * Time.deltaTime, new Vector2(_transform.position.x - wp.position.x, _transform.position.z - wp.position.z).magnitude));
        }
    
        state = NodeState.RUNNING;
        return state;
    }

}