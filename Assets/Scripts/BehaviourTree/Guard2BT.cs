using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System.Runtime.CompilerServices;

public class Guard2BT : GuardBT
{
    // Start is called before the first frame update
    public static float rotationSpeed = 0.05f;
    protected Transform guardTransform;
    protected UnityEngine.AI.NavMeshAgent agent;
    protected FieldOfView fov;
    protected UnityEngine.AI.NavMeshObstacle[] obstacles;

    protected override Node SetupTree()
    {
        agent       = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fov         = GetComponent<FieldOfView>();
        guardTransform   = GetComponent<Transform>();
        obstacles   = GetComponentsInChildren<UnityEngine.AI.NavMeshObstacle>();

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
                new CheckSuspicious(fov, guardTransform),
                new TaskInvestigate(agent, fov, obstacles),
            }),
            // Stationary rotation 
            new Selector(new List<Node>
            {
                new Wait(5.0f, agent),
                new LookAround(agent, guardTransform, rotationSpeed, obstacles),
            })
        });
        return root;
    }
}
