using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Woodlouse_ReactionState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_AttackState woodlouse_AttackState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_React react;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goTo_Woodlouse_AttackState = false;

    //Bool for making onStart() run only once per state inititation
    //bool executed = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        react = GetComponentInParent<Woodlouse_React>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Debug.Log("vars.reactEnable: " + vars.reactEnable);
        //if (!executed)
        //{
            OnStart();
        //}

        //Reset the executed variable to only call OnStart() once
        //if (vars.reactEnable == true)
        //{
        //    executed = true;
        //}
        //else
        //{
        //    executed = false;
        //}

        playerDetection.CanSeePlayer();

        if (goTo_Woodlouse_AttackState)
        {
            //Disable the React script
            vars.reactEnable = false;
            //Reset state transition
            goTo_Woodlouse_AttackState = false;
            //Transition into the Attack State
            return woodlouse_AttackState;
        }
        else
        {
            //Stay in Reaction State
            return this;
        }
    }
   
    void OnStart()
    {
        //Check if the 'React' script is added to the object
        if (react != null)
        {
            //Initiate the React effects
            vars.reactEnable = true;  
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'React' script to " + vars.enemyObject);
        }
    }
}
