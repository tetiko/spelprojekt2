using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Gives the enemy the ability to patrol between obstacles and chosen parameters

public class Patrol : MonoBehaviour
{   
    //Access external scripts
    AI_PatrollingAggro vars;
    PatrollingState patrollingState;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;

    private void Awake()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        patrollingState = GetComponentInChildren<PatrollingState>();
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
        Patrolling();
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
            //Debug.Log("Patrol collision");
            //Get the object we collided with
            col = collision.gameObject;
            //Debug.Log("col: " + col);

            if (col.CompareTag("Player"))
            {
                //Change direction and...
                DirChange(vars.enemyDir, vars.enemyRb);
                //... initiate state transition to pause state
                patrollingState.goToPauseState = true;
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
