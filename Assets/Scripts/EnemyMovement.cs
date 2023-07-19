using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
	public Transform player;
	public Transform attackPointR;
	public Transform attackPointL;
	public float attackRange = 2f;
	public float retreatRange = 5f;
	public float circleDistance = 4f;
	public float circleSpeed = 3f;

	private NavMeshAgent navMeshAgent;
	private Transform targetAttackPoint;
	private bool isAttacking;
	private bool isRetreating;
	private bool isCircling;
	private float originalSpeed;


	[SerializeField] public Animator anim;
	[SerializeField] public int attackSequence = 0;

	private enum EnemyState {
		Approach,
		Attack,
		Retreat,
		Circle
	}

	private EnemyState currentState;

	private void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		targetAttackPoint = attackPointR;
		originalSpeed = navMeshAgent.speed;
		currentState = EnemyState.Approach;

		player = GameObject.FindWithTag("Player").transform;
		attackPointR = player.transform.Find("AttackPointR").transform;
		attackPointL = player.transform.Find("AttackPointL").transform;
		navMeshAgent.updateRotation = false;
		// Set the initial target attack point to the right attack point
		targetAttackPoint = attackPointR;
	}

	private void Update() {
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		// FSM behavior
		switch (currentState) {
			case EnemyState.Approach:
				if (distanceToPlayer <= attackRange) {
					Attack();
				} else {
					MoveToPlayer();
				}
				break;

			case EnemyState.Attack:
				if (distanceToPlayer > attackRange) {
					currentState = EnemyState.Approach;
				}
				break;

			case EnemyState.Retreat:
				if (distanceToPlayer > retreatRange) {
					currentState = EnemyState.Approach;
				} else {
					Retreat();
				}
				break;

			case EnemyState.Circle:
				CirclePlayer();
				break;
		}
	}

	private void MoveToPlayer() {
		float distanceToR = Vector3.Distance(transform.position, attackPointR.position);
		float distanceToL = Vector3.Distance(transform.position, attackPointL.position);
		targetAttackPoint = (distanceToR < distanceToL) ? attackPointR : attackPointL;

		// Set the NavMeshAgent's destination to the target attack point
		navMeshAgent.SetDestination(targetAttackPoint.position);

		if (Vector3.Distance(transform.position, player.position) <= retreatRange) {
			currentState = EnemyState.Retreat;
		}
	}

	private void Attack() {
		// Implement your attack logic here

		attackSequence = Random.Range(1, 4); ;
		switch(attackSequence){
			case 1:
				anim.SetTrigger("Sequence1");
				break;
			case 2:
				anim.SetTrigger("Sequence2");
				break;
			case 3:
				anim.SetTrigger("Sequence3");
				break;
			default:
				// code block
				break;
		}
		currentState = EnemyState.Circle;
	}

	private void Retreat() {
		isAttacking = false;
		isRetreating = true;
		navMeshAgent.speed = originalSpeed;
		navMeshAgent.SetDestination(transform.position - (player.position - transform.position).normalized * retreatRange);
		currentState = EnemyState.Approach;
	}

	private void CirclePlayer() {
		if (!isCircling) {
			Vector3 circleDir = (player.position - transform.position).normalized;
			Vector3 circlePoint = player.position + Quaternion.Euler(0f, 90f, 0f) * circleDir * circleDistance;
			navMeshAgent.speed = circleSpeed;
			navMeshAgent.SetDestination(circlePoint);
			isCircling = true;
		}

		float distanceToCirclePoint = Vector3.Distance(transform.position, navMeshAgent.destination);
		if (distanceToCirclePoint < 1f) {
			isCircling = false;
			currentState = EnemyState.Approach;
		}
	}
}
