using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public static Enemy instance;
	[SerializeField] float walkSpeed = 5.0f;
	[SerializeField] float runSpeed = 8.0f;
	[SerializeField] float speed;
	[SerializeField] float maxHealth = 100.0f;
	[SerializeField] float health;
	[SerializeField] Animator anim;
	[SerializeField] Rigidbody rb;
	[SerializeField] private bool isDead = false;
	[SerializeField] private bool facingRight = true;
	[SerializeField] public bool canAttack = true;
	[SerializeField] public bool canMove = true;
	[SerializeField] public bool attacked = false;


	private BoxCollider boxCollider;
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
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Hitbox")) {
			TookDamage(other.gameObject.GetComponent<Hitbox>().damage);
		}
	}
	// Update is called once per frame
	void Update()
    {
        
    }

	public void TookDamage(float damage) {
		if (!isDead) {
			health -= damage;
			if (health <= 0) {
				isDead = true;
				anim.SetBool("Dead", true);
				//FindObjectOfType<UIManager>().UpdateHealth(health);
			} else {
				anim.SetTrigger("HitDamage");
			}
		}
	}
}
