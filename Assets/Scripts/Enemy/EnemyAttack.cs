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

    public GameObject attackBox1, attackBox2, attackBox3;
    public Sprite attack1HitFrame, attack2HitFrame, attack3HitFrame;

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
        /*if (enemySight.playerInSight == true && enemySight.targetDistance < attackRange) 
		{
			
		}*/
    }
    void Attack()
    {
        navMeshAgent.ResetPath();
        //animator.SetBool ("Attack", true);

        /*//Hitbox 1
        if (attack1HitFrame == currentSprite)
            attackBox1.gameObject.SetActive(true);
        else
            attackBox1.gameObject.SetActive(false);
        //Hitbox 2
        if (attack2HitFrame == currentSprite)
            attackBox2.gameObject.SetActive(true);
        else
            attackBox2.gameObject.SetActive(false);
        //Hitbox 3
        if (attack3HitFrame == currentSprite)
            attackBox3.gameObject.SetActive(true);
        else
            attackBox3.gameObject.SetActive(false);
        */
    }
}
