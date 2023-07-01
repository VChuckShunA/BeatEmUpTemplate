using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player instance;
    [SerializeField] float walkSpeed=5.0f;
    [SerializeField] float runSpeed=8.0f;
    [SerializeField] float speed;
	[SerializeField] float jumpForce = 400.0f;
	[SerializeField] float maxHealth = 100.0f;
	[SerializeField] float health;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;
	[SerializeField] private bool onGround;
	[SerializeField] private bool isDead = false;
	[SerializeField] private bool facingRight = true;
	[SerializeField] private bool jump = false;
	[SerializeField] public bool canAttack = true;
	[SerializeField] public bool canMove = true;
	[SerializeField] public bool attacked = false;
	[SerializeField] private Transform groundCheck; // Make sure that the Groundcheck object is slightly below the player (about -0.37)

	private BoxCollider boxCollider;
	private void Awake() {
		instance = this;
	}
	// Start is called before the first frame update
	void Start()
    {
		boxCollider = GetComponent<BoxCollider>();
		speed = walkSpeed;
		health = Mathf.Clamp(maxHealth,0,maxHealth);
        anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
		
		//onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		anim.SetBool("OnGround", onGround);
		anim.SetBool("Dead", isDead);

		if (Input.GetButtonDown("Jump") && onGround) {
			jump = true;
		}

		Attack();

		Movement();
	}


	private void OnCollisionEnter(Collision collision) {
		// Check if the collision is with an object tagged as "Ground"
		if (collision.gameObject.CompareTag("Ground")) {
			onGround = true;
			canMove = true;
		}
	}

	private void OnCollisionExit(Collision collision) {
		// Check if the collision with the tagged object has ended
		if (collision.gameObject.CompareTag("Ground")) {
			onGround = false;
			canMove=false;
		}
	}
	private void Attack() {
		if (Input.GetButtonDown("Fire1")) {
			if (canAttack) {
				canAttack = false;
				attacked = true;
			} else {

				return;
			}
		}
	}

	public void InputManager() {
		if (!canAttack) {
			canAttack = true;
		} else { 
			canAttack=false;
		}
	}
	private void Movement() {
		if (!isDead) {
			float h = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");

			if (!onGround) {
				z = 0;
			}
			if (canMove) { rb.velocity = new Vector3(h * speed, rb.velocity.y, z * speed); }

			if (onGround) {
				anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
			}

			if (canMove) { 
				if (h > 0 && !facingRight) {
				Flip();
			} else if (h < 0 && facingRight) {
				Flip();
			}
			}
			if (jump) {
				jump = false;
				rb.AddForce(Vector3.up * jumpForce);
			}

		}
	}

	private void Flip() {
		facingRight = !facingRight;

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

	}

	private void ZeroSpeed() {
		speed = 0;
	}

	private void ResetSpeed() {
		speed = walkSpeed;
	}

	public void TookDamage(int damage) {
		if (!isDead) {
			health -= damage;
			anim.SetTrigger("HitDamage");
			//FindObjectOfType<UIManager>().UpdateHealth(health);
		}
	}
}