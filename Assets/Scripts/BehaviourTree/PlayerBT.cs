using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class PlayerBT : BTree
{
    // Start is called before the first frame update
    [SerializeField]
    public List<Transform>  objectives;
    [SerializeField]
    public LayerMask        hideableObjects;
    private Transform       playerTransform;
    private NavMeshAgent    agent;
    public DetectionStatus  detectionStatus;

    protected override Node SetupTree()
    {
        agent           = GetComponent<NavMeshAgent>();
        playerTransform = GetComponent<Transform>();

        Node root = new Selector(new List<Node>
        {
        new Sequence(new List<Node>
        {
            // Chase
            new CheckChased(detectionStatus),
            new Selector(new List<Node>{
                new TaskHide(playerTransform, hideableObjects, agent, detectionStatus),
                new Wait(1f, agent),
            }),
        }),
        new TaskGoToObjective(objectives, agent, playerTransform),
        
        });

        return root;
    }
}
