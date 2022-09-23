using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gives the enemy foward-facing vision and the ability to detect the player when it's not hidden behind obstructions

public class PlayerDetectionOneDir : MonoBehaviour
{
    //Access external scripts
    AI_PatrollingAggro vars;

    private void Start()
    {
        vars = GetComponent<AI_PatrollingAggro>();
    }

    //Check to see if the enemy can spot the player within the specified range
    public bool CanSeePlayer()
    {
        bool val = false;
        float castDist = vars.aggroRange;

        //Check in which direction the enemy is looking, and set the direction for the linecast accordingly
        if (vars.enemyDir.x < 1)
        {
            castDist = -vars.aggroRange;
        }

        //Make the enemy eyes the cast point for the linecast at the parameter distance LayerMask.NameToLayer("Action")
        Vector3 endPos = vars.eyes.position + Vector3.right * castDist;

        //Cast a line from the enemy in the Action layer
        Physics.Linecast(vars.eyes.position, endPos, out RaycastHit hit, vars.detectionLayers);

        //Check to see if we hit something in the Action layer mask
        if (hit.collider != null)
        {
            //Debug.Log("Linecast hit something in the Action layer");

            //Check to see if we hit the player
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Onward!
                val = true;

                //Debug.Log("Linecast hit the object tagged Player")
            }
            else
            {
                val = false;
            }

            //Draw a red line that shows the enemy spotting an oject
            Debug.DrawLine(vars.eyes.position, hit.point, Color.red);
        }
        else
        {
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(vars.eyes.position, endPos, Color.blue);
        }
        return val;
    }
}
