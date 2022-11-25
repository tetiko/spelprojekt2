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
    Silverfish_ChaseAttack chaseAttack;
    CanRotate canRotate;
    PlayerManager playerManager;
    PlayerDetectionOneDir playerDetection;


    [HideInInspector] public bool goTo_Silverfish_PauseState = false;
    [HideInInspector] public bool goTo_Silverfish_PatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Silverfish>();
        chaseAttack = GetComponentInParent<Silverfish_ChaseAttack>();
        canRotate = GetComponentInParent<CanRotate>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        playerDetection.CanSeePlayer();

        //Stop continious checks upon state switch
        if (vars.chaseAttackEnable == false)
        {
            StopCoroutine(chaseAttack.ContiniousChecks());
        }

        //Transition to Pause State upon collision with player in ChaseAttack script
        if (goTo_Silverfish_PauseState)
        {
            //Debug.Log("State switch: silverfish_PauseState");

            //Disable the ChaseAttack script
            vars.chaseAttackEnable = false;
            StopCoroutine(chaseAttack.ContiniousChecks());
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
            StopCoroutine(chaseAttack.ContiniousChecks());
            //Reset state transition
            goTo_Silverfish_PatrollingState = false;
            
            //Transition to Patrolling State
            return silverfish_PatrollingState;
        }

        //If the enemy finished rotating
        else
        {
            //Stay in Attack state
            Attacking();
            //playerManager.PushPlayer(vars.defaultPushForces, gameObject.transform.parent.parent.gameObject, vars.impactForceX, vars.impactForceY);

            return this;
        }
        //else
        //{
        //    //Disable the ChaseAttack script
        //    vars.chaseAttackEnable = false;
        //    //Transition to Patrolling State
        //    return silverfish_PatrollingState;
        //}
    }

    void Attacking()
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
 