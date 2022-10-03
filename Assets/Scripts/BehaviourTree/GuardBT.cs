using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BehaviourTree;

public class GuardBT : BTree
{
    public Transform[] waypoints;
    protected float speed = 3.5f;
    
    protected Transform transform;
    protected NavMeshAgent agent;
    protected FieldOfView fov;

    protected override Node SetupTree()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        transform = GetComponent<Transform>();

        //Node root = new TaskPatrol(waypoints, transform, agent);

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                // Chase
                new CheckAlert(fov),
                new TaskInvestigate(agent, fov),
            }),
            new Sequence(new List<Node>
            {
                // Investigate
                new CheckSuspicious(fov),
                new TaskInvestigate(agent, fov),
            }),
            // Patrol
            new TaskPatrol(waypoints, transform, agent, speed),
        });

        return root;
    }
}