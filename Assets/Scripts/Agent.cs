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
        StartCoroutine(updateAnimState());
    }

    IEnumerator updateAnimState(){
        animator.SetBool("moving", 
            goal ? (new Vector2(goal.position.x - transform.position.x, goal.position.z - transform.position.z).sqrMagnitude) > 0.5f : false);
        yield return new WaitForSeconds(.1f);
    }

}
