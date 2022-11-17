using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Silverfish_React : MonoBehaviour
{
    //Access external scripts
    AI_Silverfish vars;
    Silverfish_ReactionState silverfish_ReactionState;
    PlayerManager playerManager;

    Animator animator;
    private void Awake()
    {
        vars = GetComponent<AI_Silverfish>();
        silverfish_ReactionState = GetComponentInChildren<Silverfish_ReactionState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        animator = GetComponent<Animator>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        Debug.Log("Class: " + GetType());

        animator.ResetTrigger("Tr_Patrol");

        //Go directly into the Reacting function
        Reacting();
    }

    public void Reacting()
    {
        //Debug.Log("enemyType: " + enemyType.tag);

        //Play React animation
        if (animator != null)
        {
            animator.SetTrigger("Tr_React");
        }

        StartCoroutine(StateTransition(vars.reactDuration));
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        //State transition to attack state
        silverfish_ReactionState.goTo_Silverfish_AttackState = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.reactEnable)
        {
            //On collision with player
            if (collision.gameObject.CompareTag("Player"))
            {
                //Push the player away
                playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                //Deal damage
                playerManager.PlayerTakesDamage(1);
                //... initiate state transition to pause state
                silverfish_ReactionState.goTo_Silverfish_AttackState = true;
            }
        }
    }
}
