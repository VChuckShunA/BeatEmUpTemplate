using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    public GameObject spriteObject;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    EnemySight enemySight;
    EnemyWalk enemyWalk;
    public Animator animator;
    EnemyAttack enemAttack;
    EnemyRetreat enemyRetreat;
    Stats stats; // we are using this to update the enemy health HUD , we only want the enemy we are fighting right nowt to have their health displayed.
    //Player Health
    [SerializeField] FlickerSprite flickerSprite;
    [SerializeField] float maxHealth = 100.0f;
    [SerializeField] float health;
    [SerializeField] Rigidbody rb;

    private BoxCollider boxCollider;
    //flags to call animation states for knocked down
    public bool tookDamage;
    public bool knockedDown;
    public float stunTime;
    public float knockDownTime;
    public static EnemyState instance;

    //Booleans for retreat logic

    public bool isRetreating = false;
    public bool canSpawnRetreatPoint = true;
    public int retreatCounter = 0;
    public float damageCount = 3;
    [SerializeField] public bool isDead = false;
    public GameObject retreatObject;
    //Variables for the state machine
    public enum currentStateEnum { idle = 0, walk = 1, attack = 2, retreat=3,hurt=4,dead=5 };
    public currentStateEnum currentState;
    [SerializeField] private Vector3 wanderTarget;
    [SerializeField] public GameObject retreatPoint; 
    private float wanderRadius = 8f; // Maximum distance from the player to wander
    //--Annimator State Variables------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    AnimatorStateInfo currentStateInfo;
    static int currentAnimationState;
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int walkState = Animator.StringToHash("Base Layer.Walk");
    static int attack1State = Animator.StringToHash("Base Layer.Attack1");
    static int attack2State = Animator.StringToHash("Base Layer.Attack2");
    static int attack3State = Animator.StringToHash("Base Layer.Attack3");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int blockState = Animator.StringToHash("Base Layer.Block");
    static int projectileState = Animator.StringToHash("Base Layer.Projectile");
    static int hurtState = Animator.StringToHash("Base Layer.Hurt");


    // Use this for initialization
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
        enemyWalk = GetComponent<EnemyWalk>();
        enemAttack = GetComponent<EnemyAttack>();
        enemyRetreat = GetComponent<EnemyRetreat>();
        animator = spriteObject.GetComponent<Animator>();
        instance=this;

        health = Mathf.Clamp(maxHealth, 0, maxHealth);
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        FlickerSprite flickerSprite = GetComponent<FlickerSprite>();
        //stats = GetComponent<Stats>();

    }

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       /* Debug.Log("Condition 1");
        Debug.Log(
        tookDamage == false);
        Debug.Log("Condition 2");
        Debug.Log(
                enemySight.playerInSight == true);
        Debug.Log("Condition 3");
        Debug.Log(
                damageCount != 0);
        Debug.Log("Condition 4");
        Debug.Log(
                enemySight.targetDistance < enemAttack.attackRange);
        Debug.Log("Condition 5");
        Debug.Log(
                navMeshAgent.velocity.sqrMagnitude < enemAttack.atackStartDelay);*/
        if (!isDead)
        {
            //Get knocked down
            if (knockedDown == true && tookDamage == false)
            {
                // stats.displayUI = true;
                animator.SetBool("Knocked Down", true);
                StartCoroutine(KnockedDown());
            }
            //take Damange
            else if (tookDamage == true)
            {
                //stats.displayUI = true;
                //stats.displayUI = true;
            }
            //attack logic
            else if (tookDamage == false &&
                enemySight.playerInSight == true &&
                damageCount > 0 &&
                enemySight.player.GetComponent<Player>().knockedDown == false &&
                enemySight.targetDistance < enemAttack.attackRange &&
                navMeshAgent.velocity.sqrMagnitude < enemAttack.atackStartDelay &&
                enemySight.target
                )
            {
                
                    StartCoroutine(AttackDelay());
            }
            //Retreat logic
            else if (damageCount == 0 && retreatCounter != 2 && !isRetreating)
            {
                enemyRetreat.StartRetreating();
            }
            //walk-animation logic
            else if (knockedDown == false &&
                tookDamage == false &&
                enemySight.playerInSight == true)
            {
                // stats.displayUI = false;
                animator.SetBool("Walk", true);
                animator.SetBool("Attack", false);
            }
            //transfer-back-to-idle-animation logic
            else if (tookDamage == false && enemySight.playerInSight == false)
            {
                //stats.displayUI = false;
                animator.SetBool("Walk", false);
                animator.SetBool("Attack", false);
            }
        }
        else
        {
            navMeshAgent.ResetPath();
            animator.SetBool("Walk",false);
            // animator.SetBool("Dead", true);
            animator.Play("Death");
            rb.constraints = RigidbodyConstraints.FreezePosition;
            boxCollider.enabled = false;
            flickerSprite.StartFlickering();
            //FindObjectOfType<UIManager>().UpdateHealth(health);
        }


        //State machine logic
        if (damageCount == 0)
        {
            currentState = currentStateEnum.retreat;
        }
        else if (currentAnimationState == idleState)
        {
            currentState = currentStateEnum.idle;
        }
        else if (currentAnimationState == walkState)
        {
            currentState = currentStateEnum.walk;
        }
        else if (currentAnimationState == attack1State || currentAnimationState == attack2State || currentAnimationState == attack3State)
        {
            currentState = currentStateEnum.attack;
        }
        else if (currentAnimationState == hurtState)
        {
            currentState = currentStateEnum.hurt;
        }

        //accessing animation state
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        currentAnimationState = currentStateInfo.fullPathHash;

        //Retreat Logic
        if(retreatCounter==2)
        {
            retreatCounter = 0;
            damageCount = 3;
            isRetreating = false;
        }

    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0);
        //stats.displayUI = false;
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", true);
    }
    
   
    /// <summary>
    /// Knockdown Logic
    /// </summary>
    /// <returns></returns>

    IEnumerator KnockedDown()
    {
        animator.Play("Fall");
        //Getting knocked bakwards
        if (enemyWalk.facingRight == false)
        {
            rb.AddForce(transform.right * 2); //applying a force through the inspector
            Debug.Log("applied force to enemy");
        }
        else if (enemyWalk.facingRight == true)
        {
            rb.AddForce(transform.right * (-1 * 2));// appying a force of through the inspector

            Debug.Log("applied force to enemy");
        }
        yield return new WaitForSeconds(knockDownTime);
        animator.SetBool("Knocked Down", false);
        knockedDown = false;
    }

   

    ///Damnage Logic <summary>
    /// Damnage Logic

    public void DecrementDamageCounter()
    {
        damageCount--;
    }
    public void TookDamage(float damage)
    {
        if (!isDead)
        {
            tookDamage = true;
            navMeshAgent.isStopped = true;
            health -= damage;
            StartCoroutine(DamageTimer());
            if (health <= 0)
            {
                isDead = true;
            }
            else
            {
                animator.SetTrigger("HitDamage");

                //animator.SetBool("Is Hit", false);
                tookDamage = false;
                navMeshAgent.isStopped = false;
            }
        }

       
    }

    IEnumerator DamageTimer() {

        yield return new WaitForSeconds(stunTime);
}
     
}