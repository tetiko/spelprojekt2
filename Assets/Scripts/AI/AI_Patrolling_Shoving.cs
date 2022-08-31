using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrolling_Shoving : MonoBehaviour
{
    [SerializeField]
    Transform player, eyes;

    [SerializeField]
    float AgroRangeX, AgroRangeY, moveSpeed, chaseSpeed, impactForceX, impactForceY, pauseAfterCollision;

    Rigidbody enemyRb;

    Vector3 enemyDir;
    public bool isGrounded = true;

    bool playerCollision = false;
    private float stopTimer = 0.0f;

    bool memoryOfPlayer = false;

    bool jumpReactionToggle = true;
    float reactionTimer = 0.0f;
    float chaseTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float yDistToPlayer = (Mathf.Abs(transform.position.y - player.position.y));
        //Debug.Log("yDistToPlayer: " + yDistToPlayer);


        //Debug.Log("CollisionWithPlayer:" + CollisionWithPlayer());


        //Debug.Log("playerCollision: " + playerCollision);

    }

    void FixedUpdate()
    {
        if (CanSeePlayer(AgroRangeX))
        {
            //Agro enemy
            ChasePlayer();
        }
        else if (memoryOfPlayer)
        {
            ChasePlayer();
            EnemyReactionTimers();
        }
        else
        {
            //Stop chasing player
            DefaultState();
        }
    }

    void DefaultState()
    {
        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = transform.right;

        if (!CollisionWithPlayer())
        {
            enemyRb.MovePosition(enemyRb.position + enemyDir * Time.fixedDeltaTime * moveSpeed);
        }
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
                //Can see the player
                val = true;

                //Chase the player
                ChasePlayer();

                //Debug.Log("Linecast hit the object tagged Player");

                //Make the enemy jump in surprise when spotting the player, and add a toggle to avoid the jump being continuous
                if (jumpReactionToggle)
                {


                }
                jumpReactionToggle = false;

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

    void ChasePlayer()
    {
        memoryOfPlayer = true;

        //Chase the player until we collide with it
        if (!CollisionWithPlayer())
        {
            enemyRb.MovePosition(enemyRb.position + enemyDir * Time.fixedDeltaTime * chaseSpeed);
        }

        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with the player
        enemyDir = transform.right;
    }

    //Add a limit to how frequently the enemy will react with a jump when spotting the player, as well as under which conditions the enemy will chase the player
    void EnemyReactionTimers()
    {
        reactionTimer += 1;
        Debug.Log("reactionTimer: " + reactionTimer);
        chaseTimer += 1;
        Debug.Log("chaseTimer: " + chaseTimer);

        if (reactionTimer >= 50)
        {
            reactionTimer = 0;
            //Debug.Log("Reaction timer check");
        }

        if (reactionTimer == 0)
        {
            //The enemy is hit with a sudden strike of amnesia
            //memoryOfPlayer = false;
            //Debug.Log("reactionTimer Ping");
            jumpReactionToggle = true;
        }

        //Keep chasing player for a bit after losing sight
        if (chaseTimer >= 100)
        {
            chaseTimer = 0;
        }
        //Stop chasing after set time
        if (chaseTimer == 0)
        {
            memoryOfPlayer = false;
            Debug.Log("chaseTimer");
        }
    }

    bool CollisionWithPlayer()
    {
        bool val = false;

        if (playerCollision)
        {
            stopTimer += 1;
            val = true;

            // Debug.Log("stopTimer: " + stopTimer);
        }

        //Add a pause in movement after the enemy has collided with the player
        if (stopTimer >= pauseAfterCollision)
        {
            enemyDirChange();

            playerCollision = false;
            stopTimer = 0;

            //Debug.Log("chase after movement pause: " + memoryOfPlayer);

        }

        return val;
    }

    void enemyDirChange()
    {

        if (enemyDir.x > 0)
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
            enemyDirChange();
            memoryOfPlayer = false;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Player") && collisionObject.TryGetComponent(out Rigidbody body))
        {
            float force = (enemyDir.x * impactForceX);
            body.AddForce(force, impactForceY, 0, ForceMode.Impulse);

            playerCollision = true;
            //The enemy is hit with a sudden strike of amnesia
            memoryOfPlayer = false;

        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}


