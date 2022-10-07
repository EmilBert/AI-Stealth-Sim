using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BehaviourTree;

public class GuardBT : BTree
{
    public Transform[] waypoints;
    public static float speed = 3.5f;
    
    protected Transform guardTransform;
    protected NavMeshAgent agent;
    protected FieldOfView fov;
    private NavMeshObstacle[] obstacles;

    protected override Node SetupTree()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        guardTransform = GetComponent<Transform>();
        obstacles = GetComponentsInChildren<NavMeshObstacle>();

        obstacles[0].gameObject.transform.Rotate(Vector3.up, fov.viewAngle / 2 * 0.7f);
        obstacles[2].gameObject.transform.Rotate(Vector3.up, -fov.viewAngle / 2 * 0.7f);

        //Node root = new TaskPatrol(waypoints, transform, agent);
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                // Chase
                new CheckAlert(fov),
                new TaskChase(fov, agent, guardTransform, obstacles),
            }),
            new Sequence(new List<Node>
            {
                // Investigate
                new CheckSuspicious(fov, guardTransform),
                new Selector(new List<Node>
                {
                    new Wait(5.0f, agent),
                    new TaskInvestigate(agent, fov, obstacles),
                })
            }),
            // Patrol
            new TaskPatrol(waypoints, guardTransform, agent, obstacles),
        });

        root.SetData("timer", 0.0f);

        return root;
    }
}