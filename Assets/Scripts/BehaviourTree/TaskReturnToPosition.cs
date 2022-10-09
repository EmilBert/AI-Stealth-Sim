using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;
public class TaskReturnToPosition : Node
{
    private UnityEngine.AI.NavMeshAgent _agent;
    private Vector3 _originalPos;
    private Transform _guardTransform;
    public TaskReturnToPosition( UnityEngine.AI.NavMeshAgent agent, Vector3 originalPos, Transform guardTransform)
    {
        _agent = agent;
        _originalPos = originalPos;
        _guardTransform = guardTransform;
    }
    public override NodeState Evaluate()
    {
        if (Vector2.Distance(_originalPos,_guardTransform.position) < 0.000001f && (Vector3.Angle(_guardTransform.forward, (_originalPos - _guardTransform.position)) < 5.0f)){
            return NodeState.FAILURE;
        }
        else{
            if(_agent.destination != _originalPos){
                _agent.SetDestination(_originalPos);
                Debug.Log("New Destination: " + _originalPos);
            }
            return NodeState.RUNNING;
        }
    }
}