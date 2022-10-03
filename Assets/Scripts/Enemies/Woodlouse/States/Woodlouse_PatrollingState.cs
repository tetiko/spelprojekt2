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
    public Woodlouse_AttackState woodlouse_AttackState;
    public Woodlouse_PauseState woodlouse_PauseState;
    public Woodlouse_ReactionState woodlouse_ReactionState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_Patrol patrol;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goTo_Woodlouse_PauseState = false;

    //Bool for making onStart() run only once per state inititation
    bool executed = false;

    private void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        patrol = GetComponentInParent<Woodlouse_Patrol>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        if (!executed)
        {
            OnStart();
        }

        //Check if we remember the player
        if (vars.hasMemory)
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Transition to Attack state
            return woodlouse_AttackState;
        }
        //Go into the PlayerDetectionOneDir script and check if we can see the player
        else if (playerDetection.CanSeePlayer())
        {
            //Debug.Log("Patrolling State to Reaction State");
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Transition to Reaction state since we have no memory of the player
            return woodlouse_ReactionState;
        }
        //Transition to pause upon collision with player in Patrol script
        else if (goTo_Woodlouse_PauseState)
        {
            //Disable the Patrol script
            vars.patrolEnable = false;
            //Reset the transition bool
            goTo_Woodlouse_PauseState = false;
            //Transition to Pause State
            return woodlouse_PauseState;
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
        //vars.enemyRb.isKinematic = false;
        executed = true;
    }
}
