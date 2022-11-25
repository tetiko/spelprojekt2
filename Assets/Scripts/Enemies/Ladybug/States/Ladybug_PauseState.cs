using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Ladybug_PauseState : MasterState
{
    //The states that this state can transition into
    public Ladybug_PatrollingState ladybug_PatrollingState;

    //Access external scripts
    AI_Ladybug vars;
    Ladybug_Pause pause;

    [HideInInspector] public bool ladybug_goToPatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Ladybug>();
        pause = GetComponentInParent<Ladybug_Pause>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        OnStart();
        
        if (ladybug_goToPatrollingState)
        { 
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset state transition
            ladybug_goToPatrollingState = false;
            //Transition into the Patrolling State
            return ladybug_PatrollingState;
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
    }
}
