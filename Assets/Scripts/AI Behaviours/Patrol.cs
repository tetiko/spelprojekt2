using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gives the enemy the ability to patrol between obstacles and chosen parameters

public class Patrol : MonoBehaviour
{   
    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    //Variable for storing collisions with the player used in Patrolling_State
    public Collision collision;

    void Awake()
    {
        vars.enemyRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Patrolling();
    }

    //Go about our usual patrolling business
    public void Patrolling()
    {   //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        vars.enemyDir = transform.right;
        vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.moveSpeed);
     }

    //Change direction of the Rigidbody this script is attached to
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

    public void OnCollisionEnter(Collision collision)
    {
        //Get the object we collided with
        if (collision.gameObject.CompareTag("Obstruction"))
        {
            //Change direction upon collision with obstruction
            DirChange(vars.enemyDir, vars.enemyRb);
        }
    }

}
