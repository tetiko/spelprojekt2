using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this script to the enemy game object to give it the desired behaviours
//When attached to the enemy object all the required behaviours will be automatically added

//States are added manually to child objects of the enemy object

//The required states for this AI:

//AttackState
//PatrollingState
//ReactionState
//PauseState

//StateManager is added to the main enemy object

//The required behaviours for this AI:
[RequireComponent(typeof(ChaseAttack))]
[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(React))]
[RequireComponent(typeof(Pause))]
[RequireComponent(typeof(PlayerDetectionOneDir))]

public class AI_PatrollingAggro : MonoBehaviour
{
    //All public settings and variables used for this AI

    [Header("Set the player object")]
    public GameObject playerObject;

    //Enemy: Patrolling Aggro
    [Header("Settings for this enemy")]
    public Transform eyes;
    public float moveSpeed = 10;
    public float impactForceX = 20;
    public float impactForceY = 20;
    public float chaseSpeed = 20;
    public float aggroRange = 30;
    public float jumpReactionForce = 20;

    [HideInInspector] public Vector3 enemyDir, playerDir;
    [HideInInspector] public Rigidbody enemyRb, playerRb;
    [HideInInspector] public PlayerController pcScript;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerRb = playerObject.GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = gameObject.transform.right;
    }
}
