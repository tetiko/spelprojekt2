using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Gives the enemy the ability to patrol between obstacles (and chosen parameters (coming soon))

public class Ladybug_Patrol : MonoBehaviour
{   
    //Access external scripts
    AI_Ladybug vars;
    Ladybug_PatrollingState patrollingState;
    PlayerManager playerManager;

    [HideInInspector] public bool disableEnemyMovement = false;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;

    private void Awake()
    {
        vars = GetComponent<AI_Ladybug>();
        patrollingState = GetComponentInChildren<Ladybug_PatrollingState>();
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
            Patrolling();
        }
    }

    //Go about our usual patrolling business
    public void Patrolling()
    {
        vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.moveSpeed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.patrolEnable)
        {
            //Get the object we collided with
            col = collision.gameObject;

            //If we collided with the player
            if (col.CompareTag("Player"))
            {
                ////Get the closest obstruction
                //GameObject closestObs = ClosestTagObject.ClosestObjectWithtag(col.transform, "Obstruction");
                ////Get distance to the closest obstruction
                //float dist = Vector3.Distance(closestObs.transform.position, transform.position);

                ////Change direction and keep patrolling if the enemy is close to an obstruction to avoid hitting the player multiple times
                //if (dist < vars.obsTurnDist)
                //{
                //    //Push the player away
                //    playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                //    //Change direction upon collision with obstruction
                //    DirChange(vars.enemyDir, vars.enemyRb);
                //    //Switch to Patrolling state after waiting for the direction change
                //}
                //else
                //{
                    //Push the player away
                    playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                    //Switch to Pause state
                    patrollingState.ladybug_goToPauseState = true;
                    //Debug.Log("ChaseAttack collision with Player");
                //}
            }

            if (col.CompareTag("Obstruction"))
            {
                //Change direction upon collision with obstruction
                DirChange(vars.enemyDir, vars.enemyRb);
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
