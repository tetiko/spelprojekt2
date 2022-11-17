using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    //Access external scripts
    PlayerController pcScript;

    float resetMoveSpeed;

    Vector3 playerDir;
    Rigidbody playerRb;
    Rigidbody enemyRb;

    [HideInInspector] public bool forceAdded = false;
    [HideInInspector] public bool slippery = false;
    [HideInInspector] public bool glide = false;
    [HideInInspector] public bool invulnerable = false;
    [HideInInspector] public bool bouncing = false;
    [HideInInspector] public bool gameOver = false;

    public int invulnerableDuration = 3;
    public GameObject[] lives;
    int life;
   

    Material material;

    // Start is called before the first frame update
    void Start()
    {
        pcScript = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;

        //Set lives
        life = lives.Length;
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

    public void PlayerTakesDamage(int dmg)
    {
        if (life >= 1 && invulnerable == false)
        {
            life -= dmg;
            
            Destroy(lives[life].gameObject);
            Invulnerable();

            if (life < 1)
            {
                GameOver();
            }
        }
    }

    public void Invulnerable()
    {
        StartCoroutine(InvulnerableTimer(invulnerableDuration));
        material.DOColor(Color.red, 0.5f).SetLoops(invulnerableDuration * 2, LoopType.Yoyo);
    }

    public IEnumerator InvulnerableTimer(float time)
    {
        invulnerable = true;

        yield return new WaitForSeconds(time);
        //Turn off invulnurability
        invulnerable = false;
    }

    public void GameOver()
    {
        gameOver = true;
        print("GAME OVER :-(");
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
            bouncing = false;
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
            pcScript.moveSpeed = pcScript.slowedMoveSpeed;
        }

        if (other.name == "Oil Spill")
        {
            resetMoveSpeed = pcScript.moveSpeed;
            pcScript.moveSpeed = pcScript.slipperyMoveSpeed;
            slippery = true;         
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
    { 
        bouncing = true;
  
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
            pcScript.moveSpeed = resetMoveSpeed;
            slippery = false;
        }
    }


}
