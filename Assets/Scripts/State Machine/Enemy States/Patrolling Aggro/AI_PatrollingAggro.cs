using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;


//Add this script to the enemy game object to give it the desired behaviours
//When attached to the enemy object all the required behaviours will be automatically added

//States are added manually to child objects of the enemy object

//The required states for this AI:

//AttackState
//PatrollingState
//ReactionState
//PauseState

//StateManager must be added manually to the main enemy object

//The required behaviours for this AI:
[RequireComponent(typeof(ChaseAttack))]
[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(React))]
[RequireComponent(typeof(Pause))]
[RequireComponent(typeof(PlayerDetectionOneDir))]

public class AI_PatrollingAggro : MonoBehaviour
{
    //All public settings and variables used for this AI

    [Header("Set objects and bodies")]
    public GameObject playerObject;
    public Rigidbody playerRb;
    public GameObject enemyObject;
    public Rigidbody enemyRb;

    //Enemy: Patrolling Aggro Settings
    [Header("Settings for this enemy")]
    public Transform eyes;
    public float moveSpeed = 10;
    public float impactForceX = 20;
    public float impactForceY = 20;
    public float chaseSpeed = 20;
    public float aggroRange = 30;
    public float jumpReactionForce = 20;
    public bool hasMemory = false;

    //public bool stopEnemy;

    [HideInInspector] public Vector3 enemyDir, playerDir;
    [HideInInspector] public PlayerController pcScript;

    //Current state and action for debugging
    MasterState currentState;
    public Type currentAction;

    //Bools for enabling and disabling actions
    public bool chaseAttackEnable;
    public bool patrolEnable;
    public bool pauseEnable;
    public bool reactEnable;


    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerRb = playerObject.GetComponentInChildren<Rigidbody>();

        //Set a placeholder type reference for the current enemy action
        currentAction = GetType();
    }

    void Start()
    {
        
        //Disable action scripts
        patrolEnable = GetComponent<Patrol>().enabled = false;
        pauseEnable = GetComponent<Pause>().enabled = false;
        reactEnable = GetComponent<React>().enabled = false;
        chaseAttackEnable = GetComponent<ChaseAttack>().enabled = false;
    }

    void Update()
    {
        //Get the current state from the state manager
        currentState = GetComponent<StateManager>().currentState;

        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = gameObject.transform.right.normalized;
        //Debug.Log("enemyDir :" + enemyDir);

        //Determine if the enemy remembers the player or not
        if (EnemyMemory.MemoryOfPlayer(hasMemory))
        {
            hasMemory = true;
        }
        else
        {
            hasMemory = false;
        }

        //Dynamically enable/disable actions from the state scripts
        GetComponent<Patrol>().enabled = patrolEnable;
        GetComponent<Pause>().enabled = pauseEnable;
        GetComponent<React>().enabled = reactEnable;
        GetComponent<ChaseAttack>().enabled = chaseAttackEnable;
    }

    //Debug: Display the current state and action above the enemy
    public void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            //State
            GUI.color = Color.black;
            Handles.Label(new Vector3((float)(transform.position.x - 0.1), (float)(transform.position.y + 0.35)), currentState.GetType().ToString());
            //Action
            if (currentAction != null)
            { 
                GUI.color = Color.black;
                Handles.Label(new Vector3((float)(transform.position.x - 0.1), (float)(transform.position.y + 0.47)), "Action: " + currentAction.ToString());
            }
        }
    }

}
