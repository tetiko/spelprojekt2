using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChaseAttack : MonoBehaviour
{
    //Access external scripts
    public AI_PatrollingAggro vars;
    AttackState attackState;
    PlayerManager playerManager;

    [HideInInspector] public bool disableEnemyMovement = false;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;
    

    void Awake()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        attackState = GetComponentInChildren<AttackState>();
        //Get the player manager component
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());
    }

    void FixedUpdate()
    {
        if (!disableEnemyMovement)
        {
            Attack();
        }
    }

    public void Attack()
    {
        vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.chaseSpeed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.chaseAttackEnable)
        {
            //Get the object we collided with
            col = collision.gameObject;
            //Debug.Log("col: " + col);

            //If we collided with player
            if (col.CompareTag("Player"))
            {
                //Disable collision on the enemy when colliding with player to avoid any forcing affecting the enemy
                //vars.enemyRb.isKinematic = true;

             
                //Push the player away
                playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                //The enemy is struck with a sudden case of Amnesia
                //vars.hasMemory = false;
                //Switch to Pause state
                attackState.goToPauseState = true;
                //Debug.Log("ChaseAttack collision with Player");
            }
            //If we collided with Obstruction
            if (col.CompareTag("Obstruction"))
            {
                //The enemy is struck with a sudden case of Amnesia
                vars.hasMemory = false;
                //Change direction upon collision with obstruction
                DirChange(vars.enemyDir, vars.enemyRb);
                //Switch to Patrolling state
                attackState.goToPatrollingState = true;
                //Debug.Log("ChaseAttack collision with Obstruction"); 
            }
        }
    }

    //Change direction of the Rigidbody
    public void DirChange(Vector3 enemyDir, Rigidbody enemyRb)
    {
        if (Mathf.Sign(enemyDir.x) > 0)
        {
            enemyRb.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else
        {
            enemyRb.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }
}
