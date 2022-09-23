using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrolling_Aggro : MonoBehaviour
{
    [SerializeField]
    Transform player, eyes;

    [SerializeField]
    float AgroRangeX, AgroRangeY, moveSpeed = 10, chaseSpeed, reactionForce, impactForceX, impactForceY, pauseAfterCollision;

    float setMoveSpeed;

    Rigidbody enemyRb;
    Rigidbody playerRb;

    Vector3 enemyDir, playerDir;

    GameObject playerObject;
    public PlayerController pcScript;

    public bool memoryOfPlayer = false, stopMovement = false, reactionToggle = true, disableMovementToggle = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");
        pcScript = playerObject.GetComponent<PlayerController>();
        playerRb = playerObject.GetComponent<Rigidbody>();
        enemyRb = GetComponent<Rigidbody>();
        setMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopMovement == true)
        {
            moveSpeed = 0;
        }
        //Get the normalized player velocity for later use
        playerDir = playerRb.velocity.normalized;
        enemyDir = enemyRb.velocity.normalized;

        if (disableMovementToggle)
        {
            EnablePlayerMovement();
        }
    }

    void FixedUpdate()
    {
        //React in surprise when spotting the player and we don't have a memory of it
        if (CanSeePlayer(AgroRangeX) && !memoryOfPlayer && reactionToggle && !stopMovement)
        {
            //Agro enemy
            EnemyReaction();
            Debug.Log("EnemyReaction");
        }
        //Chase the player if we can see it or if we have a memory of it
        else if (CanSeePlayer(AgroRangeX) || memoryOfPlayer && !stopMovement)
        {
            ChasePlayer();
            Debug.Log("ChasePlayer");
        }
        //Go about our business as usual if we can't see nor remember the player
        else if (!stopMovement)
        {
            DefaultState();
            Debug.Log("DefaultState");
        }
    }

    void DefaultState()
    {
        //Resume default enemy movement if stopped
        stopMovement = false;
        moveSpeed = setMoveSpeed;
        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        //enemyDir = transform.right;
        //Get the direction of the player for later use in force direction
        //playerDir = player.transform.right;
        
        //Keep patrolling at default speed if not stopped
        //if (!StopMovement())
        //{
            enemyRb.MovePosition(enemyRb.position + enemyDir * moveSpeed);
            //Vector3 v = new Vector3(moveSpeed, 0);
            //enemyRb.velocity = transform.TransformDirection(v);
        //}
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        //Check in which direction the enemy is looking, and set the direction for the linecast accordingly
        if (enemyDir.x < 1)
        {
            castDist = -distance;
        }

        //Make the enemy eyes the cast point for the linecast at the parameter distance
        Vector3 endPos = eyes.position + Vector3.right * castDist;

        //Cast a line from the enemy in the Action layer
        Physics.Linecast(eyes.position, endPos, out RaycastHit hit, 1 << LayerMask.NameToLayer("Action"));

        //Check to see if we hit something in the Action layer mask
        if (hit.collider != null)
        {
            //Debug.Log("Linecast hit something in the Action layer");

            //Check to see if we hit the player
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Onward!
                val = true;

                //Debug.Log("Linecast hit the object tagged Player")
            }
            else
            {
                val = false;
            }

            //Draw a red line that shows the enemy spotting an oject
            Debug.DrawLine(eyes.position, hit.point, Color.red);
        }
        else
        {
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(eyes.position, endPos, Color.blue);
        }
        return val;
    }

    void EnemyReaction()
    {
        //Make the enemy jump in surprise when spotting the player
        stopMovement = true;
        enemyRb.AddForce(Vector3.up * reactionForce, ForceMode.Impulse);

        //Memorize the player
        memoryOfPlayer = true;

        //Wait for the reaction to play out before chasing
        Invoke("ChasePlayer", 0.7f);
        StartCoroutine(ResumeMovement(0.7f));

        //Remember the player for a set amount of time to avoid too frequent enemy surprise reactions
        StartCoroutine(ShortTermMemory(5));
            
        //Toggle the surprise reaction so that it only occurs once every method call
        reactionToggle = false;

    }
    bool ChasePlayer()
    {
        //bool val = false;

        //Chase the player until we forget it or collide with something
        if (memoryOfPlayer)
        {
            Vector3 v = new Vector3(chaseSpeed, 0);
            enemyRb.velocity = transform.TransformDirection(v);

            //Get the direction of the player for later use in force direction
            playerDir = player.transform.right;

            return true;
        }

        //if (CanSeePlayer(AgroRangeX) || memoryOfPlayer)
        //{
        //    val = true;
        //}
        //else
        //{
        //    val = false;
        //}
        else
        {
            return false;
        }
    }

    //Change enemy direction
    void EnemyDirChange()
    {
        //if (enemyDir.x > 0)
        if (Mathf.Sign(enemyDir.x) > 0)

        {
            enemyRb.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else
        {
            enemyRb.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collision.gameObject.CompareTag("Obstruction"))
        {
            //Change direction upon collision with obstruction
            EnemyDirChange();
            //The enemy is hit with a sudden strike of amnesia
            memoryOfPlayer = false;
        }

        //if (collision.gameObject.CompareTag( "Ground"))
        //{
        //    isGrounded = true;
        //}

        if (collision.gameObject.CompareTag("Player") && collisionObject.TryGetComponent(out Rigidbody body))
        {
            //Pause enemy movement briefly before changing direction
            stopMovement = true;
            //Disable collision on the enemy when colliding with player
            enemyRb.isKinematic = true;

            //Go into the Player Controller script and disable movement before applying force
            pcScript.disableMovement = true;
            Debug.Log("disableMovement: " + pcScript.disableMovement);
            //Get the force power to apply
            float force = Mathf.Sign(playerDir.x * -1) * impactForceX;
            Debug.Log("Mathf.Sign(playerDir.x): " + Mathf.Sign(playerDir.x * impactForceX) * -1);
            Debug.Log("force.x: " + force);

            //Push the player away
            body.AddForce(force, impactForceY, 0, ForceMode.Impulse);



            //The enemy is hit with a sudden strike of amnesia
            memoryOfPlayer = false;

            //Change direction and state after a set amount of time
            //Invoke("EnemyDirChange", 1);
            Invoke("DefaultState", 1);

            //Enable collision again for collision with obstructions
            enemyRb.isKinematic = false;   
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            disableMovementToggle = true;
        }
    }

    void EnablePlayerMovement()
    {
        //Go into Player Controller and enable player movement once the player is grounded again
        if (!disableMovementToggle)
        {
            disableMovementToggle = true;
            
            
        }
        if (disableMovementToggle && pcScript.IsGrounded())
        {
            pcScript.disableMovement = false;
            disableMovementToggle = false;
        }
    }

    //Memory capacity
    IEnumerator ShortTermMemory(float time)
    {
        yield return new WaitForSeconds(time);

        //The enemy is hit with a sudden strike of amnesia
        memoryOfPlayer = false;
        reactionToggle = true;
    }

    //Resume enemy movement
    IEnumerator ResumeMovement(float time)
    {
        yield return new WaitForSeconds(time);

        stopMovement = false;
    }

    //Enable player movement again
    //IEnumerator EnablePlayerMovement(float time)
    //{
    //    yield return new WaitForSeconds(time);

    //    pcScript.disableMovement = false;
    //    Debug.Log("disableMovement: " + pcScript.disableMovement);
    //}
}

