using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Woodlouse_PauseState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_PatrollingState woodlouse_PatrollingState;
    public Woodlouse_AttackState woodlouse_AttackState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_Pause pause;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goTo_Woodlouse_PatrollingState = false;
    [HideInInspector] public bool goTo_Woodlouse_AttackState = false;

    //Bool for making onStart() run only once per state inititation
    //bool executed = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        pause = GetComponentInParent<Woodlouse_Pause>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //if (!executed)
        //{
            OnStart();
        //}

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
        if (goTo_Woodlouse_PatrollingState)
        { 
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset state transition
            goTo_Woodlouse_PatrollingState = false;
            //Transition into the Patrolling State
            return woodlouse_PatrollingState;
        }
        //Transition to Attack State upon collision with player in Pause script
        else if (goTo_Woodlouse_AttackState)
        {
            //Disable the Pause script
            vars.pauseEnable = false;
            //Reset the transition bool
            goTo_Woodlouse_AttackState = false;
            //Transition to Attack State
            return woodlouse_AttackState;
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
