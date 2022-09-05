using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState : MasterState
{
    //The states that this state can transition into
    public AttackState attackState;
    public PauseState pauseState;

    //Access external scripts
    AI_PatrollingAggro vars;
    Patrol patrol;
    React react;
    PlayerDetectionOneDir playerDetection;

    bool hasMemory;

    public Component thisState;

    public void Start()
    {
        //PRINTA STATE NAMN

        thisState = gameObject.GetComponent<MonoBehaviour>();
        print("State :" + thisState);

        stateManager = GetComponentInParent<StateManager>();

        vars = GetComponentInParent<AI_PatrollingAggro>();
        patrol = GetComponentInParent<Patrol>();
        react = GetComponentInParent<React>();
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();

        //Enable kinematic
        vars.enemyRb.isKinematic = true;

        //Get the enemy's current memory from the React script
        hasMemory = react.hasMemory;

       
    }

    public override void Update()
    {
        //Go into the Player_Detection_One_Dir script and check if we can see the player
        if (playerDetection.CanSeePlayer(vars.enemyDir))
        {
            if (EnemyMemory.MemoryOfPlayer(hasMemory))
            {
                //Transition to Attack state
                stateManager.ChangeState(attackState);
            }
        }
        //Go into the Patrol script and check if we collided with the Player
        else if (patrol.col.CompareTag("Player"))
        {
            //Transition to Pause State if we did
            stateManager.ChangeState(pauseState);
        }
        else
        {
            //Stay in Patrolling State
            patrol.Patrolling();
        }
    }
}
