using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class React : MonoBehaviour
{
    //Access the global timer
    EnemyMemory enemyMemory;

    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    ReactionState reactionState;

    public bool hasMemory = true;

      // Start is called before the first frame update
    void Start()
    {
        vars.enemyRb = GetComponent<Rigidbody>();

        Reacting(gameObject);
    }
    
    void Reacting(GameObject enemyType)
    {
        //Memorize the player for a set amount of time
        StartCoroutine(enemyMemory.Timer(5));

        //Pause enemy movement briefly before reacting
        transform.position = Vector3.zero;

        if (enemyType.tag != "JumpReaction")
        {
            //React with a surprise jump
            vars.enemyRb.AddForce(Vector3.up * vars.jumpReactionForce, ForceMode.Impulse);

            //Wait for the reaction to play out before chasing
            Invoke("ChasePlayer", 0.7f);
        }
    }
}
