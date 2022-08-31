using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    PauseState pauseState;

    void Awake()
    {
        vars.playerTransform = transform.Find("Player");
        vars.enemyRb = GetComponent<Rigidbody>();
        vars.playerObject = GameObject.FindWithTag("Player");
        vars.pcScript = vars.playerObject.GetComponent<PlayerController>();
        vars.playerRb = vars.playerObject.GetComponent<Rigidbody>();

        //Get the direction of the player for later use in force direction
        vars.playerDir = vars.playerTransform.transform.right;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Go directly into the Pausing function
        Pausing();
    }

    
    public void Pausing()
    {
        //Pause enemy movement briefly before changing direction
        transform.position = Vector3.zero;

        //Disable collision on the enemy when colliding with player to avoid any forcing affecting the enemy
        vars.enemyRb.isKinematic = true;

        //Go into the Player Controller script and disable movement before applying force (SET IN PLAYER STATE INSTEAD)
        vars.pcScript.disableMovement = true;
        Debug.Log("disableMovement: " + vars.pcScript.disableMovement);

        //Get the force power to apply
        float force = Mathf.Sign(vars.playerDir.x * -1) * vars.impactForceX;
        Debug.Log("Mathf.Sign(playerDir.x): " + Mathf.Sign(vars.playerDir.x * vars.impactForceX) * -1);
        Debug.Log("force.x: " + force);

        if (vars.pcScript.disableMovement == true)
        {
            //Push the player away
            vars.playerRb.AddForce(force, vars.impactForceY, 0, ForceMode.Impulse);
        }

        //Initiate the state transition from the 'PauseState' script after a brief pause
        pauseState.Invoke("StateTransition", 1f);
}

    


    
    


}
