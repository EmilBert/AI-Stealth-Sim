using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class LookAround : Node
{
    private UnityEngine.AI.NavMeshAgent _agent;
    private float _rotationSpeed;
    private Transform _transform;


    public LookAround(UnityEngine.AI.NavMeshAgent agent, Transform transform, float rotationSpeed)
    {
        _agent = agent;
        _rotationSpeed = rotationSpeed;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        //TODO: change from RotateAround to LookAt or add a wait function(the wait node)?
        _transform.RotateAround(_agent.transform.position, Vector3.up, 45 * _rotationSpeed);

        return NodeState.RUNNING;
    }
}