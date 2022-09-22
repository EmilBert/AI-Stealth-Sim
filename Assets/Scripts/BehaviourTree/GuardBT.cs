using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BehaviourTree;

public class GuardBT : BTree
{
    public Transform[] waypoints;

    public NavMeshAgent agent;
    public static float speed = 3.5f;

    protected override Node SetupTree()
    {
        agent = GetComponent<NavMeshAgent>();
        Node root = new TaskPatrol(transform, waypoints, agent);
        return root;
    }

}