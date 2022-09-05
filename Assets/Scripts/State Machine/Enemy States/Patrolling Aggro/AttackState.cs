using UnityEngine;

public class AttackState : MasterState
{
    //The states that this state can transition into
    public PatrollingState patrollingState;
    public PauseState pauseState;

    //Access external scripts
    AI_PatrollingAggro vars;
    React react;
    PlayerDetectionOneDir playerDetection;
    ChaseAttack chaseAttack;

    GameObject collisionObject;

    bool hasMemory;


    public void Start()
    {


        stateManager = GetComponentInParent<StateManager>();

        vars = GetComponentInParent<AI_PatrollingAggro>();
        react = GetComponentInParent<React>();
        Debug.Log("react component: " + react);
        playerDetection = GetComponentInParent<PlayerDetectionOneDir>();
        chaseAttack = GetComponentInParent<ChaseAttack>();

        //Get the enemy's current memory from the React script
        hasMemory = react.hasMemory;
    }

    public override void Update()
    {
        //Check if we can see the player
        if (playerDetection.CanSeePlayer(vars.enemyDir))
        {
            //Stay in Attack state if the enemy can see the player
            InitiateAttack();
        }
        else if (EnemyMemory.MemoryOfPlayer(hasMemory))
        {
            //Stay in Attack state if the enemy remembers the player
            InitiateAttack();
        }
        //Check if we collided with the Player
        else if (collisionObject == GameObject.FindWithTag("Player"))
        {
            //Transition to Pause State if we did
            stateManager.ChangeState(pauseState);
        }
        else
        {
            //Transition to Patrolling State
            stateManager.ChangeState(patrollingState);
        }
    }

    void InitiateAttack()
    {
        //Check if the 'Chase_Attack' script is added to the object
        if (chaseAttack != null)
        {
            //Initiate the attack
            chaseAttack.Attack();
            //Move in the local direction of the transform
            vars.enemyDir = transform.right;
        }
        else
        {
            Debug.Log("Resolve issue: Add the 'ChaseAttack' script to " + gameObject);
        }
    }
}
