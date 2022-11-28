using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;

//Gives the enemy foward-facing vision and the ability to detect the player when it's not hidden behind obstructions

public class PlayerDetectionOneDir : MonoBehaviour
{
    //Access external scripts
    PlayerManager playerManager;

    Vector3 enemyDir;
    public float aggroRange = 30f;

    [Header("Set the player settings object")]
    [SerializeField] GameObject playerObject;
    [Header("Set the eyes object")]
    public Transform eyes;

    [Header("Layers where enemy can detect objects")]
    public LayerMask detectionLayers;

    private void Start()
    {
        playerManager = playerObject.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = gameObject.transform.forward;
        Debug.Log("enemyDir: " + enemyDir);
    }
    //Check to see if the enemy can spot the player within the specified range
    public bool CanSeePlayer()
    {
        bool val = false;
        float castDist = aggroRange;

        //Check in which direction the enemy is looking, and set the direction for the linecast accordingly
        if (enemyDir.x > 0 && enemyDir.z == 0)
        {
            castDist = aggroRange;
        }
        else if (enemyDir.x < 0 && enemyDir.z == 0)
        {
            castDist = -aggroRange;
        }
        else if (enemyDir.z > 0 && enemyDir.x == 0)
        {
            castDist = aggroRange;
        }
        else if (enemyDir.z < 0 && enemyDir.x == 0)
        {
            castDist = -aggroRange;
        }

        Vector3 endPos = eyes.position + enemyDir * castDist;
        //Vector3 endPos2 = eyes.position + new Vector3(0, 0, -0.05f) + Vector3.right * castDist;
        //Vector3 endPos3 = eyes.position + new Vector3(0, 0, 0.05f) + Vector3.right * castDist;

        //Vector3 hit2_Z = new Vector3(0, 0, -0.05f);
        //Vector3 hit3_Z = new Vector3(0, 0, 0.05f);

        //Cast a line from the enemy in the Action layer
        Physics.Linecast(eyes.position, endPos, out RaycastHit hit, detectionLayers);
        //Physics.Linecast(eyes.position + hit2_Z, endPos2, out RaycastHit hit2, detectionLayers);
        //Physics.Linecast(eyes.position + hit3_Z, endPos3, out RaycastHit hit3, detectionLayers);

        if (hit.collider != null && !playerManager.invulnerable)
        { 
            //Debug.Log("Linecast hit something in the detectionLayers");

            //Check to see if we hit an obstruction
            if (hit.collider.gameObject.CompareTag("Obstruction"))
                //|| hit2.collider.gameObject.CompareTag("Obstruction")
                //|| hit3.collider.gameObject.CompareTag("Obstruction"))
            {
                //Debug.Log("Linecast hit obstruction")
            }

            //Check to see if we hit the player and if it's invulnerable
            if (hit.collider.gameObject.CompareTag("Player") && !playerManager.invulnerable)    
            {
                //Onward !
                val = true;

                //Debug.Log("Linecast hit the object tagged Player")
            }
            else
            {
                val = false;
            }

            //Draw a red line that shows the enemy spotting an oject
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(eyes.position, hit.point, Color.red);
            //Debug.DrawLine(eyes.position + hit2_Z, hit.point, Color.red);
            //Debug.DrawLine(eyes.position + hit3_Z, hit.point, Color.red);
        }
        else if (hit.collider != null && playerManager.invulnerable)
        {
            //Draw a green line that represents the enemy being ignorant of the player during invulnerability
            Debug.DrawLine(eyes.position, hit.point, Color.green);
        }
        else
        {
            //Draw a blue line that represents the enemy's vision
            Debug.DrawLine(eyes.position, endPos, Color.blue);
            //Debug.DrawLine(eyes.position + hit2_Z, endPos2, Color.blue);
            //Debug.DrawLine(eyes.position + hit3_Z, endPos3, Color.blue);

        }
        return val;
    }
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(transform.position, 1);
    //}
}
