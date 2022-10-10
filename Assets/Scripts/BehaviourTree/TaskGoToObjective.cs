using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskGoToObjective : Node
{
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
        Debug.Log(_objectives.Count);
        if((_playerTransform.position - _objectives[_currentObjective].position).sqrMagnitude < 2f && _objectives.Count > 1)
        {
            Debug.Log("Objective reached");
            _objectives.RemoveAt(_currentObjective);
            _currentObjective = Random.Range(0, _objectives.Count);
            _agent.SetDestination(_objectives[_currentObjective].position);
        }
        return NodeState.RUNNING;
    }

}
