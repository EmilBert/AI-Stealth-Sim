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
        StartCoroutine(updateAnimState());
    }

    public void GiveGoal(Transform trans){
        goal = trans;
        agent.destination = trans.position;
    }

    IEnumerator updateAnimState(){
        while(true){
            animator.SetBool("moving", 
                goal != null ? (new Vector2(goal.position.x - transform.position.x, goal.position.z - transform.position.z).sqrMagnitude) > 0.5f : false);
            Debug.Log(goal == null);
            yield return new WaitForSeconds(.1f);
        }
    }

}
