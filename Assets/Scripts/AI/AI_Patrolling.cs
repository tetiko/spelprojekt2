using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrolling : MonoBehaviour
{
    [SerializeField]
    float moveSpeed, impactForceX, impactForceY, pauseAfterCollision;

    Vector3 enemyDir;

    private float stopTimer = 0.0f;
    bool playerCollision = false;

    Rigidbody enemyRb;

    // Start is called before the first frame update
    void Start()
    { 
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        DefaultState();
    }

    void DefaultState()
    {
        //Move in the local direction of the transform. Important since we will be rotating the enemy on collision with obstructions
        enemyDir = transform.right;

        if (!CollisionWithPlayer())
        {
            enemyRb.MovePosition(enemyRb.position + enemyDir * Time.fixedDeltaTime * moveSpeed);
        }
    }

    bool CollisionWithPlayer()
    {
        bool val = false;

        if (playerCollision)
        {
            stopTimer += 1;
            val = true;

            // Debug.Log("stopTimer: " + stopTimer);
        }

        //Add a pause in movement after the enemy has collided with the player
        if (stopTimer >= pauseAfterCollision)
        {
            enemyDirChange();

            playerCollision = false;
            stopTimer = 0;
        }

        return val;
    }

    void enemyDirChange()
    {

        if (enemyDir.x > 0)
        {
            enemyRb.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else
        {
            enemyRb.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collision.gameObject.CompareTag("Obstruction"))
        {
            enemyDirChange();
        }

        if (collision.gameObject.CompareTag("Player") && collisionObject.TryGetComponent(out Rigidbody body))
        {
            float force = (enemyDir.x * impactForceX);
            body.AddForce(force, impactForceY, 0, ForceMode.Impulse);

            playerCollision = true;


        }
    }
}