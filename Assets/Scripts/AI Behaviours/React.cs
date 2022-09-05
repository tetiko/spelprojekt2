using UnityEngine;

public class React : MonoBehaviour
{
    //Access external scripts
    AI_PatrollingAggro vars;

    public bool hasMemory = true;

    // Start is called before the first frame update
    void Start()
    {
        vars = GetComponent<AI_PatrollingAggro>();
        Reacting(gameObject);
    }

    void Reacting(GameObject enemyType)
    {
        //Memorize the player for a set amount of time using the EnemyMemory script
        StartCoroutine(EnemyMemory.Timer(5));

        //Pause enemy movement briefly before reacting
        transform.position = Vector3.zero;

        if (enemyType.tag != "Jump Reaction")
        {
            //React with a surprise jump
            vars.enemyRb.AddForce(Vector3.up * vars.jumpReactionForce, ForceMode.Impulse);

            //Wait for the reaction to play out before chasing
            Invoke("ChasePlayer", 0.7f);
        }
    }
}
