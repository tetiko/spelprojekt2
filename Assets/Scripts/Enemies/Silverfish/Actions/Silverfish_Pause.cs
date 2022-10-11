using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Silverfish_Pause : MonoBehaviour
{
    //Access external scripts
    AI_Silverfish vars;
    Silverfish_PauseState silverfish_PauseState;
    PlayerManager playerManager;
    CanRotate canRotate;

    Type previousAction;

    private void Awake()
    {
        vars = GetComponent<AI_Silverfish>();
        silverfish_PauseState = GetComponentInChildren<Silverfish_PauseState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
        canRotate = GetComponent<CanRotate>();

    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the previous action
        previousAction = vars.currentAction;
        //Get the name of this action
        vars.currentAction = GetType();
        Debug.Log("Class: " + GetType());

        //Go directly into the Pausing function
        Pausing();
    }

    public void Pausing()
    {
        //Pause, change direction and initiate state transition
        StartCoroutine(StateTransition(vars.pauseDuration));
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        
        //Check if we came from ChaseAttack
        if (previousAction.ToString() != "Silverfish_ChaseAttack")
        {
            //... if not, rotate and...
            if (!canRotate.rotate)
            {
                canRotate.rotate = true;
                canRotate.getTargetRotation = true;
            }
            //... state transition to patrol
            silverfish_PauseState.goTo_Silverfish_PatrollingState = true;
        }
        //if we did come from ChaseAttack, go to attack state again
        else
        {
            silverfish_PauseState.goTo_Silverfish_AttackState = true;

        }
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.pauseEnable)
        {
            //On collision with player
            if (collision.gameObject.CompareTag("Player"))
            {
                //Push the player away
                playerManager.PushPlayer(vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                //... initiate state transition to pause state
                //silverfish_PauseState.goTo_Silverfish_AttackState = true;
            }
        }
    }
}
