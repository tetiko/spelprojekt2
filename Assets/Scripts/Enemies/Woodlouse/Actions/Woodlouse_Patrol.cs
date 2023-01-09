using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

//Gives the enemy the ability to patrol between obstacles (and chosen parameters (coming soon))

public class Woodlouse_Patrol : MonoBehaviour
{
    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_PatrollingState Woodlouse_PatrollingState;
    PlayerManager playerManager;
    CanRotate canRotate;

    Animator animator;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;

    private void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        Woodlouse_PatrollingState = GetComponentInChildren<Woodlouse_PatrollingState>();
        canRotate = GetComponent<CanRotate>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get and set the this action
        vars.setAction = GetType();
        vars.currentAction = vars.setAction;
        //Debug.Log("Class: " + GetType());

        //Play Patrol animation
        animator.ResetTrigger("Tr_Patrol");
        animator.SetTrigger("Tr_Patrol");      
    }

    void Update()
    {
        AnimationLoopChecks();
        StateTransition();
    }

    void FixedUpdate()
    {
        Patrolling();
    }

    //Go about our usual patrolling business
    public void Patrolling()
    {
        //Move the enemy if not curled up
        //if(!vars.curledUp)
        //{ 
            //Debug.Log("vars.enemyRb.position: " + vars.enemyRb.position);
            vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.moveSpeed);
        //}
    }

    private void StateTransition()
    {

        if (vars.curlUp == true)
        {
            Woodlouse_PatrollingState.goTo_Woodlouse_RolledUpState = true;
            vars.curlUp = false;
        }
    }

    void AnimationLoopChecks()
    {
        //Wait for turn animation to finish
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Turn") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            //Debug.Log("1 Turn animation loop over");
            animator.ResetTrigger("Tr_Patrol");
            animator.SetTrigger("Tr_Patrol");
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.patrolEnable)
        {
            //Get the object we collided with
            col = collision.gameObject;

            if (col.CompareTag("Player"))
            {
                //Deal damage
                playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
            }

            if (col.CompareTag("Obstruction") && !canRotate.rotate && !animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Turn"))
            {
                //print("turn check");
                animator.ResetTrigger("Tr_Turn");

                animator.SetTrigger("Tr_Turn");

                //Rotate the enemy
                canRotate.rotate = true;
                canRotate.getTargetRotation = true;
            }

        }
    }
}
