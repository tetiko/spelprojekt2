using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Woodlouse_CrashState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_PatrollingState Woodlouse_PatrollingState;
    //public Woodlouse_AttackState Woodlouse_AttackState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_Crash crash;

    [HideInInspector] public bool goTo_Woodlouse_PatrollingState = false;
    [HideInInspector] public bool goTo_Woodlouse_AttackState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        crash = GetComponentInParent<Woodlouse_Crash>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Check if the 'crash' script is added to the object
        if (crash != null)
        {
            //Initiate the crash effects
            vars.crashEnable = true;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'Woodlouse_Crash' script to " + vars.enemyObject);
        }

        //Debug.Log("StateTransition to Patrolling: " + StateTransition());
        if (goTo_Woodlouse_PatrollingState)
        { 
            //Disable the crash script
            vars.crashEnable = false;
            //Reset state transition
            goTo_Woodlouse_PatrollingState = false;
            //Transition into the Patrolling State
            return Woodlouse_PatrollingState;
        }

        //Transition to Attack State upon collision with player in crash script
        //else if (goTo_Woodlouse_AttackState)
        //{
        //    //Disable the crash script
        //    vars.crashEnable = false;
        //    //Reset the transition bool
        //    goTo_Woodlouse_AttackState = false;
        //    //Transition to Attack State
        //    return Woodlouse_AttackState;
        //}
        else
        {
            //Stay in crash State
            return this;
        }
    }
}
