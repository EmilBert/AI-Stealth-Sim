using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskGoToObjective : Node
{
    private NavMeshAgent _agent;
    private Transform _target;
    private Transform _playerTransform;
    float timer = 0f;
    readonly float pathUpdate = 0.1f;

    public TaskGoToObjective(Transform target, NavMeshAgent agent, Transform playerTransform) {
        _agent = agent;
        _target = target;
        _playerTransform = playerTransform;
        _agent.SetDestination(_target.position);
    }

    public override NodeState Evaluate()
    {
        if (!_agent.isOnNavMesh){
            NavMeshHit hit = new NavMeshHit();
            _agent.FindClosestEdge(out hit);

            _agent.enabled = false;
            _playerTransform.LookAt(hit.position);
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, hit.position, 
                Mathf.Min(GuardBT.speed * Time.deltaTime, new Vector2(_playerTransform.position.x - hit.position.x, _playerTransform.position.z - hit.position.z).magnitude));
        }
        else _agent.enabled = true;
        return NodeState.RUNNING;
    }

}
