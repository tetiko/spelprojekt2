using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Woodlouse_PatrollingState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_RolledUpState Woodlouse_RolledUpState;
    public Woodlouse_ReactionState Woodlouse_ReactionState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_Patrol patrol;
    PlayerDetectionOneDir playerDetection;
    CanRotate canRotate;

    [HideInInspector] public bool goTo_Woodlouse_RolledUpState = false;

    private void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        patrol = GetComponentInParent<Woodlouse_Patrol>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
        canRotate = GetComponentInParent<CanRotate>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Check if we can see the player, and if the enemy finished rotating 
        if (playerDetection.CanSeePlayer() && !canRotate.rotate)
        {
            //Debug.Log("Patrolling State to Reaction State");
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Transition to Reaction state since we have no memory of the player
            return Woodlouse_ReactionState;
        }
        //Transition to Rolled Up state
        else if (goTo_Woodlouse_RolledUpState)
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Reset the transition bool
            goTo_Woodlouse_RolledUpState = false;
            //Transition to crash State
            return Woodlouse_RolledUpState;
        }
        else
        {
            //Check if the 'crash' script is added to the object
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
