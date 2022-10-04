using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Silverfish_ChaseAttack : MonoBehaviour
{
    //Access external scripts
    AI_Silverfish vars;
    Silverfish_AttackState silverfish_AttackState;
    PlayerManager playerManager;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;

    void Awake()
    {
        vars = GetComponent<AI_Silverfish>();
        silverfish_AttackState = GetComponentInChildren<Silverfish_AttackState>();
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

            //If we collided with the player
            if (col.CompareTag("Player"))
            {
                //Get the closest obstruction
                GameObject closestObs = ClosestTagObject.ClosestObjectWithtag(col.transform, "Obstruction");
                //Get distance to the closest obstruction
                float dist = Vector3.Distance(closestObs.transform.position, transform.position);

                //Change direction and switch to patrol if the enemy is close to an obstruction to avoid hitting the player multiple times
                if (dist < vars.obsTurnDist)
                {
                    //Push the player away
                    playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                    //The enemy is struck with a sudden case of Amnesia
                    vars.hasMemory = false;
                    //Change direction upon collision with the player
                    DirChange(vars.enemyDir, vars.enemyRb);
                    //Switch to Patrolling state after waiting for the direction change
                    StartCoroutine(StateTransitionToPatrol(0.05f));
                }
                else
                {
                    //Push the player away
                    playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                    //Switch to Pause state
                    silverfish_AttackState.goTo_Silverfish_PauseState = true;
                    //Debug.Log("ChaseAttack collision with Player");
                }
            }

            //If we collided with Obstruction
            if (col.CompareTag("Obstruction"))
            {
                //The enemy is struck with a sudden case of Amnesia
                vars.hasMemory = false;
                //Change direction upon collision with obstruction
                DirChange(vars.enemyDir, vars.enemyRb);
                //Switch to Patrolling state after waiting for the direction change
                StartCoroutine(StateTransitionToPatrol(0.05f));
                //Debug.Log("ChaseAttack collision with Obstruction"); 
            }
        }
    }

    IEnumerator StateTransitionToPatrol(float time)
    {
        yield return new WaitForSeconds(time);
        //State transition
        silverfish_AttackState.goTo_Silverfish_PatrollingState = true;
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
