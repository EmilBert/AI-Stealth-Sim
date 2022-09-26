using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BehaviourTree;

public class GuardBT : BTree
{
    public Transform[] waypoints;
    public static float speed = 3.5f;
    
    private Transform transform;
    private NavMeshAgent agent;
    private FieldOfView fov;

    protected override Node SetupTree()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        transform = GetComponent<Transform>();

        Node root = new TaskPatrol(waypoints, transform, agent);

        // Node root = new Selector(new List<Node>
        // {
        //     new Sequence(new List<Node>
        //     {
        //         // Chase
        //         new CheckAlert(),
        //         new TaskChase(),
        //     }),
        //     new Sequence(new List<Node>
        //     {
        //         // Investigate
        //         new CheckSuspicious(),
        //         new TaskInvestigate(),
        //     }),
        //     // Patrol
        //     new TaskPatrol(waypoints, transform, agent),
        // });

        return root;
    }
}