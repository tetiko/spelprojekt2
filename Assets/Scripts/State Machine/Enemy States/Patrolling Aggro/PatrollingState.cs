using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PatrollingState : MasterState
{
    //The states that this state can transition into
    public AttackState attackState;
    public PauseState pauseState;
    public ReactionState reactionState;

    //Access external scripts
    AI_PatrollingAggro vars;
    Patrol patrol;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goToPauseState = false;

    //Bool for making onStart() run only once per state inititation
    bool onStart = true;

    private void Awake()
    {
        vars = GetComponentInParent<AI_PatrollingAggro>();
        patrol = GetComponentInParent<Patrol>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        if (onStart)
        {
            OnStart();
            //Debug.Log("action: " + onStart);
        }
        
        //vars.patrolEnable = true;
        //Debug.Log("StateTransition to Pause: " + StateTransition());
        //Debug.Log("Can see player: " + playerDetection.CanSeePlayer());

        //Check if we remember the player
        if (vars.hasMemory)
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Transition to Attack state
            return attackState;
        }
        //Go into the PlayerDetectionOneDir script and check if we can see the player
        else if (playerDetection.CanSeePlayer())
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Transition to Reaction state since we have no memory of the player
            return reactionState;
        }
        //Transition to pause upon collision with player in Patrol script
        else if (StateTransition())
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Reset the transition bool
            goToPauseState = false;
            //Transition to Pause State
            return pauseState;
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

    
    void OnStart()
    {
        //Enable kinematic
        vars.enemyRb.isKinematic = false;
    }

    public bool StateTransition()
    {
        if (goToPauseState)
        {
            return true;
        }
        else return false;
    }
}
