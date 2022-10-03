using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class PlayerBT : BTree
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform target;
    private NavMeshAgent agent;
    protected override Node SetupTree()
    {
        agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
        //  new Sequence(new List<Node>
        //  {
        //      // Chase
        //      new CheckChased(),
        //      new TaskHide(),
        //  }),
            new TaskGoToObjective(target, agent),
            new TaskGoToObjective(target, agent),
        });

        return root;
    }
}
