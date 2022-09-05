using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public MasterState currentState;

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(MasterState newState)
    {
        currentState = newState;
    }
}
