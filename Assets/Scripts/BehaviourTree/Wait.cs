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
        Node root = this;
        while(root.parent != null) root = root.parent;
        GetRoot().SetData("resetTimer", false);
    }

    private Node GetRoot(){
        Node root = this;
        while(root.parent != null) root = root.parent;
        return root;
    } 
    public override NodeState Evaluate()
    {
        Node root = GetRoot();
        Debug.Log(root.GetData("resetTimer") != null && (bool)root.GetData("resetTimer"));
        
        if(root.GetData("resetTimer") != null && (bool)root.GetData("resetTimer")){
            time = 0f;
            timerReached = false;
            root.ClearData("resetTimer");
            return NodeState.FAILURE;
        }

        // TIMER REACHED
        if(time >= timeToWait){
            Debug.Log("Timer reached");
            root.ClearData("resetTimer");
            timerReached = true;
            return NodeState.SUCCESS;
        }
        // STILL WAITING
        return NodeState.RUNNING;
    }
}
