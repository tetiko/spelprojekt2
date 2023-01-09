using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Woodlouse_RolledUpState : MasterState
{
    //The states that this state can transition into
    public Woodlouse_PatrollingState Woodlouse_PatrollingState;

    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_RollUp rollUp;

    [HideInInspector] public bool goTo_Woodlouse_PatrollingState = false;

    void Awake()
    {
        vars = GetComponentInParent<AI_Woodlouse>();
        rollUp = GetComponentInParent<Woodlouse_RollUp>();
    }

    //Update function for the state machine
    public override MasterState RunCurrentState()
    {
        if (goTo_Woodlouse_PatrollingState)
        {
            //Disable the React script
            vars.reactEnable = false;
            //Reset state transition
            goTo_Woodlouse_PatrollingState = false;
            //Transition into the Attack State
            return Woodlouse_PatrollingState;
        }
        else
        {
            //Check if the 'React' script is added to the object
            if (rollUp != null)
            {
                //Initiate the React effects
                vars.rollUpEnable = true;
            }
            else
            {
                Debug.Log("Resolve issue: Add the 'RollUp' script to " + vars.enemyObject);
            }

            //Stay in Reaction State
            return this;
        }
    }
   


}
