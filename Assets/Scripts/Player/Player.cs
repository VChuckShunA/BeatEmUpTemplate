using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	/// <summary>
	/// This script controls the player
	/// This script requires the Weapon Holder Script to hold Weapons
	/// This script requires the Stat Script to update the UI
	/// This script requires the Attack point script for the Enemy Retreat Script to work
	/// </summary>
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
    [SerializeField]public float knockBackForce;
	public bool nearPickUp;
    public bool knockedDown;
    public float knockedDownTime, minHeight, maxHeight;
    private BoxCollider boxCollider;
    [SerializeField] private bool canPickup=false;
    [SerializeField] private GameObject weaponToPickUp;

	Stats otherStats;
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
		otherStats=GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
		//Alternate GroundCheck
		//onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		//Updating the animator
		anim.SetBool("OnGround", onGround);
		anim.SetBool("Dead", isDead);

		//Jump Input
		if (Input.GetButtonDown("Jump") && onGround) {
			jump = true;
		}

		//Calling Attack
		Attack();

		//Calling Movement
		Movement();

		//knockdown logic
        if (knockedDown == true)
            StartCoroutine(KnockedDown());

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
	//Attack & Weapon Pickup logic
	private void Attack() {
		if (Input.GetButtonDown("Fire1")) {
			//Weapon Pickup logic
			if (canPickup)
            {//TODO: Play Your Weapon Pickup Animation here
                weaponToPickUp.GetComponent<WeaponPickup>().PickupWeapon(this.gameObject,6);
                canAttack = true;
				canPickup = false;
                
			}
			//Attack Logic
			else if (canAttack)
			{
                anim.SetTrigger("Attack1");
            }
        }
	}


	//Movement Logic
	private void Movement()
    {
        if (!isDead) {
			//Taking Input
			float h = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");

			//Ground Check
			if (!onGround) {
				z = 0;
			}
			//Movement
			if (canMove) { rb.velocity = new Vector3(h * speed, rb.velocity.y, z * speed); }

			//Updating  animator
			if (onGround) {
				anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
			}
			//Flipping the sprite
			if (canMove) { 
				if (h > 0 && !facingRight) {
				Flip();
			} else if (h < 0 && facingRight) {
				Flip();
			}
			}
			//Jump Logic
			if (jump) {
				jump = false;
				rb.AddForce(Vector3.up * jumpForce);
			}
			//Camping movement
            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));

        }
    }

	//Flipping Sprites
    private void Flip() {
		facingRight = !facingRight;

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

	}

	//This functions are called as animation events
	//Stop movement while attacking
	private void ZeroSpeed() {
		speed = 0;
	}

    //This functions are called as animation events
    //Reset Speed
    private void ResetSpeed() {
		speed = walkSpeed;
	}

	//Take Damage
	public void TookDamage(float damage) {
		if (!isDead && knockedDown == false) { //Player can only take damage if they're NOT dead and NOT knocked down
			health -= damage;//Redact health
            if (health > 0)
			{
				if (knockedDown)
				{
					//Calling Knockdown
                    StartCoroutine(KnockedDown());
                }
				anim.SetTrigger("Hurt"); //Hurt Animation
				otherStats.health = health; //Updating the UI
			}
			else if (health <= 0)
			{
				anim.SetBool("Dead", true);
				isDead = true;
			}
        }
	}

	//Knockdown Function
    public IEnumerator KnockedDown()
    {
        health -= 30;
        anim.Play("Fall");//skip the animator and directly play the flal animation
        anim.SetBool("Knocked Down", true);
        canMove = false; // prevent the  sprite from flipping when down
		canAttack = false;
		//Getting knocked bakwards
		if (facingRight == false)
		{
			rb.AddForce(transform.right * knockBackForce); //applying a force through the inspector
		}
		else if (facingRight == true)
		{
            rb.AddForce(transform.right * (-1 * knockBackForce));// appying a force of through the inspector
        }
		
        yield return new WaitForSeconds(knockedDownTime); //waiting for two seconds as specified through the inspector

        anim.SetBool("Knocked Down", false);
        canMove = true; //allowing the player to flip
        knockedDown = false;
		canAttack = true;
    }

    private void OnTriggerStay(Collider other)
    {
		//Weapon Pickup Logic
		if (other.GetComponent<WeaponPickup>())
        {
            if (!other.GetComponent<WeaponPickup>().pickedUp)
			{
                canAttack = false;
				canPickup = true;
				weaponToPickUp = other.gameObject;
            }
        }
			
    }

	private void OnTriggerExit(Collider other)
	{
		//Disable Weapon Pickup
		if (other.GetComponent<WeaponPickup>())
		{
			canAttack = true;
			canPickup = false;
		}

	}
}
