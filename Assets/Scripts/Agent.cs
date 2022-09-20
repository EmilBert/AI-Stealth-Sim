using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MoveTo.cs
using UnityEngine.AI;

public class Agent : MonoBehaviour
{   
    public Transform goal;
    public Animator animator;
    private NavMeshAgent agent;
    
    void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position; 
    }

    private void Update()
    {
        animator.SetBool("moving", Vector3.Distance(goal.position, transform.position) > 0.1f);
    }

}
