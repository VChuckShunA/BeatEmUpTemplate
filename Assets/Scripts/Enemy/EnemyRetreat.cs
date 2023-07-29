using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRetreat : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    Animator animator;
    [SerializeField] private Vector3 wanderTarget;
    private float wanderRadius = 5f; // Maximum distance from the player to wander
    EnemySight enemySight;
    EnemyWalk enemyWalk;
    EnemyState enemeyState;
    WeaponScanner weaponScanner;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemeyState = GetComponent<EnemyState>();
        enemySight = GetComponent<EnemySight>();
        enemyWalk = GetComponent<EnemyWalk>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        weaponScanner= GetComponent<WeaponScanner>();
    }

    /// <summary>
    /// Retreat Logic
    /// </summary>
    public void StartRetreating()
    {
        animator.SetBool("Attack", false);
        enemeyState.isRetreating = true;
        StartCoroutine(Retreat());
    }
    private Vector3 GetRandomPointAroundPlayer(Vector3 center, float radius)
    {
        if (weaponScanner.closestWeapon)
        {
            //Return the location of the closest weapon. The location has been adjust here to account for the offset
            Vector3 weaponPosition = weaponScanner.closestWeapon.transform.position - weaponScanner.closestWeapon.transform.GetChild(0).GetComponent<BoxCollider>().center;
            return weaponPosition;
            //return new Vector3 (weaponScanner.closestWeapon.transform.position.x - 2.984165f, weaponScanner.closestWeapon.transform.position.y + 1.994796f, weaponScanner.closestWeapon.transform.position.z+ 0.01569921f);
        }
        else {
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
        
    }

    IEnumerator Retreat()
    {
        animator.SetBool("Walk", false);
        enemeyState.canWalk = false;
        yield return new WaitForSeconds(0.5f);
        enemeyState.canWalk = true;
        wanderTarget = GetRandomPointAroundPlayer(enemySight.player.transform.position, wanderRadius);
        Collider[] colliders = Physics.OverlapSphere(wanderTarget, 3f); // Adjust the radius as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("RetreatPoint"))
            {
                // Retreat point found nearby, get a new random target
                wanderTarget = GetRandomPointAroundPlayer(enemySight.player.transform.position, wanderRadius);
            }
        }
        Walk(wanderTarget);
    }

    void Walk(Vector3 wanderTarget)
    {
        if (enemeyState.canSpawnRetreatPoint)
        {
            enemeyState.retreatObject = Instantiate(enemeyState.retreatPoint, new(wanderTarget.x, -2, wanderTarget.z), Quaternion.identity);
            enemeyState.canSpawnRetreatPoint = false;
        }
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
