using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseState : MasterState
{
    //The states that this state can transition into
    public PatrollingState patrollingState;
    public AttackState attackState;

    //Access external scripts
    AI_Silverfish vars;
    Pause pause;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goToPatrollingState = false;

    //Bool for making onStart() run only once per state inititation
    bool executed = false;

    public bool goToAttackState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Silverfish>();
        pause = GetComponentInParent<Pause>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        if (!executed)
        {
            OnStart();
        }

        //Reset the executed variable to only call OnStart() once
        //if (vars.pauseEnable == true)
        //{
        //    executed = true;
        //}
        //else
        //{
        //    executed = false;
        //}

        playerDetection.CanSeePlayer();

        //Debug.Log("StateTransition to Patrolling: " + StateTransition());
        if (goToPatrollingState)
        { 
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset state transition
            goToPatrollingState = false;
            //Transition into the Patrolling State
            return patrollingState;
        }
        //Transition to Attack State upon collision with player in Pause script
        else if (goToAttackState)
        {
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset the transition bool
            goToAttackState = false;
            //Transition to Attack State
            return attackState;
        }
        else
        {
            //Stay in Pause State
            return this;
        }
    }

    void OnStart()
    { 
        //Check if the 'Pause' script is added to the object
        if (pause != null)
        {
            //Initiate the Pause effects
            vars.pauseEnable = true;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'Pause' script to " + vars.enemyObject);
        }
    }
}
