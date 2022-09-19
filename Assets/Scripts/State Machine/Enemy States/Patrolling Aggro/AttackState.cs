using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class AttackState : MasterState
{
    //The states that this state can transition into
    public PatrollingState patrollingState;
    public PauseState pauseState;

    //Access external scripts
    AI_PatrollingAggro vars;
    PlayerDetectionOneDir playerDetection;
    ChaseAttack chaseAttack;

    [HideInInspector] public bool toPauseState = false, toPatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_PatrollingAggro>();
        chaseAttack = GetComponentInParent<ChaseAttack>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Transition to Pause State upon collision with player in ChaseAttack script
        if (chaseAttack.stateSwitch == "PauseState")
        {
            //Debug.Log("State switch: PauseState");
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Transition to Pause State
            return pauseState;
        }
        //Transition to Patrolling State upon collision with player in ChaseAttack script
        else if (chaseAttack.stateSwitch == "PatrollingState")
        {
            //Debug.Log("State switch: PauseState");
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Transition to Patrolling State
            return patrollingState;
        }

        //If we can see the player
        if (playerDetection.CanSeePlayer())
        {
            //Stay in Attack state if the enemy can see the player
            InitiateAttack();
            return this;
        }
        //If we remember the player (Memory timer starts during an enemy reaction in React.cs)
        else if (vars.hasMemory)
        {
            //Stay in Attack state if we remember the player
            InitiateAttack();
            return this;
        }
        else
        {
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Transition to Patrolling State
            return patrollingState;
        }
    }

    void InitiateAttack()
    {
        //Check if the 'Chase_Attack' script is added to the object
        if (chaseAttack != null)
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
