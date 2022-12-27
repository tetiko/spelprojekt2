using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.UIElements;
using TMPro;

//Add this script to the enemy game object to give it the desired behaviours

//States are added manually to child objects of the enemy object

//The required states for this AI:

//Woodlouse_AttackState
//Woodlouse_PatrollingState
//Woodlouse_ReactionState
//Woodlouse_CrashState

//StateManager must be added manually to the main enemy object

//When this script is attached to the enemy object the below actions will be automagically added
//The required actions for this AI:
[RequireComponent(typeof(PlayerDetectionOneDir))]
[RequireComponent(typeof(CanRotate))]
[RequireComponent(typeof(Woodlouse_RollAttack))]
[RequireComponent(typeof(Woodlouse_Patrol))]
[RequireComponent(typeof(Woodlouse_React))]
[RequireComponent(typeof(Woodlouse_Crash))]

public class AI_Woodlouse : MonoBehaviour
{
    //All public settings and variables used for this AI
    [Header("Set Player object")]
    public GameObject playerObject;
    [Header("Set the Bones parent object")]
    public GameObject bonesObject;

    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public Rigidbody enemyRb;

    //Enemy: Patrolling Aggro Settings
    [Header("Settings for this enemy")]
    public float moveSpeed = 1f;
    public float chaseSpeed = 5f;
    public float crashDuration = 1f;
    public float reactDuration = 0.2f;
    public float headbuttStartDist = 0.5f;
    public float obsTurnDist = 1.5f;
    public bool defaultPushForces = true;
    [Header("Push Forces (Overriden if Default Push Forces is checked)")]
    public float impactForceX;
    public float impactForceY;

    [HideInInspector] public Vector3 enemyDir;
    [HideInInspector] public PlayerController pcScript;

    //Current state and action for debugging
    MasterState currentState;
    public Type currentAction;

    //Bools for enabling and disabling actions
    [HideInInspector] public bool rollAttackEnable;
    [HideInInspector] public bool patrolEnable;
    [HideInInspector] public bool crashEnable;
    [HideInInspector] public bool reactEnable;

    void Awake()
    {
        enemyObject = gameObject;
        enemyRb = GetComponent<Rigidbody>();
        playerRb = playerObject.GetComponentInChildren<Rigidbody>();

        //Set a placeholder type reference for the current enemy action
        currentAction = GetType();
    }

    void Start()
    { 
        //Disable action scripts
        patrolEnable = GetComponent<Woodlouse_Patrol>().enabled = false;
        crashEnable = GetComponent<Woodlouse_Crash>().enabled = false;
        reactEnable = GetComponent<Woodlouse_React>().enabled = false;
        rollAttackEnable = GetComponent<Woodlouse_RollAttack>().enabled = false;
    }

    void Update()
    {
        //Get the current state from the state manager
        currentState = GetComponent<StateManager>().currentState;

        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = gameObject.transform.forward.normalized;
        //Debug.Log("enemyDir :" + enemyDir);

        //Dynamically enable/disable actions from the state scripts
        GetComponent<Woodlouse_Patrol>().enabled = patrolEnable;
        GetComponent<Woodlouse_Crash>().enabled = crashEnable;
        GetComponent<Woodlouse_React>().enabled = reactEnable;
        GetComponent<Woodlouse_RollAttack>().enabled = rollAttackEnable;
    }

    //Debug: Display the current state and action above the enemy
    public void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            //State
            GUI.color = Color.black;
            Handles.Label(new Vector3((float)(transform.position.x - 0.1), (float)(transform.position.y + 0.35), transform.position.z), currentState.GetType().ToString());
            //Action
            if (currentAction != null)
            { 
                GUI.color = Color.black;
                Handles.Label(new Vector3((float)(transform.position.x - 0.1), (float)(transform.position.y + 0.47), transform.position.z), "Action: " + currentAction.ToString());
            }
        }
    }

}
