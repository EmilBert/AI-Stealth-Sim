using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;


public class TaskHide : Node
{
    private NavMeshAgent _agent;
    private LayerMask _mask;
    private Transform _transform;
    private DetectionStatus _status;
    private Collider[] colliders = new Collider[10];
    public TaskHide(Transform transform, LayerMask mask, NavMeshAgent agent, DetectionStatus status)
    {
        _mask = mask;
        _transform = transform;
        _agent = agent;
        _status = status;
    }

    public override NodeState Evaluate()
    {
        int hits = Physics.OverlapSphereNonAlloc(_transform.position, 2.0f, colliders, _mask); // Hur många obstacles finns i närheten?

        float thereshold = -0.5f * _status.GetDetections().Count; // Hur bra behöver ett gömställe vara?

        for (int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 8f, _agent.areaMask)) // Hur långt från obstacles mitt vill vi gömma oss?
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, _agent.areaMask))
                {
                    // Debug.Log("No edges?");
                    continue;
                }
                float normalSum = 0;
                foreach (GameObject chaser in _status.GetDetections())
                { // Summerar hur bra ett gömställe är, kanske ändra till att bara respektera en chaser.
                    normalSum += Vector3.Dot(hit.normal, (chaser.transform.position - hit.position).normalized); 
                }
               
                if (normalSum < thereshold)
                {
                    
                    _agent.SetDestination(hit.position);
                     return NodeState.FAILURE;
                }
            }
        }

        return NodeState.RUNNING;
    }
}
