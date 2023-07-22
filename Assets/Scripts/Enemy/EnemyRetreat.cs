using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRetreat : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    Animator animator;
    public GameObject spriteObject;
    [SerializeField] private Vector3 wanderTarget;
    private float wanderRadius = 5f; // Maximum distance from the player to wander
    EnemySight enemySight;
    EnemyWalk enemyWalk;
    EnemyState enemeyState;
 


    void Awake()
    {

        //Inititilizing Navmesh 
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>(); //Acessing Navmesh Agent
        enemySight = GetComponent<EnemySight>(); //Accessing the EnemySight.cs script

        //Initializing Animator
        animator = spriteObject.GetComponent<Animator>();//accessing the animator
        enemeyState = GetComponent<EnemyState>();
    }


    // Update is called once per frame
    void Update()
    {
        if (enemeyState.currentState == EnemyState.currentStateEnum.retreat)
        {
            Debug.Log("Before");
            StartCoroutine(Retreat());
            Debug.Log("After");
        }
    }

    private Vector3 GetRandomPointAroundPlayer(Vector3 center, float radius)
    {
        // Generate a random point in a circle around the player
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0f;
        Debug.Log("GetRandomPointAroundPlayer");
        return center + randomDirection;
    }

    IEnumerator Retreat()
    {
        Debug.Log("Retreat");
        yield return new WaitForSeconds(0.0f);
        animator.SetBool("Walk", false);
        wanderTarget = GetRandomPointAroundPlayer(this.transform.position, wanderRadius);
        Debug.Log("Wander Target" + wanderTarget);
        Walk(wanderTarget);
    }

    void Walk(Vector3 wanderTarget)
    {
        if (enemySight.playerOnRight == true && enemyWalk.facingRight == true)
        {
            enemyWalk.Flip();
        }
        else if (enemySight.playerOnRight == false && !enemyWalk.facingRight)
        {
            enemyWalk.Flip();
        }
        navMeshAgent.speed = enemyWalk.enemySpeed; //Assign the enemy speed to the navmesh speed
        enemyWalk.enemyCurrentSpeed = navMeshAgent.velocity.sqrMagnitude;
        navMeshAgent.SetDestination(wanderTarget); // Move to the player's location
        navMeshAgent.updateRotation = false; //prevent the enemy sprite from rotating

    }
}
