using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : MasterState
{
    //The states that this state can transition into
    public PatrollingState patrollingState;

    //Access external scripts
    Pause pause;

    bool stateTransition = false;

    public Component currentState;

    public void Start()
    {


        stateManager = GetComponentInParent<StateManager>();

        pause = GetComponentInParent<Pause>();

        //Check if the 'Pause' script is added to the object
        if (pause != null)
        {
            //Initiate the Pause effects
            pause.Pausing();
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'Pause' script to " + gameObject);
        }
    }

    public override void Update()
    {
        if (StateTransition())
        { 
            //Transition into the Patrolling State
            stateManager.ChangeState(patrollingState);
        }
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
