using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ReactionState : MasterState
{
    //The states that this state can transition into
    public AttackState attackState;

    //Access external scripts
    AI_Silverfish vars;
    React react;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goToAttackState = false;

    //Bool for making onStart() run only once per state inititation
    bool executed = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Silverfish>();
        react = GetComponentInParent<React>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        //Debug.Log("vars.reactEnable: " + vars.reactEnable);
        if (!executed)
        {
            OnStart();
        }

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

        if (goToAttackState)
        {
            //Disable the React script
            vars.reactEnable = false;
            //Reset state transition
            goToAttackState = false;
            //Transition into the Attack State
            return attackState;
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
