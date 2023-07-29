using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //accesing states
    EnemyState enemeyState;
    /// <summary>
    /// The attack range.
    /// </summary>
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentSprite = spriteObject.GetComponent<SpriteRenderer>().sprite;

        if (enemeyState.currentState == EnemyState.currentStateEnum.attack)
        {
            Attack();
        }
       
    }
    void Attack()
    {
        navMeshAgent.ResetPath();
        //animator.SetBool ("Attack", true);
       
  
    }
}
