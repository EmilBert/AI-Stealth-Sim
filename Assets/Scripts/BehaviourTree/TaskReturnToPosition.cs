using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;
public class TaskReturnToPosition : Node
{
    private NavMeshAgent _agent;
    private Vector3 _originalPos;
    private Quaternion _originalRot;
    private Transform _guardTransform;
    private NavMeshObstacle[] _obstacles;
    private List<Vector3> waypoints;
    public TaskReturnToPosition(NavMeshAgent agent, Transform guardTransform, NavMeshObstacle[] obstacles)
    {
        _agent = agent;
        _originalPos = guardTransform.position;
        _originalRot = guardTransform.rotation;
        _guardTransform = guardTransform;
        _obstacles = obstacles;
    }
    public override NodeState Evaluate()
    {
        if (_agent.enabled && _agent.destination != _originalPos)
        {
            Node root = this;
            while (root.parent != null) root = root.parent;
            root.SetData("currentWaypointIndex", 0);
            _agent.SetDestination(_originalPos);
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(_originalPos, path);
            waypoints = new List<Vector3>(path.corners);
            _agent.enabled = false;

            foreach (NavMeshObstacle _obstacle in _obstacles)
            {
                _obstacle.enabled = false;
            }
            //Debug.Log("New Destination: " + _originalPos);
        }
        if (waypoints == null)
        {
            Debug.Log("1");
            return NodeState.FAILURE;
        }
        else if (waypoints.Count == 0){
            Debug.Log("2");
            waypoints = null;
            _guardTransform.position = _originalPos;
            return NodeState.FAILURE;
        }
        else if (Vector2.Distance(waypoints[0], _guardTransform.position) < 0.000001f)
        {
            Debug.Log("3");
            _guardTransform.position = waypoints[0];
            waypoints.RemoveAt(0);
            if(waypoints.Count == 0)
            {
                _guardTransform.position = _originalPos;
                _guardTransform.rotation = _originalRot;
                return NodeState.SUCCESS;
            }
            else return NodeState.RUNNING;
        }
        else
        {
            Debug.Log("4");
            if ((_guardTransform.position - waypoints[0]).sqrMagnitude > 0.01) _guardTransform.LookAt(waypoints[0]);
            _guardTransform.position = Vector3.MoveTowards(_guardTransform.position, waypoints[0],
                Mathf.Min(GuardBT.speed * Time.deltaTime, new Vector2(_guardTransform.position.x - waypoints[0].x, _guardTransform.position.z - waypoints[0].z).magnitude));
            return NodeState.RUNNING;
        }
    }
}