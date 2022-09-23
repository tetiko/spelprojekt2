using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Access external scripts
    PlayerController pcScript;
    ChaseAttack chaseAttackScript;

    Vector3 playerDir;
    Rigidbody playerRb;

    bool forceAdded = false;

    // Start is called before the first frame update
    void Start()
    {
        pcScript = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        chaseAttackScript = GameObject.Find("EnemyPatrollingAggro").GetComponent<ChaseAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        ResetPush();
    }

    public void PushPlayer(bool defaultPushForces, GameObject forceSource, float customXForce, float customYForce)
    {
        //Disable movement and set velocity to zero to stop player velocity from affecting the push force
        pcScript.disableMovement = true;
        playerRb.velocity = Vector3.zero;
        //chaseAttackScript.disableEnemyMovement = true;

        //Get the direction of the player in relation to the enemy
        playerDir = (transform.position - forceSource.transform.position).normalized;
        //Debug.Log("playerDir.x: " + playerDir.x);

        //If Default Push Forces is checked in the inspector for this object
        if (defaultPushForces && pcScript.disableMovement == true && playerRb.velocity == Vector3.zero)
        {
            //Get the force power to apply
            float xForce = playerDir.x * pcScript.impactForceX;
            //Debug.Log("Default xForce: " + xForce);
            playerRb.AddForce(xForce, pcScript.impactForceY, 0, ForceMode.Impulse);
        }
        //If Default Push Forces is not checked
        else if (!defaultPushForces && pcScript.disableMovement == true && playerRb.velocity == Vector3.zero)
        {
            float xForce = playerDir.x * customXForce;
            //Debug.Log("Custom xForce: " + xForce);
            playerRb.AddForce(xForce, customYForce, 0, ForceMode.Impulse); 
        }
    }

    void ResetPush()
    {
        if (!pcScript.IsGrounded() && pcScript.disableMovement == true)
        {
            //Debug.Log("In air");
            forceAdded = true;
        }

        if (pcScript.IsGrounded() && forceAdded == true)
        {
            //Debug.Log("Landed after push");
            playerRb.velocity = Vector3.zero;
            pcScript.disableMovement = false;
            forceAdded = false;
            chaseAttackScript.disableEnemyMovement = false;
        }
    }
}
