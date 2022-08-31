using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_State : MasterState
{
    //The states that this state can transition into
    public Patrolling_State patrollingState;
    public PauseState pauseState;

    //Access the global timer
    EnemyMemory enemyMemory;

    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    React reactScript;

    bool hasMemory;


    void Awake()
    {
        //Get and store the enemy object - the parent object of this state
        vars.enemyObject = transform.parent.gameObject;

        //Get the enemy's current memory from the React script
        hasMemory = reactScript.hasMemory;
    }
    public void Start()
    {
        stateManager = GetComponentInParent<StateManager>();
    }

    public override void Update()
    {
        //Check if we can see the player
        if (vars.enemyObject.GetComponent<Player_Detection_One_Dir>().CanSeePlayer(vars.enemyDir))
        {
            //Stay in Attack state if the enemy can see the player
            InitiateAttack();
        }
        else if (EnemyMemory.MemoryOfPlayer(hasMemory))
        {
            //Stay in Attack state if the enemy remembers the player
            InitiateAttack();
        }
        //Check if we collided with the Player
        else if (vars.collisionObject == GameObject.FindWithTag("Player"))
        {
            //Transition to Pause State if we did
            stateManager.ChangeState(pauseState);
        }
        else
        {
            //Transition to Patrolling State
            stateManager.ChangeState(patrollingState);
        }
    }

    public override void FixedUpdate()
    {

    }

    void InitiateAttack()
    {
        //Check if the 'Chase_Attack' script is added to the object
        if (vars.enemyObject.GetComponent("Chase_Attack") != null)
        {
            //Initiate the attack
            vars.enemyObject.GetComponent<Chase_Attack>().Attack();
            //Move in the local direction of the transform
            vars.enemyDir = transform.right;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'Chase_Attack' script to " + vars.enemyObject);
        }
    }
}
