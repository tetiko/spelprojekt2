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
    Woodlouse_RollAttack woodlouse_Rollattack;
    PlayerManager playerManager;
    CanRotate canRotate;

    Animator animator;
    Animation anim;

    void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        Woodlouse_CrashState = GetComponentInChildren<Woodlouse_CrashState>();
        woodlouse_Rollattack = GetComponent<Woodlouse_RollAttack>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        canRotate = GetComponent<CanRotate>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the previous action
        //previousAction = vars.currentAction;
        //Get and set the this action
        vars.setAction = GetType();
        vars.currentAction = vars.setAction;
        //Debug.Log("Class: " + GetType());

        //Go directly into the Crash function
        Crash();
    }

    void Update()
    {
        //print("In crash ability");
        AnimationLoopChecks();
    }

    void Crash()
    {
        animator.ResetTrigger("Tr_Crash");
        //Play crash animation
        animator.SetTrigger("Tr_Crash");
    }

    void AnimationLoopChecks()
    {
        //AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //string stateName = clipInfo[0].clip.name;
        //Debug.Log("Current animator state: " + stateName);

        //StartCoroutine(CrashAnimation());

        //Wait for crash animation to finish
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Crash") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95)
        {
            //print("Animation time: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //Rotate the transform

            //Rotate if we the crash was the result av collided with an obstruction
            if (woodlouse_Rollattack.obsCol)
            {
                Vector3 rotate = new Vector3(0, transform.eulerAngles.y - 180, 0);
                transform.eulerAngles = rotate;
                woodlouse_Rollattack.obsCol = false;
            }

            //Adjust the enemy's position to make room for the turn animation to trigger upon collision with obstructions
            vars.enemyTransform.position = new Vector3(vars.enemyTransform.position.x + (0.2f * -vars.enemyDir.x), vars.enemyTransform.position.y);

            //Transition to patrolling state
            Woodlouse_CrashState.goTo_Woodlouse_PatrollingState = true;
            vars.crashEnable = false;
        }
        //Wait for the death animation to finish
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            //Destroy the enemy
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (vars.crashEnable)
    //    {
    //        //If we collided with the player
    //        if (collision.gameObject.CompareTag("Obstruction"))
    //        {
                   //COLLISION SET IN PLAYER MANAGER
    //        }
    //    }
    //}

    //IEnumerator CrashAnimation()
    //{
    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Crash"))
    //    {
    //        //print("Animation length: " + animator.GetCurrentAnimatorStateInfo(0).length);

    //        //print("Animation time: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

    //        //Wait for crash animation to finish
    //        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

    //        //Transition to patrolling state
    //        //Debug.Log("Crash animation loop over");

    //    }
    //}

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

}
