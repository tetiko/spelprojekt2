using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Access external scripts
    PlayerController pcScript;

    float resetMoveSpeed;

    Vector3 playerDir;
    Rigidbody playerRb;
    Rigidbody enemyRb;

    [HideInInspector] public bool forceAdded = false;
    public bool slippery = false, glide = false;
    public Vector3 initialGlideVel;
    public Vector3 initialOilVel;

    // Start is called before the first frame update
    void Start()
    {
        pcScript = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!invulnerable)
        {
            material.DOColor(Color.green, 0);
        }
    }

    public void PushPlayer(bool defaultPushForces, GameObject forceSource, float customXForce, float customYForce)
    {
        //if (dealDamage)
        //{
            //Make the player briefly invulnurable after taking damage
            StartCoroutine(Invulnerable(2f));


        //}

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

        //If Default Push Forces is true for this object
        if (defaultPushForces && pcScript.disableMovement == true && playerRb.velocity == Vector3.zero && enemyRb.isKinematic == true)
        {
            //Get the force power to apply
            float xForce = playerDir.x * pcScript.impactForceX;
            //Debug.Log("Default xForce: " + xForce);
            playerRb.AddForce(xForce, pcScript.impactForceY, 0, ForceMode.Impulse);
        }
        //If Default Push Forces is false
        else if (!defaultPushForces && pcScript.disableMovement == true && playerRb.velocity == Vector3.zero)
        {
            float xForce = playerDir.x * customXForce;
            //Debug.Log("Custom xForce: " + xForce);
            playerRb.AddForce(xForce, customYForce, 0, ForceMode.Impulse); 
        }
    }

    public IEnumerator DisableMovement(float time)
    {
        //Wait a minimum amount of time before movement can be enabled
        yield return new WaitForSeconds(time);
        //Reset variables
        playerRb.useGravity = true;
        enemyRb.isKinematic = false;
        forceAdded = true;   
    }

    public IEnumerator Invulnerable(float time)
    {
        invulnerable = true;

        while (invulnerable)
        {
            //Make the player blink to signal invulnerability
            material.DOColor(Color.red, 1).From().SetLoops(2, LoopType.Restart);
        }

        //Turn off invulnurability
        yield return new WaitForSeconds(time);
        invulnerable = false;


    }
            material.DOColor(Color.green, 0);
        }
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

    //Interactables and hazards affecting the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Spiderweb")
        {
            pcScript.disableJump = true;
            resetMoveSpeed = pcScript.moveSpeed;
            pcScript.moveSpeed = pcScript.webbedMoveSpeed;
        }

        if (other.name == "Oil Spill")
        {
            //resetMoveSpeed = pcScript.oilMoveSpeed;
            //pcScript.moveSpeed = pcScript.oilMoveSpeed;
            slippery = true;
            glide = true;

            if (glide)
            {
                initialGlideVel = playerRb.velocity;
                playerRb.velocity = new Vector3(initialGlideVel.x, playerRb.velocity.y);
            }
            //else if (!glide)
            //{
            //    initialOilVel = playerRb.velocity;
                
            //}
            
        }

        if (other.name == "Bouncy Area")
        {
            float velX = playerRb.velocity.x;
            pcScript.disableMovement = true;
            playerRb.AddForce(new Vector3(velX * pcScript.bounceForceX, pcScript.bounceForceY), ForceMode.Impulse);
            StartCoroutine(Trampoline(0.05f));
        }
    }

    IEnumerator Trampoline(float time)
        bouncing = true;
    {
        yield return new WaitForSeconds(time);
        forceAdded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Spiderweb")
        {
            pcScript.disableJump = false;
            pcScript.moveSpeed = resetMoveSpeed;
        }

        if (other.name == "Oil Spill")
        {
            slippery = false;
            glide = false;
        }
    }


}
