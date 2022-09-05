using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAttack : MonoBehaviour
{
    //Access external scripts
    AI_PatrollingAggro vars;

    private void Start()
    {
        vars = GetComponent<AI_PatrollingAggro>();
    }

    void FixedUpdate()
    {
        Attack();
    }

     public void Attack()
    {
        vars.enemyRb.MovePosition(vars.enemyRb.position + new Vector3(0,0) * Time.fixedDeltaTime * vars.chaseSpeed);
    }
}
