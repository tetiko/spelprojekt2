using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : MasterState
{
    //The states that this state can transition into
    public Patrolling_State patrollingState;

    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    bool stateTransition = false;

    void Awake()
    {
        //Get and store the enemy object - the parent object of this state
        vars.enemyObject = transform.parent.gameObject;
    }
    public void Start()
    {
        stateManager = GetComponentInParent<StateManager>();

        //Check if the 'Pause' script is added to the object
        if (vars.enemyObject.GetComponent("Pause") != null)
        {
            //Initiate the Pause effects
            gameObject.GetComponent<Pause>().Pausing();
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'Pause' script to " + vars.enemyObject);
        }
    }

    public override void Update()
    {
        if (StateTransition())
        { 
            //Transition into the Patrolling State
            stateManager.ChangeState(patrollingState);
        }
    }

    public override void FixedUpdate()
    {
        //Not used in this state
    }

    bool StateTransition()
    {
        stateTransition = true;

        if (stateTransition)
        {
            return true;
        }
        else return false;
    }

 
}
