using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class Silverfish_AttackState : MasterState
{
    //The states that this state can transition into
    public Silverfish_PatrollingState silverfish_PatrollingState;
    public Silverfish_PauseState silverfish_PauseState;

    //Access external scripts
    AI_Silverfish vars;
    PlayerDetectionOneDir playerDetection;
    Silverfish_ChaseAttack chaseAttack;
    CanRotate canRotate;

    [HideInInspector] public bool goTo_Silverfish_PauseState = false;
    [HideInInspector] public bool goTo_Silverfish_PatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Silverfish>();
        chaseAttack = GetComponentInParent<Silverfish_ChaseAttack>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
        canRotate = GetComponentInParent<CanRotate>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Transition to Pause State upon collision with player in ChaseAttack script
        if (goTo_Silverfish_PauseState)
        {
            //Debug.Log("State switch: silverfish_PauseState");

            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Reset state transition
            goTo_Silverfish_PauseState = false;
            //Transition to Pause State
            return silverfish_PauseState;
        }
        //Transition to Patrolling State upon collision with player in ChaseAttack script
        else if (goTo_Silverfish_PatrollingState)
        {
            //Debug.Log("State switch: silverfish_PatrollingState");
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Reset state transition
            goTo_Silverfish_PatrollingState = false;
            //Transition to Patrolling State
            return silverfish_PatrollingState;
        }

        //If we can see or remember the player
        if (playerDetection.CanSeePlayer() && !canRotate.rotate)
        {
            //Stay in Attack state
            InitiateAttack();
            return this;
        }

        //If we remember the player(Memory timer starts during an enemy reaction in React.cs)
        else if (vars.hasMemory)
        {
            Debug.Log("Using memory in Attack State");
            //Stay in Attack state if we remember the player
            InitiateAttack();
            return this;
        }
        else
        {
            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            //Transition to Patrolling State
            return silverfish_PatrollingState;
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
