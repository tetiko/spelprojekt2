using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Woodlouse_RushAttack : MonoBehaviour
{
    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_AttackState woodlouse_AttackState;
    PlayerManager playerManager;

    [HideInInspector] public bool disableEnemyMovement = false;

    //Variable for storing collisions with the player used in woodlouse_PatrollingState
    [HideInInspector] public GameObject col = null;

    void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        woodlouse_AttackState = GetComponentInChildren<Woodlouse_AttackState>();
        //Get the player manager component
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        Debug.Log("Class: " + GetType());
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
                    //Change direction upon collision with obstruction
                    DirChange(vars.enemyDir, vars.enemyRb);
                    //Switch to Patrolling state after waiting for the direction change
                    StartCoroutine(StateTransitionToPatrol(0.05f));
                }
                else
                {
                    //Push the player away
                    playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                    //The enemy is struck with a sudden case of Amnesia
                    //vars.hasMemory = false;

                    //Switch to Pause state
                    woodlouse_AttackState.goTo_Woodlouse_PauseState = true;
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
        woodlouse_AttackState.goTo_Woodlouse_PatrollingState = true;
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
