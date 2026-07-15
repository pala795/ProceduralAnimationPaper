using UnityEngine;
using System;
using System.Collections.Generic;   

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;
    private bool isTransitioning;
    void Start()
    {
        currentState.EnterState();
    }

    void Update()
    {
        EState nextStateKey = currentState.GetNextState();
        if (!isTransitioning && nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();    
        }
        else
        {
            TransitionToState(nextStateKey);
        }
        
    }

    private void TransitionToState(EState stateKey)
    {
        isTransitioning = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.EnterState();
        isTransitioning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
}
