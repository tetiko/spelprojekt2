using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionState : MasterState
{
    //The states that this state can transition into
    public AttackState attackState;

    //Access external scripts


    bool stateTransition = false;

    public Component currentState;

    public void Start()
    {


        //stateManager = GetComponentInParent<StateManager>();
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
