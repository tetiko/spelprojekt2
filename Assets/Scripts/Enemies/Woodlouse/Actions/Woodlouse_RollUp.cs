using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Woodlouse_RollUp : MonoBehaviour
{
    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_RolledUpState Woodlouse_RolledUpState;
    PlayerManager playerManager;

    Animator animator;

    void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        Woodlouse_RolledUpState = GetComponentInChildren<Woodlouse_RolledUpState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get and set the this action
        vars.setAction = GetType();
        vars.currentAction = vars.setAction;
        //Debug.Log("Class: " + GetType());

        //Go directly into the Reacting function
        RollUp();

        //Reset the curled up bool for the Roll up animatíon
        //vars.curledUp = false;
    }

    private void Update()
    {
        AnimationLoopChecks();
    }

    public void RollUp()
    {
        //Debug.Log("enemyType: " + enemyType.tag);

        //Play the roll up animation
        animator.ResetTrigger("Tr_Roll_Up");
        animator.SetTrigger("Tr_Roll_Up");

    }

    void AnimationLoopChecks()
    {
        //Wait for the RollOut animation to finish
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.RollOut") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95)
        {
            Woodlouse_RolledUpState.goTo_Woodlouse_PatrollingState = true;
        }
    }

    //IEnumerator StateTransition(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    //State transition to patrolling state
    //    Woodlouse_RolledUpState.goTo_Woodlouse_PatrollingState = true;
    //}

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.rollUpEnable)
        {
            //On collision with player
            if (collision.gameObject.CompareTag("Player"))
            {
                //Deal damage
                playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                print("Collision when rolled up");
            }
        }
    }
}
