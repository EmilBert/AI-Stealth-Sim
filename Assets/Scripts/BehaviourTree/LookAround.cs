using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class LookAround : Node
{
    private UnityEngine.AI.NavMeshAgent _agent;
    private float _rotationSpeed;
    private Transform _transform;
    private UnityEngine.AI.NavMeshObstacle[]   _obstacles;

    public LookAround(UnityEngine.AI.NavMeshAgent agent, Transform transform, float rotationSpeed,  UnityEngine.AI.NavMeshObstacle[] obstacles)
    {
        _agent = agent;
        _rotationSpeed = rotationSpeed;
        _transform = transform;
        _obstacles = obstacles;
    }

    public override NodeState Evaluate()
    {
        _agent.enabled = false;
        foreach (UnityEngine.AI.NavMeshObstacle _obstacle in _obstacles)
        {
            _obstacle.enabled = true;
        }

        //TODO: change from RotateAround to LookAt or add a wait function(the wait node)?
        _transform.RotateAround(_agent.transform.position, Vector3.up, 45);
        Node root = this;
        while(root.parent != null) root = root.parent;
        root.SetData("resetTimer", true);
        return NodeState.SUCCESS;
    }
}