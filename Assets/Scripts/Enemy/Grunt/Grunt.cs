using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Grunt : MonoBehaviour
{
    public float maxSpeed;
    public float minHeight, maxHeight;
    public float damageTime = 0.5f;
    public int maxHealth;
    public float attackRate = 1f;
   

    private float currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    protected Animator anim;
    private Transform groundCheck;
    private bool onGround;
    protected bool facingRight = false;
    private Transform target;
    protected bool isDead = false;
    private float zForce;
    private float walkTimer;
    private bool damaged = false;
    private float damageTimer;
    private float nextAttack;

    Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<Player>().transform;
        currentHealth = maxHealth;

        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //anim.SetBool("Grounded", onGround);
        anim.SetBool("Dead", isDead);

        if (!isDead)
        {
            facingRight = (target.position.x < transform.position.x) ? false : true;
            if (facingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }


        if (damaged && !isDead)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                damaged = false;
                damageTimer = 0;
            }
        }

        walkTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 targetDitance = target.position - transform.position;
            float hForce = targetDitance.x / Mathf.Abs(targetDitance.x);

            if (walkTimer >= Random.Range(1f, 2f))
            {
                zForce = Random.Range(-1, 2);
                walkTimer = 0;
            }

            if (Mathf.Abs(targetDitance.x) < 1.5f)
            {
                hForce = 0;
            }

            if (!damaged)
                rb.velocity = new Vector3(hForce * currentSpeed, 0, zForce * currentSpeed);

            if (Mathf.Abs(currentSpeed) > 0)
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
            //anim.SetFloat("Speed", Mathf.Abs(currentSpeed));

            if (Mathf.Abs(targetDitance.x) < 1.5f && Mathf.Abs(targetDitance.z) < 1.5f && Time.time > nextAttack)
            {
                anim.SetTrigger("GruntAttack");
                currentSpeed = 0;
                nextAttack = Time.time + attackRate;
            }
        }

        rb.position = new Vector3
            (
                rb.position.x,
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));
    }

    public void TookDamage(float damage)
    {
        if (!isDead)
        {
            damaged = true;
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");
            stats.displayUI = true;
            stats.health = currentHealth;
            if (currentHealth <= 0)
            {
                isDead = true;
                rb.AddRelativeForce(new Vector3(3, 5, 0), ForceMode.Impulse);
            }
        }
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }

}
