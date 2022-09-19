using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChaseAttack : MonoBehaviour
{
    //Access external scripts
    AI_PatrollingAggro vars;
    //AttackState attackState;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;
    [HideInInspector] public string stateSwitch = null;

    void Awake()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        //attackState = GetComponentInChildren<AttackState>();
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
        Attack();
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
                //The enemy is struck with a sudden case of Amnesia
                vars.hasMemory = false;
                //Switch to pause state
                //Debug.Log("ChaseAttack collision with Player");
                stateSwitch = "PauseState";

            }
            //If we collided with Obstruction
            if (col.CompareTag("Obstruction"))
            {
                //The enemy is struck with a sudden case of Amnesia
                vars.hasMemory = false;
                //Switch Patrolling state
                Debug.Log("ChaseAttack collision with Obstruction");
                stateSwitch = "PatrollingState";
            }
        }

    }

    //Change direction of the Rigidbody
    //public void DirChange(Vector3 enemyDir, Rigidbody enemyRb)
    //{
    //    if (Mathf.Sign(enemyDir.x) > 0)
    //    {
    //        enemyRb.rotation = Quaternion.AngleAxis(180, Vector3.up);
    //    }
    //    else
    //    {
    //        enemyRb.rotation = Quaternion.AngleAxis(0, Vector3.up);
    //    }
    //}
}
