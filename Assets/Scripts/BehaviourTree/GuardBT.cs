using System.Collections.Generic;
using BehaviourTree;

public class GuardBT : BTree
{
    public UnityEngine.Transform[] waypoints;

    public Agent agent;
    public static float speed = 1.0f;

    protected override Node SetupTree()
    {
        Node root = new TaskPatrol(transform, waypoints, agent);
        return root;
    }

}