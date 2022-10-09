using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;
public class TaskReturnToPosition : Node
{
    private NavMeshAgent _agent;
    private Vector3 _originalPos;
    private Transform _guardTransform;
    private List<Vector3> waypoints;
    public TaskReturnToPosition(NavMeshAgent agent, Vector3 originalPos, Transform guardTransform)
    {
        _agent = agent;
        _originalPos = originalPos;
        _guardTransform = guardTransform;
    }
    public override NodeState Evaluate()
    {
        if (_agent.enabled && _agent.destination != _originalPos)
        {
            _agent.SetDestination(_originalPos);
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(_originalPos, path);
            waypoints = new List<Vector3>(path.corners);
            _agent.enabled = false;
            Debug.Log("New Destination: " + _originalPos);
        }
        if (waypoints == null)
        {
            return NodeState.FAILURE;
        }
        else if (waypoints.Count == 0){
            waypoints = null;
            _guardTransform.position = _originalPos;
            return NodeState.FAILURE;
        }
        else if (Vector2.Distance(waypoints[0], _guardTransform.position) < 0.000001f && (Vector3.Angle(_guardTransform.forward, waypoints[0] - _guardTransform.position) < 5.0f))
        {
            _guardTransform.position = waypoints[0];
            waypoints.RemoveAt(0);
            return NodeState.RUNNING;
        }
        else
        {
            _guardTransform.LookAt(waypoints[0]);
            _guardTransform.position = Vector3.MoveTowards(_guardTransform.position, waypoints[0],
                Mathf.Min(GuardBT.speed * Time.deltaTime, new Vector2(_guardTransform.position.x - waypoints[0].x, _guardTransform.position.z - waypoints[0].z).magnitude));
            return NodeState.RUNNING;
        }
    }
}