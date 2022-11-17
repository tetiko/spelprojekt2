using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Silverfish_ChaseAttack : MonoBehaviour
{
    //Access external scripts
    AI_Silverfish vars;
    Silverfish_AttackState silverfish_AttackState;
    PlayerManager playerManager;
    CanRotate canRotate;

    Animator animator;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;

    float distToPlayer;

    float lastPos;
    float velocity;
    float travelTime;

    void Awake()
    {
        vars = GetComponent<AI_Silverfish>();
        silverfish_AttackState = GetComponentInChildren<Silverfish_AttackState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        canRotate = GetComponent<CanRotate>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
     {
        //Get the name of this action
        vars.currentAction = GetType();
        Debug.Log("Class: " + GetType());

        animator.ResetTrigger("Tr_React");
        animator.ResetTrigger("Tr_Pause");
        animator.ResetTrigger("Tr_Turn");
        animator.ResetTrigger("Tr_Headbutt");

        //Play Charge animation
        animator.SetTrigger("Tr_Charge");

        //Start continious checks coroutine
        StartCoroutine(ContiniousChecks());    
    }

    void Update()
    {
        distToPlayer = Vector3.Distance(vars.playerObject.transform.position, transform.position); 
    }

    void FixedUpdate()
    {
        Attack();

        velocity = (vars.enemyRb.position.x - lastPos) * 50;
        lastPos = vars.enemyRb.position.x;

        //Debug.Log("velocity: " + velocity);
    }

    public void Attack()
    {
        //Check if the headbutt animation is over
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Headbutt") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            animator.ResetTrigger("Tr_Headbutt");
            //Debug.Log("1 Headbutt animation loop over");
            //Initiate charge animation
            animator.SetTrigger("Tr_Charge");
        }

        //Attack
        vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.chaseSpeed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.chaseAttackEnable)
        {
            //Get the object we collided with
            col = collision.gameObject;
            //Debug.Log("col: " + col);

            //If we collided with the player
            if (col.CompareTag("Player"))
            {
                //Get the closest obstruction
                GameObject closestObs = ClosestTagObject.ClosestObjectWithtag(col.transform, "Obstruction");
                //Get distance to the closest obstruction
                float dist = Vector3.Distance(closestObs.transform.position, transform.position);

                //Change direction and switch to patrol if the enemy is close to an obstruction to avoid hitting the player multiple times
                if (dist < vars.obsTurnDist)
                {
                    //Push the player away
                    playerManager.PushPlayer(false, gameObject, vars.impactForceX, vars.impactForceY);
                    //Deal damage
                    playerManager.PlayerTakesDamage(1);
                    //animator.ResetTrigger("Tr_Headbutt");
                    animator.ResetTrigger("Tr_Charge");

                    //Play Turn animation and...
                    animator.SetTrigger("Tr_Turn");

                    //...change direction upon collision with the player
                    if (!canRotate.rotate)
                    {
                        canRotate.rotate = true;
                        canRotate.getTargetRotation = true;
                    }

                    //Switch to Patrolling state after waiting for the direction change
                    StartCoroutine(StateTransitionToPatrol(0.05f));
                }
                else
                {
                    //Push the player away
                    playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                    //Deal damage
                    playerManager.PlayerTakesDamage(1);
                    //Switch to Pause state
                    silverfish_AttackState.goTo_Silverfish_PauseState = true;
                    //Debug.Log("ChaseAttack collision with Player");
                }
            }

            //If we collided with Obstruction
            if (col.CompareTag("Obstruction"))
            {
                animator.ResetTrigger("Tr_Headbutt");
                animator.ResetTrigger("Tr_Charge");
                //Play Turn animation and...
                animator.SetTrigger("Tr_Turn");

                //...change direction upon collision with obstruction
                if (!canRotate.rotate)
                {
                    canRotate.rotate = true;
                    canRotate.getTargetRotation = true;
                }
                //Switch to Patrolling state after waiting for the direction change
                StartCoroutine(StateTransitionToPatrol(0.05f)); 
            }
        }
    }

    IEnumerator StateTransitionToPatrol(float time)
    {
        yield return new WaitForSeconds(time);
        //State transition
        //if (!playerDetection.CanSeePlayer() && !canRotate.rotate) 
        //{
            silverfish_AttackState.goTo_Silverfish_PatrollingState = true;
        //}
    }

    public IEnumerator ContiniousChecks()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            Vector3 playerPos = vars.playerObject.transform.position;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toPlayer = playerPos - transform.position;

            //Check if the player is behind us...
            if (Vector3.Dot(forward, toPlayer) < 0)
            {
                //... if it is, go to patrolling state
                silverfish_AttackState.goTo_Silverfish_PatrollingState = true;
                //print("The player is behind me!");   
            }

            travelTime = Mathf.Abs(distToPlayer / (velocity - vars.playerRb.velocity.x));
            //print("travelTime: " + travelTime);

            //Initiate Headbutt animation at the appropriate distance
            if (travelTime < vars.headbuttStartDist)
            {

                //SET MINIMUM DISTANCE
                // Debug.Log("Headbutt");

                animator.SetTrigger("Tr_Headbutt");
            }


        }
    }
}
