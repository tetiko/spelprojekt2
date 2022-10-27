using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Access external scripts
    PlayerController pcScript;

    Vector3 playerDir;
    Rigidbody playerRb;
    Rigidbody enemyRb;

    bool forceAdded = false;

    // Start is called before the first frame update
    void Start()
    {
        pcScript = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //ResetPush();
    }

    public void PushPlayer(bool defaultPushForces, GameObject forceSource, float customXForce, float customYForce)
    {
        //Disable movement and set velocity to zero to stop player velocity from affecting the push force
        pcScript.disableMovement = true;
        playerRb.velocity = Vector3.zero;
        playerRb.useGravity = false;

        //Minimum amount of time until movement can be enabled again
        StartCoroutine(DisableMovement(0.05f));

        //Get the rigidbody of the force source if it has one and make it kinematic during impact
        enemyRb = forceSource.GetComponent<Rigidbody>();

        if (enemyRb != null)
        {
            enemyRb.isKinematic = true;
        }
        else
        {
            Debug.Log("The source of the force applied to the player doesn't have a Rigidbody");
        }
        
        //Get the direction of the player in relation to the enemy
        playerDir = (transform.position - forceSource.transform.position).normalized;
        //Debug.Log("playerDir.x: " + playerDir.x);

        //If Default Push Forces is checked in the inspector for this object
        if (defaultPushForces && pcScript.disableMovement == true && playerRb.velocity == Vector3.zero && enemyRb.isKinematic == true)
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

    IEnumerator DisableMovement(float time)
    {
        //Wait a minimum amount of time before movement can be enabled
        yield return new WaitForSeconds(time);
        //Reset variables
        playerRb.useGravity = true;
        enemyRb.isKinematic = false;
        forceAdded = true;   
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject colObject = collision.gameObject;
        LayerMask colMask = collision.gameObject.layer;
        //Enable movement again after the player lands again or collides with an obstruction
        if (forceAdded && colObject.CompareTag("Obstruction") || forceAdded && colMask == LayerMask.NameToLayer("Ground"))
        {
            //Reset push
            playerRb.velocity = Vector3.zero;
            pcScript.disableMovement = false;
            forceAdded = false;
            //Debug.Log("Landed after push");
        }
    }

}
