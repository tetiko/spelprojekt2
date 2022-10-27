using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Silverfish_PatrollingState : MasterState
{
    //The states that this state can transition into
    public Silverfish_AttackState silverfish_AttackState;
    public Silverfish_PauseState silverfish_PauseState;
    public Silverfish_ReactionState silverfish_ReactionState;

    //Access external scripts
    AI_Silverfish vars;
    Silverfish_Patrol patrol;
    PlayerDetectionOneDir playerDetection;
    CanRotate canRotate;

    [HideInInspector] public bool goTo_Silverfish_PauseState = false;

    private void Awake()
    {
        vars = GetComponentInParent<AI_Silverfish>();
        patrol = GetComponentInParent<Silverfish_Patrol>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
        canRotate = GetComponentInParent<CanRotate>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Check if we can see the player, have a memory of it, and if the enemy finished rotating
        if (playerDetection.CanSeePlayer() && !canRotate.rotate)
        {
            //Debug.Log("Patrolling State to Reaction State");
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Transition to Reaction state since we have no memory of the player
            return silverfish_ReactionState;
        }
        //Transition to pause upon collision with player in Patrol script
        else if (goTo_Silverfish_PauseState)
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Reset the transition bool
            goTo_Silverfish_PauseState = false;
            //Transition to Pause State
            return silverfish_PauseState;
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
