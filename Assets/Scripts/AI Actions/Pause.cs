using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pause : MonoBehaviour
{
    //Access external scripts
    AI_PatrollingAggro vars;
    PauseState pauseState;
    PlayerController pcScript;

    private void Awake()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        pauseState = GetComponentInChildren<PauseState>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        //Debug.Log("Class: " + GetType());

        //Get the player object
        pcScript = vars.playerObject.GetComponentInChildren<PlayerController>();

        //Get the direction of the player for later use in force direction
        vars.playerDir = vars.playerObject.transform.right;

        //Go directly into the Pausing function
        Pausing();
    }

    public void Pausing()
    {
        //Pause enemy movement briefly before changing direction
        //vars.stopEnemy = true;

        ////Disable collision on the enemy when colliding with player to avoid any forcing affecting the enemy
        //vars.enemyRb.isKinematic = true;

        //Go into the Player Controller script and disable movement before applying force (SET IN PLAYER STATE INSTEAD)
        pcScript.disableMovement = true;
        //Debug.Log("disableMovement: " + pcScript.disableMovement);

        //Get the force power to apply
        float force = Mathf.Sign(vars.playerDir.x * -1) * vars.impactForceX;
        //Debug.Log("Mathf.Sign(playerDir.x): " + Mathf.Sign(vars.playerDir.x * vars.impactForceX) * -1);

        //Debug.Log("force.x: " + force);

        if (pcScript.disableMovement == true)
        {
            //Push the player away
            vars.playerRb.AddForce(force, vars.impactForceY, 0, ForceMode.Impulse);
        }

        //Pause, change direction and initiate state transition
        StartCoroutine(StateTransition(2f));
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("Dir change in coroutine");
        //Change direction and...
        DirChange(vars.enemyDir, vars.enemyRb);
        //... state transition
        pauseState.goToPatrollingState = true;
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
