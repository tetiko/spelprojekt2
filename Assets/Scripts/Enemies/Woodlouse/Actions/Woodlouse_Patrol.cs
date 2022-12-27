using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Gives the enemy the ability to patrol between obstacles (and chosen parameters (coming soon))

public class Woodlouse_Patrol : MonoBehaviour
{   
    //Access external scripts
    AI_Woodlouse vars;
    PlayerManager playerManager;
    CanRotate canRotate;

    Animator animator;

    //Variable for storing collisions with the player used in Patrolling_State
    [HideInInspector] public GameObject col = null;

    private void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        canRotate = GetComponent<CanRotate>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());

        //Reset animation triggerr
        //animator.ResetTrigger("Tr_Turn");
        animator.ResetTrigger("Tr_Patrol");

        //animator.ResetTrigger("Tr_Charge");
        //animator.ResetTrigger("Tr_Headbutt");
        //animator.ResetTrigger("Tr_crash");

        //Play Patrol animation
        animator.SetTrigger("Tr_Patrol");
    }

    void Update()
    {
        TurnAnimation();
    }

    void FixedUpdate()
    {
        Patrolling();
    }

    //Go about our usual patrolling business
    public void Patrolling()
    {
        //Debug.Log("vars.enemyRb.position: " + vars.enemyRb.position);
        vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.moveSpeed);
    }

    void TurnAnimation()
    { 
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

                //Play Turn animation
                if (animator != null)
                {

                    print("turn check");
                    animator.ResetTrigger("Tr_Turn");

                    animator.SetTrigger("Tr_Turn");
                }

                //Rotate the enemy
                canRotate.rotate = true;
                canRotate.getTargetRotation = true;
            }
        }
    } 
}
