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
        Node root = this;
        while (root.parent != null) root = root.parent;
        root.SetData("resetTimer", true);
        Debug.Log("Gotta hide");
        for (int i = 0; i < colliders.Length; i++) colliders[i] = null;
        int hits = Physics.OverlapSphereNonAlloc(_transform.position, 15.0f, colliders, _mask); // Hur många obstacles finns i närheten?
        int hitReductions = 0;

        for (int i = 0; i < hits; i++) {
            if (Vector3.Distance(colliders[i].transform.position, _status.GetDetections()[0].transform.position) < 3f) {
                colliders[i] = null;
                hitReductions++;
            }
        }

        hits -= hitReductions;

        System.Array.Sort(colliders, colliderSorter);

        float thereshold = -0.5f; // Hur bra behöver ett gömställe vara?

        for (int i = 0; i < hits; i++)
        //for (int i = hits - 1; i >= 0; i--)
        {
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 1f, _agent.areaMask)) // Hur långt från obstacles mitt vill vi gömma oss?
            {
                if (Vector3.Dot(_transform.localRotation * Vector3.forward, (hit.position - _transform.position).normalized) < 0.1f) continue;
                if (!NavMesh.FindClosestEdge(hit.position, out hit, _agent.areaMask))
                {
                    // Debug.Log("No edges?");
                    continue;
                }
                // Summerar hur bra ett gömställe är, kanske ändra till att bara respektera en chaser.
               
                if (Vector3.Dot(hit.normal, (_status.GetDetections()[0].transform.position - hit.position).normalized) < thereshold)
                {
                    //Debug.Log("Collider: " + colliders[i] + ", Normal: " + hit.normal);
                    Debug.DrawLine(_transform.position, hit.position, Color.red, 0.1f);
                    _agent.SetDestination(hit.position);
                    return NodeState.SUCCESS;
                }
                else
                {
                    // Since the previous spot wasn't facing "away" enough from teh target, we'll try on the other side of the object
                    if (NavMesh.SamplePosition(colliders[i].transform.position - (_transform.position - hit.position).normalized * 2, out NavMeshHit hit2, 0.5f, _agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit2.position, out hit2, _agent.areaMask))
                        {
                            continue;
                        }

                        if (Vector3.Dot(hit2.normal, (_transform.position - hit2.position).normalized) < thereshold)
                        {
                            //Debug.Log("Collider: " + colliders[i] + ", Normal: " + hit2.normal);
                            Debug.DrawLine(_transform.position, hit2.position, Color.red, 0.1f);
                            _agent.SetDestination(hit2.position);
                            return NodeState.SUCCESS;
                        }
                    }
                }
            }
        }
        //Debug.Log("we got problems");
        return NodeState.FAILURE;
    }

    private int colliderSorter(Collider A, Collider B)
    {
        if (A == null && B != null) return 1;
        else if (A != null && B == null) return -1;
        else if (A == null && B == null) return 0;
        else return Vector3.Distance(_transform.position, A.transform.position).CompareTo(Vector3.Distance(_transform.position, B.transform.position));
    }
}
