using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MoveTo.cs
using UnityEngine.AI;

public class Agent : MonoBehaviour
{   
    public Vector3 lastPos;
    private Animator animator;
    private NavMeshAgent agent;
    
    void Start () {
        lastPos = transform.position;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(updateAnimState());
    }

    // public void GiveGoal(Vector3 newGoal, int urgency){
    //     //TODO: Remove all references to this
    //     goal = newGoal;
    //     agent.destination = newGoal;
    //     if(urgency >= 0 && urgency < speeds.Length) agent.speed = speeds[urgency];
    //     else agent.speed = speeds[0];
    // }

    IEnumerator updateAnimState(){
        while(true){
            //Default: No target available, should not move.
            bool shouldMove;
            if (agent.enabled && agent.speed > 0.1f) shouldMove = true;
            else shouldMove = (lastPos - transform.position).sqrMagnitude > 0.1;
            //TODO: If we have several agent speeds, probably change this to int or float
            animator.SetBool("moving", shouldMove);
            Debug.Log(lastPos - transform.position);
            lastPos = transform.position;
            yield return new WaitForSeconds(.1f);
        }
    }

}
