using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Woodlouse_RollAttack : MonoBehaviour
{
    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_AttackState Woodlouse_AttackState;
    PlayerManager playerManager;

    Animator animator;

    public AnimationClip attackClip;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;
    [HideInInspector] public GameObject col2 = null;


    void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        Woodlouse_AttackState = GetComponentInChildren<Woodlouse_AttackState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Initialize the lastPos and lastTime variables to the current position and time
        //lastPos = vars.enemyRb.position;
        //lastTime = Time.time;

        // Initialize the AttackAnimationTiming instance
        //animationTiming = new AttackAnimationTiming();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
     {
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());

        //animator.ResetTrigger("Tr_crash");
        animator.ResetTrigger("Tr_Attack_Start");

        //Play Charge animation
        animator.SetTrigger("Tr_Attack_Start");  
    }

    private void Update()
    {
        //EnemyVelocity();
        AttackAnimations();
    }

    void FixedUpdate()
    {
        Attack();
        //CheckRelativePlayerPos();

        //Debug.Log("velocity: " + velocity);
    }

    public void Attack()
    {
        //Attack
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.AttackRoll"))
        {
            vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.chaseSpeed);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {  
        if (vars.rollAttackEnable)
        {
            //Get the object we collided with
            col = collision.gameObject;
            //Debug.Log("col: " + col);

            if (col.CompareTag("Obstruction"))
            {
               

                Woodlouse_AttackState.goTo_Woodlouse_CrashState = true;
            }
           
            //Play Turn animation and...
            //animator.SetTrigger("Tr_Turn");

            ////...change direction upon collision with obstruction
            //if (!canRotate.rotate)
            //{
            //    canRotate.rotate = true;
            //    canRotate.getTargetRotation = true;
            //}
            //Switch to Patrolling state after waiting for the direction change
            //StartCoroutine(StateTransitionToPatrol(0.05f));

            //If we collided with the player
            else if (col.CompareTag("Player"))
            {
                //Get the closest obstruction
                //GameObject closestObs = ClosestTagObject.ClosestObjectWithtag(col.transform, "Obstruction");
                ////Get distance to the closest obstruction
                //float dist = Vector3.Distance(closestObs.transform.position, transform.position);

                //Change direction and switch to patrol if the enemy is close to an obstruction to avoid hitting the player multiple times
                //if (dist < vars.obsTurnDist)
                //{
                //    //Deal damage
                //    playerManager.PlayerTakesDamage(1, false, gameObject, vars.impactForceX, vars.impactForceY);
                //    //animator.ResetTrigger("Tr_Headbutt");

                //    //...change direction upon collision with the player
                //    if (!canRotate.rotate)
                //    {
                //        canRotate.rotate = true;
                //        canRotate.getTargetRotation = true;
                //    }

                //    //Switch to Patrolling state after waiting for the direction change
                //    StartCoroutine(StateTransitionToPatrol(0.05f));
                //}
                //else
                //{  


                //Deal damage
                playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                Woodlouse_AttackState.goTo_Woodlouse_CrashState = true;

                //Switch to crash state
                //Woodlouse_AttackState.goTo_Woodlouse_CrashState = true;
                //Debug.Log("RollAttack collision with Player");
                //}
            }

        }
    }

    //IEnumerator StateTransitionToPatrol(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    //State transition
    //    //if (!playerDetection.CanSeePlayer() && !canRotate.rotate) 
    //    //{
    //        Woodlouse_AttackState.goTo_Woodlouse_PatrollingState = true;
    //    //}
    //}

    void AttackAnimations()
    {

        //Check if the Attack_Start animation is over
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.AttackStart") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75)
        {
            //Debug.Log("1 Attack_Start animation loop over");

            //Initiate attack animation
            animator.ResetTrigger("Tr_Attack_Roll");

            animator.SetTrigger("Tr_Attack_Roll");
            
        }
    }

    //private void EnemyVelocity()
    //{
    //    // Calculate the time that has elapsed since the last frame
    //    float deltaTime = Time.deltaTime;

    //    // Calculate the distance the enemy has traveled since the last frame
    //    Vector3 deltaDistance = vars.enemyRb.position - lastPos;

    //    // Calculate the velocity of the enemy
    //    velocity = deltaDistance / deltaTime;

    //    // Update the lastPos and lastTime variables for the next frame
    //    lastPos = vars.enemyRb.position;
    //    lastTime = Time.time;
    //}

    //private void CheckRelativePlayerPos()
    //{
    //    Vector3 playerPos = vars.playerObject.transform.position;
    //    Vector3 forward = transform.TransformDirection(Vector3.forward);
    //    Vector3 toPlayer = playerPos - transform.position;

    //    //Check if the player is behind us...
    //    if (Vector3.Dot(forward, toPlayer) < 0)
    //    {
    //        //... if it is, go to patrolling state
    //        Woodlouse_AttackState.goTo_Woodlouse_PatrollingState = true;
    //        //print("The player is behind me!");   
    //    }
    //}
}
