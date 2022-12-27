using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationTiming : MonoBehaviour
{
    [SerializeField] float minimumAttackDist = 1;
    [SerializeField] float timingTolerance = 0.5f;

    // The time it will take for the enemy to reach the player
    // Used to time the attack animation for enemies that move during the initiation of the attack animation
    public float CalculateAnimationTiming(float dist, Vector3 enemyVel, Vector3 playerVel)
    {
        return dist / (enemyVel.x - playerVel.x);
    }

    // Check if the attack animation should start based on the calculated start time
    public bool ShouldStartAnimation(float animationStartTime)
    {
        return animationStartTime >= 0 && animationStartTime <= timingTolerance;
    }
}


