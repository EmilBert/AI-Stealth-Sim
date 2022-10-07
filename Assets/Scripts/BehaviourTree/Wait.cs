using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Wait : Node
{
    float timer;
    float waitTime = 0f;
    bool timerReached = false;
    NavMeshAgent _agent;

    public Wait(float seconds, NavMeshAgent agent)
    {
        Debug.Log("Wait start");
        timerReached = false;
        waitTime = seconds;
        _agent = agent;
        timer = 0f;
    }
    
    public override NodeState Evaluate()
    {
        Node root = this;
        while(root.parent != null) root = root.parent;
        
        timer += Time.deltaTime;
        
        if(timer >= waitTime){
            timerReached = true;
            Debug.Log("Wait over");
             //_agent.isStopped = false;
             return NodeState.FAILURE;
        }else
        {
            //_agent.isStopped = true;
            //root.SetData("timer", timer);
            return NodeState.RUNNING;
        } 



        // if (!timerReached && timer > waitTime)
        // {
        //     Debug.Log("Wait over");
        //     timerReached = true;
        //     //_agent.isStopped = false;
        //     return NodeState.FAILURE;
        // }else{
        //     //_agent.isStopped = true;
        //     root.SetData("timer", timer);
        //     return NodeState.RUNNING;
        // }

        // if (!timerReached) 
        // {
        //     timer += Time.deltaTime;
        // }
        // else
        // {
        //     timerReached = false;
        //     return NodeState.SUCCESS;
        // }
    }
}
