using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gives the enemy the ability to patrol between obstacles and chosen parameters

public class Patrol : MonoBehaviour
{   
    //Access external scripts
    AI_PatrollingAggro vars;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col;

    void Start()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        col = vars.playerObject;
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
        col = collision.gameObject;
        Debug.Log("Enemy collided with: ");

        //Get the object we collided with
        if (col.gameObject.CompareTag("Obstruction"))
        {
            //Change direction upon collision with obstruction
            DirChange(vars.enemyDir, vars.enemyRb);
        }
    }

}
