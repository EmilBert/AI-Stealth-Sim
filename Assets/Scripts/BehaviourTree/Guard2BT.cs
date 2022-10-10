using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System.Runtime.CompilerServices;

public class Guard2BT : GuardBT
{
    // Start is called before the first frame update
    public static float rotationSpeed = 0.05f;
    public static float turnAngle = 90f;
    protected Vector3 originalPosition;
    protected override Node SetupTree()
    {
        agent           = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fov             = GetComponent<FieldOfView>();
        guardTransform  = GetComponent<Transform>();
        obstacles       = GetComponentsInChildren<UnityEngine.AI.NavMeshObstacle>();
        originalPosition= guardTransform.position;

        obstacles[0].gameObject.transform.Rotate(Vector3.up, fov.viewAngle / 2 * 0.7f);
        obstacles[2].gameObject.transform.Rotate(Vector3.up, -fov.viewAngle / 2 * 0.7f);

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                // Chase
                new CheckAlert(fov),
                new TaskChase(fov,agent, guardTransform, obstacles),
            }),
            new Sequence(new List<Node>
            {
                // Investigate
                new CheckSuspicious(fov, guardTransform, agent, obstacles),
                new TaskInvestigate(agent, fov, obstacles),
            }),
            // Return to position
            new TaskReturnToPosition(agent, originalPosition, guardTransform),
            
            new Selector(new List<Node>
            {
                // Stationary rotation 
                new Wait(5.0f, agent),
                new LookAround(agent, guardTransform, rotationSpeed, obstacles),
            })
        });
        return root;
    }
}
