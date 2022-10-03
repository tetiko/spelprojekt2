using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;

//Gives the enemy foward-facing vision and the ability to detect the player when it's not hidden behind obstructions

public class PlayerDetectionOneDir : MonoBehaviour
{
    Vector3 enemyDir;
    public float aggroRange = 30f;

    [Header("Set the eyes object")]
    public Transform eyes;

    [Header("Layers where enemy can detect objects")]
    public LayerMask detectionLayers;

    private void Update()
    {
        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = gameObject.transform.right.normalized;
    }
    //Check to see if the enemy can spot the player within the specified range
    public bool CanSeePlayer()
    {
        bool val = false;


        float castDist = aggroRange;

        //Check in which direction the enemy is looking, and set the direction for the linecast accordingly
        if (enemyDir.x < 1)
        {
            castDist = -aggroRange;
        }

        //Make the enemy eyes the cast point for the linecast at the parameter distance LayerMask.NameToLayer("Action")
        Vector3 endPos = eyes.position + Vector3.right * castDist;

        //Cast a line from the enemy in the Action layer
        Physics.Linecast(eyes.position, endPos, out RaycastHit hit, detectionLayers);

        //Check to see if we hit something in the Action layer mask
        if (hit.collider != null)
        {
            //Debug.Log("Linecast hit something in the Action/Ground layer");

            //Check to see if we hit the player
            if (hit.collider.gameObject.CompareTag("Obstruction"))
            {
                //Store the latest Obstruction hit for use in enemy AI
                val = true;

                //Debug.Log("Linecast hit the object tagged Player")
            }

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
            Debug.DrawLine(eyes.position, hit.point, Color.red);
        }
        else
        {
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(eyes.position, endPos, Color.blue);
        }
        return val;
    }
}
