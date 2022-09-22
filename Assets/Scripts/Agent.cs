using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MoveTo.cs
using UnityEngine.AI;

public class Agent : MonoBehaviour
{   
    public Vector3 goal;
    private Animator animator;
    private NavMeshAgent agent;
    private readonly float[] speeds = {0.0f, 3.5f};
    
    void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(updateAnimState());
    }

    public void GiveGoal(Vector3 newGoal, int urgency){
        goal = newGoal;
        agent.destination = newGoal;
        if(urgency >= 0 && urgency < speeds.Length) agent.speed = speeds[urgency];
        else agent.speed = speeds[0];
    }

    IEnumerator updateAnimState(){
        while(true){
            //Default: No target available, should not move.
            bool shouldMove = false;
            if(goal != null) {
                shouldMove = new Vector2(goal.x - transform.position.x, goal.z - transform.position.z).sqrMagnitude > 0.5f;
            }
            //TODO: If we have several agent speeds, probably change this to int or float
            animator.SetBool("moving", shouldMove);
            yield return new WaitForSeconds(.1f);
        }
    }

}
