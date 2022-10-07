using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Wait : Node
{
    float time;
    float timeToWait;
    bool timerReached = false;
    NavMeshAgent _agent;

    public Wait(float seconds, NavMeshAgent agent)
    {
        timeToWait = seconds;
        _agent = agent;
    }

    private Node GetRoot(){
        Node root = this;
        while(root.parent != null) root = root.parent;
        return root;
    } 
    public override NodeState Evaluate()
    {
        Debug.Log("time: " + time);
        Debug.Log(time >= timeToWait);
        time += Time.deltaTime;
        
        Node root = GetRoot();
        Debug.Log(root.GetData("resetTimer") != null && (bool)root.GetData("resetTimer"));
        
        if(root.GetData("resetTimer") != null && (bool)root.GetData("resetTimer")){
            time = 0f;
            timerReached = false;
            root.ClearData("resetTimer");
            return NodeState.SUCCESS;
        }

        // TIMER REACHED
        if(time >= timeToWait){
            Debug.Log("Timer reached");
            root.ClearData("resetTimer");
            timerReached = true;
            return NodeState.FAILURE;
        }
        // STILL WAITING
        return NodeState.RUNNING;
    }
}
