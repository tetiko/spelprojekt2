using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ladybug_Pause : MonoBehaviour
{
    //Access external scripts
    AI_Ladybug vars;
    Ladybug_PauseState pauseState;
    PlayerManager playerManager;

    private void Awake()
    {
        vars = GetComponent<AI_Ladybug>();
        pauseState = GetComponentInChildren<Ladybug_PauseState>();
        playerManager = vars.playerObject.GetComponentInChildren<PlayerManager>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());

        //Go directly into the Pausing function
        Pausing();
    }

    public void Pausing()
    {
        //if (animator != null)
        //{
        //    animator.SetTrigger("Tr_Pause");
        //}

        //Pause, change direction and initiate state transition
        StartCoroutine(StateTransition(vars.pauseDuration));
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("StateTransition");

        //... state transition
        pauseState.ladybug_goToPatrollingState = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (vars.pauseEnable)
        {
            //On collision with player
            if (collision.gameObject.CompareTag("Player"))
            {
                print("COLLISION");
                //Deal damage
                playerManager.PlayerTakesDamage(1, vars.defaultPushForces, gameObject, vars.impactForceX, vars.impactForceY);
                //Pause briefly after impact before we keep patrolling
                //Pausing();
            }
        }
    }
}
