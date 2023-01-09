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
//Woodlouse_RollUpState


//StateManager must be added manually to the main enemy object

//When this script is attached to the enemy object the below actions will be automagically added
//The required actions for this AI:
[RequireComponent(typeof(PlayerDetectionOneDir))]
[RequireComponent(typeof(CanRotate))]
[RequireComponent(typeof(Woodlouse_RollAttack))]
[RequireComponent(typeof(Woodlouse_Patrol))]
[RequireComponent(typeof(Woodlouse_React))]
[RequireComponent(typeof(Woodlouse_Crash))]
[RequireComponent(typeof(Woodlouse_RollUp))]


public class AI_Woodlouse : MonoBehaviour
{
    //All public settings and variables used for this AI
    [Header("Set Player object")]
    public GameObject playerObject;

    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public Rigidbody enemyRb;
    [HideInInspector] public Transform enemyTransform;


    //Enemy: Patrolling Aggro Settings
    [Header("Settings for this enemy")]
    public float moveSpeed = 1f;
    public float rollSpeed = 5f;
    //public float crashDuration = 1f;
    public float reactDuration = 0.2f;
     public float attackStartForceX = 5f;
    public float attackStartForceY = 3f;
    //public float obsTurnDist = 1.5f;
    public bool defaultPushForces = true;
    [Header("Push Forces (Overriden if Default Push Forces is checked)")]
    public float impactForceX;
    public float impactForceY;

    [HideInInspector] public Vector3 enemyDir;
    [HideInInspector] public PlayerController pcScript;

    //Current state and action
    MasterState currentState;
    public Type currentAction;
    public Type setAction;

    //Bools for enabling and disabling actions
    [HideInInspector] public bool rollAttackEnable;
    [HideInInspector] public bool patrolEnable;
    [HideInInspector] public bool crashEnable;
    [HideInInspector] public bool reactEnable;
    [HideInInspector] public bool rollUpEnable;


    //Colliders
    public BoxCollider topCollider;
    public BoxCollider sideCollider;
    public BoxCollider rollingCollider;

    //Animator
    [HideInInspector] public Animator animator;

    public bool playerOutsideCollider = true;
    public bool curlUp = false;
    //Woodlouse_CrashState crashStateScript;

    CanRotate canRotate;

    public GameObject bonesObject;

    void Awake()
    {
        enemyObject = gameObject;
        enemyRb = GetComponent<Rigidbody>();
        playerRb = playerObject.GetComponentInChildren<Rigidbody>();
        enemyTransform = enemyObject.transform;
        animator = GetComponent<Animator>();

        //crashStateScript = GetComponentInChildren<Woodlouse_CrashState>();
        canRotate = GetComponent<CanRotate>();

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
        rollUpEnable = GetComponent<Woodlouse_RollUp>().enabled = false;


        topCollider.enabled = true;
        sideCollider.enabled = true;

        rollingCollider.enabled = false;
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
        GetComponent<Woodlouse_RollUp>().enabled = rollUpEnable;

        //Current action
        currentAction = setAction;

        //Enable and disable colliders depending on animations
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.AttackRoll") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("Base.RollUp") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Base.RollOut"))
        {
            rollingCollider.enabled = true;
            topCollider.enabled = false;
            sideCollider.enabled = false;
            playerOutsideCollider = true;
        }
        else
        {
            if (playerOutsideCollider)
            {
                topCollider.enabled = true;
                sideCollider.enabled = true;
                rollingCollider.enabled = false;
            }
        }


        //print("goTo_Woodlouse_PatrollingState: " + crashStateScript.goTo_Woodlouse_PatrollingState);
        //print("crashEnable: " + crashEnable);

    }

    //Trigger that checks if the player exited the the enemy's general collider area
    //Used for enabling/disabling colliders for different animations
    void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is the player object
        if (other.gameObject == playerObject)
        {
            // Enable the box collider
            playerOutsideCollider = true;
        }
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
