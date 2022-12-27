using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Woodlouse_React : MonoBehaviour
{
    //Access external scripts
    AI_Woodlouse vars;
    Woodlouse_ReactionState Woodlouse_ReactionState;
    PlayerManager playerManager;

    Animator animator;
    private void Awake()
    {
        vars = GetComponent<AI_Woodlouse>();
        Woodlouse_ReactionState = GetComponentInChildren<Woodlouse_ReactionState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());

        //Go directly into the Reacting function
        Reacting();
    }

    public void Reacting()
    {
        //Debug.Log("enemyType: " + enemyType.tag);

        //Play React animation
        //if (animator != null)
        //{
        animator.ResetTrigger("Tr_React");

        animator.SetTrigger("Tr_React");
        //}

        StartCoroutine(StateTransition(vars.reactDuration));
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        //State transition to attack state
        Woodlouse_ReactionState.goTo_Woodlouse_AttackState = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.reactEnable)
        {
            //On collision with player
            if (collision.gameObject.CompareTag("Player"))
            {
                //Deal damage
                playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                //... initiate state transition to crash state
                Woodlouse_ReactionState.goTo_Woodlouse_AttackState = true;
            }
        }
    }
}
