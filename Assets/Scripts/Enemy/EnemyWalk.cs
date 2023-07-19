using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{
    [SerializeField] public float enemySpeed;
    [SerializeField] public float enemyCurrentSpeed;
    [SerializeField] public GameObject spriteObject;

	[SerializeField] private Animator animator;
	private NavMeshAgent navmeshAgent;
    private EnemySight enemySight;
    // Start is called before the first frame update
    void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
        animator=GetComponent<Animator>();

		navmeshAgent.updateRotation = false;
        navmeshAgent.speed = enemySpeed;
	}

    // Update is called once per frame
    void Update()
    {
        if (enemySight.playerInSight) {
            navmeshAgent.SetDestination(enemySight.target.transform.position);
            animator.SetBool("Walk", true);
                }
    }
}
