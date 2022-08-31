using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gives the enemy foward-facing vision and the ability to detect the player when it's not hidden behind obstructions

public class Player_Detection_One_Dir : MonoBehaviour
{
    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    void Awake()
    {
        //Check if an 'eyes' object is added to this enemy, and if not display debug message
        if (vars.eyes != null)
        { 
        vars.eyes = transform.Find("eyes");
        }
        else
        {
            Debug.Log("Resolve issue: Add an 'eyes' object to " + vars.gameObject);
        }
    }

    //Check to see if the enemy can spot the player within the specified range
    public bool CanSeePlayer(Vector3 enemyDir)
    {
        bool val = false;
        float castDist = vars.aggroRange;

        //Check in which direction the enemy is looking, and set the direction for the linecast accordingly
        if (enemyDir.x < 1)
        {
            castDist = -vars.aggroRange;
        }

        //Make the enemy eyes the cast point for the linecast at the parameter distance
        Vector3 endPos = vars.eyes.position + Vector3.right * castDist;

        //Cast a line from the enemy in the Action layer
        Physics.Linecast(vars.eyes.position, endPos, out RaycastHit hit, 1 << LayerMask.NameToLayer("Action"));

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
