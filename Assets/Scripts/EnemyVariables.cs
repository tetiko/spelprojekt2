using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    //All public variables used in enemy specific scripts

    //General
    [HideInInspector] public Vector3 enemyDir;
    [HideInInspector] public GameObject collisionObject;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public Rigidbody enemyRb;
    [HideInInspector] public Transform eyes;

    [HideInInspector] public Vector3 playerDir;
    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public PlayerController pcScript;

    //Enemy: Patrolling
    [Header("Enemy: Patrolling:")]
    public float moveSpeed = 10;

    //Enemy: Patrolling Aggro
    [Header("Enemy: Patrolling Aggro:")]
    public float impactForceX = 20;
    public float impactForceY = 20;
    public float chaseSpeed = 20;
    public float aggroRange = 30;
    public float jumpReactionForce = 20;


    //Enemy: Patrolling Shoving
    //[Header("Enemy: Patrolling Shoving:")]


    ////Enemy: Jumping
    //[Header("Enemy: Jumping:")]

    //FIX THE MEMORY OF PLAYER TIMER


}
