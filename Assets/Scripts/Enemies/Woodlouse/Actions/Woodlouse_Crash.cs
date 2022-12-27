using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Woodlouse_Crash : MonoBehaviour
{
    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_CrashState Woodlouse_CrashState;
    PlayerManager playerManager;
    CanRotate canRotate;

    Animator animator;
    Animator parentAnimator;

    Type previousAction;

    private void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        Woodlouse_CrashState = GetComponentInChildren<Woodlouse_CrashState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        canRotate = GetComponent<CanRotate>();

        animator = GetComponent<Animator>();
        
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        parentAnimator = vars.enemyObject.GetComponent<Animator>();
        //Get the previous action
        previousAction = vars.currentAction;
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());

        //Go directly into the Crash function
        Crash();
    }

    void Update()
    {
        StateTransition();
    }

    void Crash()
    {
        //Rotate the parent holding the enemy bone structure
        //vars.bonesObject.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        //Play crash animation
        animator.ResetTrigger("Tr_Crash");
        animator.SetTrigger("Tr_Crash");
    }

    void StateTransition()
    {
        //StartCoroutine(CrashAndStateTransition());
        //Wait for crash animation to finish
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Crash") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            //Transition to patrolling state
            //Debug.Log("Crash animation loop over");
            Woodlouse_CrashState.goTo_Woodlouse_PatrollingState = true;

        }
    }

    //IEnumerator CrashAndStateTransition()
    //{
    //    // Get the current state of the animator
    //    AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

    //    // Check if the current animation is the one we want to wait for
    //    if (currentState.IsName("Tr_Crash"))
    //    {
    //        // If so, get the length of the animation
    //        float animationLength = currentState.length;

    //        // Wait for the animation to finish
    //        yield return new WaitForSeconds(animationLength);

    //        //Transition to Patrolling state
    //        Woodlouse_CrashState.goTo_Woodlouse_PatrollingState = true;
    //    }
    //}

    //IEnumerator StateTransition(float time)
    //{
    //    yield return new WaitForSeconds(time);

    //    //Check if we came from a Roll Attack
    //    if (previousAction.ToString() != "Woodlouse_RollAttack")
    //    {
    //        //... if not, rotate and...
    //        if (!canRotate.rotate)
    //        {
    //            //Play Turn animation
    //            if (animator != null)
    //            {
    //                animator.SetTrigger("Tr_Turn");
    //            }

    //            //Change direction
    //            canRotate.rotate = true;
    //            canRotate.getTargetRotation = true;
    //        }
    //        //... state transition to patrol
    //        Woodlouse_CrashState.goTo_Woodlouse_PatrollingState = true;
    //    }
    //    //if we did come from RollAttack, go to attack state again
    //    else
    //    {
    //        Woodlouse_CrashState.goTo_Woodlouse_AttackState = true;
    //    }  
    //}

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.crashEnable)
        {
            //On collision with player
            if (collision.gameObject.CompareTag("Player"))
            {
                // Get the direction of the collision
                Vector3 direction = transform.position - collision.gameObject.transform.position;

                Debug.Log("Mathf.Abs(direction.y): " + Mathf.Abs(direction.y));

                // Check if the collision comes from the sides
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                    {
                        playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                        print("collision is to the right");
                    }
                    else
                    {
                        playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);

                        print("collision is to the left");
                    }
                }
                else
                {
                    //Check if the player jumped on the enemy's belly
                    if (direction.y < 0)
                    {
                        animator.ResetTrigger("Tr_Death");
                        animator.SetTrigger("Tr_Death");

                        print("collision is up");
                    }
                    else
                    {
                        print("collision is down");
                    }

                }
            }
        }
    }
}
