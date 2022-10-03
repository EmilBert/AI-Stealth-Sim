using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuardStates
    {
        DEFAULT,
        SUSPICIOUS,
        ALERTED
    }

public abstract class BTree : MonoBehaviour
{

    private Node _root = null;
    
    protected void Start()
    {
        _root = SetupTree();
    }

    // Update is called once per frame
    void Update()
    {
        if( _root != null)
        {
            _root.Evaluate();
        }
    }

    protected abstract Node SetupTree();
}
