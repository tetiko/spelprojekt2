using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Silverfish_PauseState : MasterState
{
    //The states that this state can transition into
    public Silverfish_PatrollingState Silverfish_PatrollingState;
    public Silverfish_AttackState Silverfish_AttackState;

    //Access external scripts
    AI_Silverfish vars;
    Silverfish_Pause pause;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goToS_ilverfish_PatrollingState = false;
    [HideInInspector] public bool goTo_Silverfish_AttackState = false;

    //Bool for making onStart() run only once per state inititation
    bool executed = false;

    

    void Awake()
    {
        vars = GetComponentInParent<AI_Silverfish>();
        pause = GetComponentInParent<Silverfish_Pause>();
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
        if (goToS_ilverfish_PatrollingState)
        { 
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset state transition
            goToS_ilverfish_PatrollingState = false;
            //Transition into the Patrolling State
            return Silverfish_PatrollingState;
        }
        //Transition to Attack State upon collision with player in Pause script
        else if (goTo_Silverfish_AttackState)
        {
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset the transition bool
            goTo_Silverfish_AttackState = false;
            //Transition to Attack State
            return Silverfish_AttackState;
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
