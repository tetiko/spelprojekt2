using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class Woodlouse_AttackState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_PatrollingState woodlouse_PatrollingState;
    public Woodlouse_PauseState woodlouse_PauseState;

    //Access external scripts
    AI_Woodlouse vars;
    PlayerDetectionOneDir playerDetection;
    Woodlouse_RushAttack rushAttack;

    [HideInInspector] public bool goTo_Woodlouse_PauseState = false, goTo_Woodlouse_PatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        rushAttack = GetComponentInParent<Woodlouse_RushAttack>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Transition to Pause State upon collision with player in ChaseAttack script
        if (goTo_Woodlouse_PauseState)
        {
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Reset state transition
            goTo_Woodlouse_PauseState = false;
            //Transition to Pause State
            return woodlouse_PauseState;
        }
        //Transition to Patrolling State upon collision with player in ChaseAttack script
        else if (goTo_Woodlouse_PatrollingState)
        {
            //Debug.Log("State switch: woodlouse_PauseState");
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Reset state transition
            goTo_Woodlouse_PatrollingState = false;
            //Transition to Patrolling State
            return woodlouse_PatrollingState;
        }

        //If we can see or remember the player
        if (playerDetection.CanSeePlayer() || vars.hasMemory)
        {
            //Stay in Attack state
            InitiateAttack();
            return this;
        }
        //If we remember the player (Memory timer starts during an enemy reaction in React.cs)
        //else if (vars.hasMemory)
        //{
        //    Debug.Log("Using memory in Attack State");
        //    //Stay in Attack state if we remember the player
        //    InitiateAttack();
        //    return this;
        //}
        else
        {
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Transition to Patrolling State
            return woodlouse_PatrollingState;
        }
    }

    void InitiateAttack()
    {
        //Check if the 'Chase_Attack' script is added to the object
        if (rushAttack != null)
        {
            //Initiate the attack
            vars.chaseAttackEnable = true;
            //Move in the local direction of the transform
            vars.enemyDir = transform.right;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'ChaseAttack' script to " + vars.enemyObject);
        }
    }
}
