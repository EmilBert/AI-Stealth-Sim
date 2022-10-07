using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System.Runtime.CompilerServices;

public class Guard2BT : GuardBT
{
    // Start is called before the first frame update
    public static float rotationSpeed = 0.05f;
    
    protected Transform transform;
    protected UnityEngine.AI.NavMeshAgent agent;
    protected FieldOfView fov;

    protected override Node SetupTree()
    {
        agent       = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fov         = GetComponent<FieldOfView>();
        transform   = GetComponent<Transform>();

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
                new CheckSuspicious(fov, transform, agent),
                new TaskInvestigate(agent, fov),
            }),
            // Stationary rotation 
            new Sequence(new List<Node>
            {
                new Wait(5.0f, agent),
                new LookAround(agent, transform, rotationSpeed),
            })
        });
        return root;
    }
}
