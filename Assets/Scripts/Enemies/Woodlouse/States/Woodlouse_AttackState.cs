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
    //public Woodlouse_PatrollingState Woodlouse_PatrollingState;
    public Woodlouse_CrashState Woodlouse_CrashState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_RollAttack rollAttack;
    //CanRotate canRotate;
    //PlayerManager playerManager;
    //PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goTo_Woodlouse_CrashState = false;
    //[HideInInspector] public bool goTo_Woodlouse_PatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        rollAttack = GetComponentInParent<Woodlouse_RollAttack>();
        //canRotate = GetComponentInParent<CanRotate>();
        //playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        //playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Transition to Crash State upon collision with player in RollAttack script
        if (goTo_Woodlouse_CrashState)
        {
            //Debug.Log("State switch: Woodlouse_CrashState");

            //Disable the RollAttack script
            vars.rollAttackEnable = false;
            //Reset state transition
            goTo_Woodlouse_CrashState = false;
            //Transition to crash State
            return Woodlouse_CrashState;
        }
        //Transition to Patrolling State upon collision with player in RollAttack script
        //else if (goTo_Woodlouse_PatrollingState)
        //{
        //    //Debug.Log("State switch: Woodlouse_PatrollingState");
        //    //Disable the RollAttack script
        //    vars.rollAttackEnable = false;
        //    //Reset state transition
        //    goTo_Woodlouse_PatrollingState = false;
            
        //    //Transition to Patrolling State
        //    return Woodlouse_PatrollingState;
        //}

        //If the enemy finished rotating
        else
        {
            //Stay in Attack state
            Attacking();
            return this;
        }
        //else
        //{
        //    //Disable the RollAttack script
        //    vars.RollAttackEnable = false;
        //    //Transition to Patrolling State
        //    return Woodlouse_PatrollingState;
        //}
    }

    void Attacking()
    {
        //Check if the 'Chase_Attack' script is added to the object
        if (rollAttack != null)
        {
            //Initiate the attack
            vars.rollAttackEnable = true;
            //Move in the local direction of the transform
            //vars.enemyDir = transform.right;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'RollAttack' script to " + vars.enemyObject);
        }
    }


}
 