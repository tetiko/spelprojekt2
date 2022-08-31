using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionState : MasterState
{
    //The states that this state can transition into
    public Attack_State attackState;

    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    bool stateTransition = false;

    public void Start()
    {
        stateManager = GetComponentInParent<StateManager>();
    }

    public override void Update()
    {
        if (StateTransition())
        {
            //Transition into the Attack State
            stateManager.ChangeState(attackState);
        }
    }

    //CALL THE REACT SCRIPT

    public override void FixedUpdate()
    {

    }
    bool StateTransition()
    {
        stateTransition = true;

        if (stateTransition)
        {
            return true;
        }
        else return false;
    }

}
