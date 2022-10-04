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

    private void Awake()
    {
        vars = GetComponent<AI_Silverfish>();
        silverfish_PauseState = GetComponentInChildren<Silverfish_PauseState>();
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
        //Pause, change direction and initiate state transition
        StartCoroutine(StateTransition(vars.pauseDuration));
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("Dir change in coroutine");
        //Change direction and...
        //DirChange(vars.enemyDir, vars.enemyRb);

        //... state transition
        silverfish_PauseState.goToS_ilverfish_PatrollingState = true;
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
                silverfish_PauseState.goTo_Silverfish_AttackState = true;
            }
        }
    }

    //Change direction of the Rigidbody
    public void DirChange(Vector3 enemyDir, Rigidbody enemyRb)
    {
        if (Mathf.Sign(enemyDir.x) > 0)
        {
            enemyRb.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else
        {
            enemyRb.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }
}