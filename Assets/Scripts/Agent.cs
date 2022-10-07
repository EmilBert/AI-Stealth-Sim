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
    private NavMeshObstacle[] obstacles;
    private FieldOfView fov;

    void Start () {
        lastPos = transform.position;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacles = GetComponentsInChildren<NavMeshObstacle>();
        fov = GetComponent<FieldOfView>();
        StartCoroutine(UpdateAnimState());
        StartCoroutine(UpdateObstacles());
    }

    // public void GiveGoal(Vector3 newGoal, int urgency){
    //     //TODO: Remove all references to this
    //     goal = newGoal;
    //     agent.destination = newGoal;
    //     if(urgency >= 0 && urgency < speeds.Length) agent.speed = speeds[urgency];
    //     else agent.speed = speeds[0];
    // }

    IEnumerator UpdateAnimState(){
        while(true){
            //Default: No target available, should not move.
            bool shouldMove;
            if (agent.enabled && agent.speed > 0.1f) shouldMove = true;
            else shouldMove = (lastPos - transform.position).sqrMagnitude > 0.1;
            //TODO: If we have several agent speeds, probably change this to int or float
            animator.SetBool("moving", shouldMove);
            lastPos = transform.position;
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator UpdateObstacles()
    {
        while (true)
        {
            foreach (NavMeshObstacle obstacle in obstacles)
            {
                if (obstacle.enabled)
                {
                    RaycastHit hit;
                    Vector3 lookDir = transform.forward;
                    Vector3 betterLookDir = new Vector3(lookDir.x, 0, lookDir.z);
                    betterLookDir = obstacle.transform.localRotation * betterLookDir;
                    if (Physics.Raycast(transform.position, betterLookDir, out hit, fov.viewRadius, fov.obstacleMask))
                    {
                        obstacle.center = new Vector3(0, 0, hit.distance / 2);
                        obstacle.size = new Vector3(hit.distance / 2.5f, 0.5f, hit.distance);
                        Debug.DrawRay(transform.position, hit.point - transform.position, Color.red, 0.1f);
                    }
                    else
                    {
                        obstacle.center = new Vector3(0, 0, fov.viewRadius / 2);
                        obstacle.size = new Vector3(fov.viewRadius / 2.5f, 0.5f, fov.viewRadius);
                    }
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

}
