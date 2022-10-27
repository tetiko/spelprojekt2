using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Ladybug_PatrollingState : MasterState
{
    //The states that this state can transition into
    public Ladybug_PauseState ladybug_PauseState;
    
    //Access external scripts
    AI_Ladybug vars;
    Ladybug_Patrol patrol;

    [HideInInspector] public bool ladybug_goToPauseState = false;

    private void Awake()
    {
        vars = GetComponentInParent<AI_Ladybug>();
        patrol = GetComponentInParent<Ladybug_Patrol>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Debug.Log("ladybug_goToPauseState: " + ladybug_goToPauseState);
        //Transition to pause upon collision with player in Patrol script
        if (ladybug_goToPauseState)
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Reset the transition bool
            ladybug_goToPauseState = false;
            //Transition to Pause State
            return ladybug_PauseState;
        }
        else
        {
            //Check if the 'Pause' script is added to the object
            if (patrol != null)
            {
                //Initiate the Patrol effects
                vars.patrolEnable = true;
            }
            else
            {
                Debug.Log("Resolve issue: Add the 'Patrol' script to " + vars.enemyObject);
            }
            //Stay in Patrolling State
            return this;
        }  
    }
}
