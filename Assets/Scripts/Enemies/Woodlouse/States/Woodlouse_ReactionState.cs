using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Woodlouse_ReactionState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_AttackState Woodlouse_AttackState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_React react;
    PlayerDetectionOneDir playerDetection;

    [HideInInspector] public bool goTo_Woodlouse_AttackState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        react = GetComponentInParent<Woodlouse_React>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        playerDetection.CanSeePlayer();

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

        if (goTo_Woodlouse_AttackState)
        {
            //Disable the React script
            vars.reactEnable = false;
            //Reset state transition
            goTo_Woodlouse_AttackState = false;
            //Transition into the Attack State
            return Woodlouse_AttackState;
        }
        else
        {
            //Stay in Reaction State
            return this;
        }
    }
   


}
