using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    //All public settings and variables used for this AI

    //General
    [HideInInspector] public Vector3 enemyDir;
    [HideInInspector] public GameObject collisionObject;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public Rigidbody enemyRb;

    [HideInInspector] public Vector3 playerDir;
    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public PlayerController pcScript;

    //Enemy: Patrolling Aggro
    [Header("Settings for this enemy")]
    public float moveSpeed = 10;
    public float impactForceX = 20;
    public float impactForceY = 20;
    public float chaseSpeed = 20;
    public float aggroRange = 30;
    public float jumpReactionForce = 20;



}
