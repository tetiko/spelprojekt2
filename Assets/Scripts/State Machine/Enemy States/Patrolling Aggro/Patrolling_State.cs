using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling_State : MasterState
{
    //The states that this state can transition into
    public Attack_State attackState;
    public PauseState pauseState;

    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    //Access the variables from the Patrol script
    Patrol patrolScript;

    //Access the variables from the React script
    React reactScript;

    bool hasMemory;

    void Awake()
    {
        //Get and store the enemy object - the parent object of this state
        vars.enemyObject = transform.parent.gameObject;

        //Disable kinematic for collisions with obstructions
        vars.enemyRb.isKinematic = false;
    }

    public void Start()
    {
        stateManager = GetComponentInParent<StateManager>();

        //Get the enemy's current memory from the React script
        hasMemory = reactScript.hasMemory;
    }

    public override void Update()
    {
        //Go into the Player_Detection_One_Dir script and check if we can see the player
        if (vars.enemyObject.GetComponent<Player_Detection_One_Dir>().CanSeePlayer(vars.enemyDir))
        {
            if (EnemyMemory.MemoryOfPlayer(hasMemory))
            {
                //Transition to Attack state
                stateManager.ChangeState(attackState);
            }
        }
        //Go into the Patrolling script and check if we collided with the Player
        else if (patrolScript.collision.gameObject == gameObject.CompareTag("Player"))
        {
            //Transition to Pause State if we did
            stateManager.ChangeState(pauseState);
        }
        else
        {
            //Stay in Patrolling State
            vars.enemyObject.GetComponent<Patrol>().Patrolling();
        }
    }

    public override void FixedUpdate()
    {
        //Not used in this state
    }


}
