using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System.Runtime.CompilerServices;

public class Guard2BT : GuardBT
{
    // Start is called before the first frame update
    private float rotationSpeed = 0.02f;
    protected override Node SetupTree()
    {
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        transform = GetComponent<Transform>();
        this.speed = 4.5f;
        
        
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
            // Stationary rotation 
            new LookAround(agent, transform, rotationSpeed),
        });

        return root;
    }
}
