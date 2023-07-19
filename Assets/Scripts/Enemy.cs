using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

	public static Enemy instance;
	[SerializeField] FlickerSprite flickerSprite;
	[SerializeField] Transform player;
	[SerializeField] Transform attackPointR;
	[SerializeField] Transform attackPointL;
	[SerializeField] Transform targetAttackPoint;
	[SerializeField] NavMeshAgent navMeshAgent;
	[SerializeField] float walkSpeed = 5.0f;
	[SerializeField] float runSpeed = 8.0f;
	[SerializeField] float speed;
	[SerializeField] float maxHealth = 100.0f;
	[SerializeField] float health;
	[SerializeField] public Animator anim;
	[SerializeField] Rigidbody rb;
	[SerializeField] private bool isDead = false;
	[SerializeField] private bool facingRight = false;
	[SerializeField] public int retreatCount = 4;
	[SerializeField] public bool canAttack = false;
	[SerializeField] public bool canMove = true;
	[SerializeField] public bool attacked = false;
	[SerializeField] public int attackSequence = 0;


	private BoxCollider boxCollider;
	private float wanderRadius = 5f; // Maximum distance from the player to wander
	private float wanderInterval = 1f; // Time interval between each wander movement
	private float stoppingDistance = 1f; // The distance at which the agent should stop
	private Vector3 wanderTarget;
	private float lastWanderTime;
	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		boxCollider = GetComponent<BoxCollider>();
		speed = walkSpeed;
		health = Mathf.Clamp(maxHealth, 0, maxHealth);
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		FlickerSprite flickerSprite = GetComponent<FlickerSprite>();
		player = GameObject.FindWithTag("Player").transform;
		attackPointR = player.transform.Find("AttackPointR").transform;
		attackPointL = player.transform.Find("AttackPointL").transform;
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.updateRotation=false;
		// Set the initial target attack point to the right attack point
		targetAttackPoint = attackPointR;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Hitbox")) {
			TookDamage(other.gameObject.GetComponent<Hitbox>().damage);
		}
		
	}
	// Update is called once per frame
	void Update()
    {
		// Check if the player is within the enemy's range
		if (Vector3.Distance(transform.position, player.position) < 10f) {
			if (canMove) {
				MoveToPlayer();
			}
		}

	}

	public void TookDamage(float damage) {
		if (!isDead) {
			health -= damage;
			if (health <= 0) {
				isDead = true;
				anim.SetBool("Dead", true);
				rb.constraints = RigidbodyConstraints.FreezePosition;
				boxCollider.enabled=false;
				flickerSprite.StartFlickering();
				//FindObjectOfType<UIManager>().UpdateHealth(health);
			} else {
				anim.SetTrigger("HitDamage");
			}
		}
	}

	private void Flip() {
		facingRight = !facingRight;

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

	}

	private void Attack() {
		if (canAttack) {
			attackSequence = Random.Range(1, 4); ;
			anim.SetBool("CanAttack", canAttack);
			anim.SetInteger("Sequence", attackSequence);
		} else {
			MoveToPlayer();
		}

	}
	private Vector3 GetRandomPointAroundPlayer(Vector3 center, float radius) {
		// Generate a random point in a circle around the player
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection.y = 0f;
		return center + randomDirection;
	}

	public void Retreat() {
		if (retreatCount > 0) {
			if (Time.time - lastWanderTime >= wanderInterval) {
				wanderTarget = GetRandomPointAroundPlayer(player.position, wanderRadius);
				lastWanderTime = Time.time;
			}

			StartCoroutine(RetreatCoroutine());
		} 
		else {
			canAttack = false;
			canMove = true;
		}
	}

	private IEnumerator RetreatCoroutine() {
		retreatCount--;

		while (Vector3.Distance(transform.position, wanderTarget) > stoppingDistance) {
			navMeshAgent.SetDestination(wanderTarget);
			yield return null;
		}

		if (retreatCount > 0) {
			wanderTarget = GetRandomPointAroundPlayer(player.position, wanderRadius);
			yield return new WaitForSeconds(wanderInterval);
			StartCoroutine(RetreatCoroutine());
		}
	}
	private void MoveToPlayer() {
				
			// Update the target attack point based on the player's position
			float distanceToR = Vector3.Distance(transform.position, attackPointR.position);
			float distanceToL = Vector3.Distance(transform.position, attackPointL.position);
			targetAttackPoint = (distanceToR < distanceToL) ? attackPointR : attackPointL;

			// Set the NavMeshAgent's destination to the target attack point
			navMeshAgent.SetDestination(targetAttackPoint.position);

			if (!navMeshAgent.pathPending) {
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
				if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
					canMove = false;
					canAttack=true;
					Attack();
				}
			}
			}

		// Check if the player is to the left or right of the enemy
		float enemyToPlayer = player.position.x - transform.position.x;

			if (enemyToPlayer > 0f) {
				// Player is to the right of the enemy
				if (facingRight) {
					Flip();
				}
			} else if (enemyToPlayer < 0f) {
				// Player is to the left of the enemy
				if (!facingRight) {
					Flip();
				}
			}
	}
}
