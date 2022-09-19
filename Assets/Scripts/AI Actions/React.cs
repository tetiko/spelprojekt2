using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class React : MonoBehaviour
{
    //Access external scripts
    AI_PatrollingAggro vars;
    ReactionState reactionState;

    private void Awake()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        reactionState = GetComponentInChildren<ReactionState>();
    }

    // OnEnable is called upon enabling a component
    void OnEnable()
    {
        //Get the name of this action
        vars.currentAction = GetType();
        Debug.Log("Class: " + GetType());

        //Go directly into the Reacting function
        Reacting(gameObject);
    }

    public void Reacting(GameObject enemyType)
    {
        //Debug.Log("enemyType: " + enemyType.tag);
        //Memorize the player for a set amount of time using the EnemyMemory script
        vars.hasMemory = true;
        StartCoroutine(EnemyMemory.Timer(5));

        if (enemyType.tag == "Jump Reaction")
        {
            //Debug.Log("Jump Reaction");
            //React with a surprise jump
            vars.enemyRb.AddForce(Vector3.up * vars.jumpReactionForce, ForceMode.Impulse);

            //Wait for the reaction to play out before chasing
            StartCoroutine(StateTransition(0.5f));
        }
    }

    IEnumerator StateTransition(float time)
    {
        yield return new WaitForSeconds(time);
        //state transition
        reactionState.goToAttackState = true;
    }
}
