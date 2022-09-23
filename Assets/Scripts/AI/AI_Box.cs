using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Box : MonoBehaviour
{
    [SerializeField]
    Transform eyes;

    [SerializeField]
    float dist, torqueForce, forceAdjust = 150, initialJumpAngle = 40.0f, attackDelay = 300.0f, aggroRange = 120;

    float attackTimer = 0;

    bool attack = true, attackDelayToggle = false;

    public bool canSeePlayer = false, movingTowards, isPlayerMoving, isCoroutineExecuting = true;

    //Shaking shakingScript;

    Vector3 playerPos;
    Vector3 enemyPos;
    GameObject player;
    Rigidbody playerRb;
    Rigidbody enemyRb; 

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        //shakingScript = gameObject.GetComponent<Shaking>();
        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        //Attack the player if the enemy can see it and the delay is inactive
        if (CanSeePlayer(aggroRange) && attack)
        {
            Attack();
        }

        AttackDelay();

        //Debug.Log("attackDelayToggle: " + attackDelayToggle);
        //Debug.Log("attackTimer:" + attackTimer);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        enemyPos = transform.position;

        MovingTowardsEnemy();
        IsPlayerMoving();
  
        //Debug.Log("playerRb.velocity.magnitude: " + playerRb.velocity.magnitude);
    }

    //Check to see if the player is moving towards or away from the enemy
    bool MovingTowardsEnemy()
    {
        float distTemp = Vector3.Distance(playerPos, enemyPos);

        if (distTemp < dist)
        {
            dist = distTemp;
            movingTowards = true;
        }
        else if (distTemp > dist)
        { 
            dist = distTemp;
            movingTowards = false;
        }
        return movingTowards;
    }

    //Check to see if the player is moving at all
    bool IsPlayerMoving()
    {
        if (playerRb.velocity.magnitude > 0.01f)
        {
            isPlayerMoving = true;
        }
        else
        {
            isPlayerMoving = false;
        }
        return isPlayerMoving;
    }

    void AttackDelay()
    {
        if (attackDelayToggle == true)
        {
            attackTimer += 1;
        }

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            attackDelayToggle = false;
            attack = true;
        }
    }

    Vector3 CalcJumpDist()
    {
        Vector3 p = playerRb.position;

        float gravity = Physics.gravity.magnitude;
        //Selected angle in radians
        float angle = initialJumpAngle * Mathf.Deg2Rad;

        //Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        //Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        //Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        //Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        //Debug.Log("finalVelocity: " + finalVelocity);
        return finalVelocity;

        //Jump!
        //enemyRb.AddForce(finalVelocity * enemyRb.mass, ForceMode.Impulse);

        //Alternative way:
        //enemyRb.velocity = finalVelocity;

    }

    void Attack()
    {
        attackDelayToggle = true;

        float dir, forcePowerX, forcePowerY;
        forcePowerY = CalcJumpDist().y * enemyRb.mass;
        dir = Mathf.Sign(playerPos.x - enemyPos.x); 

        //If player is moving TOWARDS enemy from the RIGHT
        if (dir == 1 && attack && IsPlayerMoving() && MovingTowardsEnemy())
        {
            forcePowerX = (CalcJumpDist().x * enemyRb.mass) - forceAdjust;
            Debug.Log("forcePowerX: " + forcePowerX);

            enemyRb.AddForce(forcePowerX, forcePowerY, 0, ForceMode.Impulse);
            enemyRb.AddTorque(0, 0, -torqueForce, ForceMode.Impulse);

            attack = false;
        }
        //If player is moving AWAY from the enemy on the RIGHT
        else if (dir == 1 && attack && IsPlayerMoving() && !MovingTowardsEnemy())
        {
            forcePowerX = (CalcJumpDist().x * enemyRb.mass) + forceAdjust;
            Debug.Log("forcePowerX: " + forcePowerX);

            enemyRb.AddForce(forcePowerX, forcePowerY, 0, ForceMode.Impulse);
            enemyRb.AddTorque(0, 0, -torqueForce, ForceMode.Impulse);

            attack = false;
        }
        //If player is moving TOWARDS the enemy from the LEFT
        else if (dir == -1 && attack && IsPlayerMoving() && MovingTowardsEnemy())
        {
            forcePowerX = (-CalcJumpDist().x * enemyRb.mass) + forceAdjust;
            Debug.Log("forcePowerX: " + forcePowerX);

            enemyRb.AddForce(forcePowerX, forcePowerY, 0, ForceMode.Impulse);
            enemyRb.AddTorque(0, 0, torqueForce, ForceMode.Impulse);

            attack = false;
        }
        //If player is moving AWAY from the enemy on the LEFT
        else if (dir == -1 && attack && IsPlayerMoving() && !MovingTowardsEnemy())
        {
            forcePowerX = (-CalcJumpDist().x * enemyRb.mass) - forceAdjust;
            Debug.Log("forcePowerX: " + forcePowerX);

            enemyRb.AddForce(forcePowerX, forcePowerY, 0, ForceMode.Impulse);
            enemyRb.AddTorque(0, 0, torqueForce, ForceMode.Impulse);

            attack = false;
        }
        //If player is standing still on the RIGHT side of the enemy
        else if (dir == 1 && attack && !IsPlayerMoving())
        {
            forcePowerX = (CalcJumpDist().x * enemyRb.mass);
            Debug.Log("forcePowerX: " + forcePowerX);

            enemyRb.AddForce(forcePowerX, forcePowerY, 0, ForceMode.Impulse);
            enemyRb.AddTorque(0, 0, -torqueForce, ForceMode.Impulse);

            attack = false;
        }
        //If player is standing still on the LEFT side of the enemy
        else if (dir == -1 && attack && !IsPlayerMoving())
        {
            forcePowerX = (-CalcJumpDist().x * enemyRb.mass);
            Debug.Log("forcePowerX: " + forcePowerX);

            enemyRb.AddForce(forcePowerX, forcePowerY, 0, ForceMode.Impulse);
            enemyRb.AddTorque(0, 0, torqueForce, ForceMode.Impulse);

            attack = false;
        }
    }

    bool CanSeePlayer(float distance)
    {
        canSeePlayer = false;
        float castDist = distance;

        //Make the enemy eyes the cast point for the linecast at the parameter distance
        Vector3 endPosRight = eyes.position + Vector3.right * castDist;
        Vector3 endPosLeft = eyes.position + Vector3.right * -castDist;

        //Cast a line from the enemy in the Action layer
        Physics.Linecast(eyes.position, endPosRight, out RaycastHit hitRight, 1 << LayerMask.NameToLayer("Action"));
        Physics.Linecast(eyes.position, endPosLeft, out RaycastHit hitLeft, 1 << LayerMask.NameToLayer("Action"));

        //Check to see if we hit something in the Action layer mask
        if (hitRight.collider != null)
        {
            //Check to see if we hit the player to the right
            if (hitRight.collider.gameObject.CompareTag("Player"))
            {
                    //Onward!
                    canSeePlayer = true;

                //Debug.Log("Linecast hit the object tagged Player to the right");
            }
            else
            {
                canSeePlayer = false;
            }
            //Draw red and blue lines that shows if the enemy spotted an object to the right
            Debug.DrawLine(eyes.position, endPosRight, Color.red);
            Debug.DrawLine(eyes.position, endPosLeft, Color.blue);
        }
        else
        {
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(eyes.position, endPosRight, Color.blue);
            Debug.DrawLine(eyes.position, endPosLeft, Color.blue);
        }

        if (hitLeft.collider != null)
        {
            //Check to see if we hit the player to the right
            if (hitLeft.collider.gameObject.CompareTag("Player"))
            {
                //Onward!
                canSeePlayer = true;

                //Debug.Log("Linecast hit the object tagged Player to the left")
            }
            //Draw red and blue lines that shows if the enemy spotted an object to the left
            Debug.DrawLine(eyes.position, endPosLeft, Color.red);
            Debug.DrawLine(eyes.position, endPosRight, Color.blue);
        }
        else
        {
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(eyes.position, endPosRight, Color.blue);
            Debug.DrawLine(eyes.position, endPosLeft, Color.blue);
        }
        return canSeePlayer;
    }

    //IEnumerator ExecuteAfterTime(float time, Action task)
    //{

    //    if (isCoroutineExecuting)
    //        yield break;
    //    isCoroutineExecuting = true;
    //    yield return new WaitForSeconds(time);
    //    task();
    //    isCoroutineExecuting = false;
    //}
}
