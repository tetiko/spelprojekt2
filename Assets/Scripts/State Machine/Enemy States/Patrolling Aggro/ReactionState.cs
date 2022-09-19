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
    AI_PatrollingAggro vars;
    React react;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goToAttackState = false;

    //Bool for making onStart() run only once per state inititation
    public bool onStart = true;

    void Awake()
    {
        vars = GetComponentInParent<AI_PatrollingAggro>();
        react = GetComponentInParent<React>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();

    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        Debug.Log("vars.reactEnable: " + vars.reactEnable);
        


        //if (onStart)
        //{
            Debug.Log("onStart: " + onStart);
            OnStart();
        //}

        playerDetection.CanSeePlayer();

        if (StateTransition())
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
        //onStart = true;

        //Check if the 'React' script is added to the object
        if (react != null)
        {
            //Initiate the React effects
            
            vars.reactEnable = true;

            if (onStart && vars.reactEnable)
            {
                //Reset the action variable to only call the function once
                //onStart = false;
            }
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'React' script to " + vars.enemyObject);
        }


    }

    bool StateTransition()
    {
        if (goToAttackState)
        {
            return true;
        }
        else return false;
    }
}
