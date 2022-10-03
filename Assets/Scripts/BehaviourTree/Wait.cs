using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Wait : Node
{
    float waitTime = 0f;
    bool timerReached = false;
    NavMeshAgent _agent;

    public Wait(float seconds, NavMeshAgent agent)
    {
        Debug.Log("Wait start");
        timerReached = false;
        waitTime = seconds;
        _agent = agent;
    }
    
    public override NodeState Evaluate()
    {
        Node root = this;
        while (root.parent != null) root = root.parent;
        float timer = (float)root.GetData("timer");
        if (!timerReached) 
        {
            timer += Time.deltaTime;
        }
        else
        {
            timerReached = false;
            return NodeState.SUCCESS;
        }

        if (!timerReached && timer > waitTime)
        {
            Debug.Log("Wait over");
            timerReached = true;
            _agent.isStopped = false;
            return NodeState.FAILURE;
        }else{
            _agent.isStopped = true;
            root.SetData("timer", timer);
            return NodeState.RUNNING;
        }
    }

}