using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskGoToObjective : Node
{
    private float stuckCheckTimer = 0f;
    private float stuckCheckInterval = 1f;
    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private int _currentObjective;
    private List<Transform> _objectives;

    public TaskGoToObjective(List<Transform> objectives, NavMeshAgent agent, Transform playerTransform) {
        _agent = agent;
        _objectives = objectives;
        _playerTransform = playerTransform;

        _currentObjective = Random.Range(0, _objectives.Count);
        _agent.SetDestination(_objectives[_currentObjective].position);
    }

    public override NodeState Evaluate()
    {
        if (!_agent.isOnNavMesh)
        {
            NavMesh.FindClosestEdge(_playerTransform.position, out NavMeshHit hit, _agent.areaMask);
            _agent.Warp(hit.position);
        }
        if(_agent.destination != _objectives[_currentObjective].position)
        {
            _agent.SetDestination(_objectives[_currentObjective].position);
        }
        Debug.Log((_playerTransform.position - _objectives[_currentObjective].position).sqrMagnitude);
        if((_playerTransform.position - _objectives[_currentObjective].position).sqrMagnitude < 2f && _objectives.Count > 1)
        {
            Debug.Log("Objective reached");
            _objectives[_currentObjective].gameObject.SetActive(false);
            _objectives.RemoveAt(_currentObjective);
            _currentObjective = Random.Range(0, _objectives.Count);
            _agent.SetDestination(_objectives[_currentObjective].position);
        }
        if(stuckCheckTimer > stuckCheckInterval)
        {
            if (!_agent.hasPath && _agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Character stuck");
                _agent.enabled = false;
                _agent.enabled = true;
                Debug.Log("navmesh re enabled");
                // navmesh agent will start moving again
            }
            stuckCheckTimer = 0f;
        }
        return NodeState.RUNNING;
    }

}
