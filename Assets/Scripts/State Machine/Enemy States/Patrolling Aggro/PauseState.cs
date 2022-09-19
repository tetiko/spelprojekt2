using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseState : MasterState
{
    //The states that this state can transition into
    public PatrollingState patrollingState;

    //Access external scripts
    AI_PatrollingAggro vars;
    Pause pause;
    
    [HideInInspector] public bool goToPatrollingState = false;

    //Bool for making onStart() run only once per state inititation
    bool onStart = true;

    void Awake()
    {
        vars = GetComponentInParent<AI_PatrollingAggro>();
        pause = GetComponentInParent<Pause>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //if (onStart)
        //{
            OnStart();
        //}

        //Debug.Log("StateTransition to Patrolling: " + StateTransition());
        if (StateTransition())
        { 
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset state transition
            goToPatrollingState = false;
            //Transition into the Patrolling State
            return patrollingState;
        }
        else
        {
            //Stay in Pause State
            return this;
        }
    }

   
    void OnStart()
    {
        
        //Check if the 'Pause' script is added to the object
        if (pause != null)
        {
            //Initiate the Pause effects
            vars.pauseEnable = true;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'Pause' script to " + vars.enemyObject);
        }
        //Reset the action variable to only call the function once
        onStart = false;
    }

    public bool StateTransition()
    {
        if (goToPatrollingState)
        {
            return true;
        }
        else return false;
    }
}
