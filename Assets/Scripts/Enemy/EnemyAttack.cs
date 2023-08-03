using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// This script handles some aspects of the enemy attack system 
    /// </summary>
    EnemyState enemeyState;
    public float attackRange;
    public float atackStartDelay;
    public GameObject spriteObject;
    Sprite currentSprite;
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    EnemySight enemySight;
    EnemyWalk enemyWalk;
    Animator animator;

    // Use this for initialization
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
        enemyWalk = GetComponent<EnemyWalk>();
        animator = spriteObject.GetComponent<Animator>();
        enemeyState = GetComponent<EnemyState>();

    }

    // Update is called once per frame
    void Update()
    {
        currentSprite = spriteObject.GetComponent<SpriteRenderer>().sprite;

        if (enemeyState.currentState == EnemyState.currentStateEnum.attack) //checks if the enemy is in the attack state
        {
            Attack();
        }
       
    }
    void Attack()
    {
        //This function stops the navmesh agent
        //The actual attacking is done in the Enemy State
        navMeshAgent.ResetPath();
    }
}
