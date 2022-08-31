using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Attack : MonoBehaviour
{
    //Access the variables from the EnemyVariables file
    EnemyVariables vars;

    // Start is called before the first frame update
    void Awake()
    {
        vars.enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
    }

     public void Attack()
    {   //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        vars.enemyDir = transform.right;
        vars.enemyRb.MovePosition(vars.enemyRb.position + vars.enemyDir * Time.fixedDeltaTime * vars.chaseSpeed);
    }
}
