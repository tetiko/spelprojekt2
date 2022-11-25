using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public MasterState currentState;

    void Update()
    {
        RunStateMachine(); 
    }

    void RunStateMachine()
    {
        MasterState nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
        //Debug.Log("current state: " + currentState);
    }

    void SwitchToNextState(MasterState nextState)
    {
        currentState = nextState;
    }
}
