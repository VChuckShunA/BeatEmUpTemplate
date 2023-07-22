using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    public GameObject spriteObject;
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    EnemySight enemySight;
    EnemyWalk enemyWalk;
    Animator animator;
    EnemyAttack enemAttack;
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
    //Variables for the state machine
    public enum currentStateEnum { idle = 0, walk = 1, attack = 2, retreat=3,hurt=4 };
    public currentStateEnum currentState;
    [SerializeField] private Vector3 wanderTarget;
    [SerializeField] private GameObject retreatPoint; 
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
                damageCount != 0 &&
                //enemySight.player.GetComponent<PlayerControllerScript>().knockedDown == false &&
                enemySight.targetDistance < enemAttack.attackRange &&
                navMeshAgent.velocity.sqrMagnitude < enemAttack.atackStartDelay)
            {
                //stats.displayUI = false;
                animator.SetBool("Walk", false);
                animator.SetBool("Attack", true);
            }
            //Retreat logic
            else if (damageCount == 0 && retreatCounter != 4 && !isRetreating)
            {
                animator.SetBool("Attack", false);
                isRetreating = true;
                StartCoroutine(Retreat());
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
        else if (currentAnimationState == attack1State)
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
        if(retreatCounter==4)
        {
            retreatCounter = 0;
            damageCount = 3;
            isRetreating = false;
        }

    }

    IEnumerator KnockedDown()
    {
        animator.Play("Fall");

        yield return new WaitForSeconds(knockDownTime);
        animator.SetBool("Knocked Down", false);
        knockedDown = false;
    }

    private Vector3 GetRandomPointAroundPlayer(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0f;
        Vector3 randomPoint = center + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If no valid position is found, return the original center position
        return center;
    }

    IEnumerator Retreat()
    {
        yield return new WaitForSeconds(0.0f);
        animator.SetBool("Walk", false);
        wanderTarget = GetRandomPointAroundPlayer(enemySight.player.transform.position, wanderRadius);
        
        Walk(wanderTarget);
    }

    void Walk(Vector3 wanderTarget)
    {
        if (canSpawnRetreatPoint)
        {
            Instantiate(retreatPoint, new(wanderTarget.x, -2, wanderTarget.z), Quaternion.identity);
            canSpawnRetreatPoint = false;
        }
        if (enemySight.playerOnRight == true && enemyWalk.facingRight == true)
        {
            enemyWalk.Flip();
        }
        else if (enemySight.playerOnRight == false && !enemyWalk.facingRight)
        {
            enemyWalk.Flip();
        }
        Debug.Log("Walk");
        navMeshAgent.speed = enemyWalk.enemySpeed; //Assign the enemy speed to the navmesh speed
        enemyWalk.enemyCurrentSpeed = navMeshAgent.velocity.sqrMagnitude;
        navMeshAgent.SetDestination(wanderTarget); // Move to the player's location
        navMeshAgent.updateRotation = false; //prevent the enemy sprite from rotating
       

    }

    ///Damnage Logic <summary>
    /// Damnage Logic
    IEnumerator TookDamage(float damage)
    {
        if (!isDead)
        {
            navMeshAgent.speed = 0;
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                animator.SetBool("Dead", true);
                rb.constraints = RigidbodyConstraints.FreezePosition;
                boxCollider.enabled = false;
                flickerSprite.StartFlickering();
                //FindObjectOfType<UIManager>().UpdateHealth(health);
            }
            else
            {
                yield return new WaitForSeconds(stunTime);
                animator.SetBool("Is Hit", false);
                tookDamage = false;
                animator.SetTrigger("HitDamage");
            }
        }

       
    }
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hitbox"))
        {
            StartCoroutine(TookDamage(other.gameObject.GetComponent<Hitbox>().damage));
        }

    }

    
}