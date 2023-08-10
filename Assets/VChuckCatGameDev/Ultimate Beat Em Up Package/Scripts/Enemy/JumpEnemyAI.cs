using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class JumpEnemyAI : MonoBehaviour
{
    public Transform player, groundCheck;
    public GameObject jumpKickChecker;
    public float jumpDistance = 5.0f;
    public float jumpForce = 400.0f;
    public bool onGround;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    float distanceToPlayer;
    private bool isJumping = false;
    public bool facingRight = true;
    bool playerOnRight;
    public bool canAttack = false;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        animator.SetBool("OnGround", onGround);
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //Flipping the sprites
        if (player.position.x < this.transform.position.x)
        {
            playerOnRight = true;
        }
        else if (player.position.x > this.transform.position.x)
        {
            playerOnRight = false;
        }
        if (playerOnRight && !facingRight)
        {
            Flip();
        }
        else if (!playerOnRight && facingRight)
        {
            Flip();
        }
        //Jump Attack
        if (Mathf.Abs(distanceToPlayer) <= jumpDistance && !isJumping)
        {
            // Enemy is close enough to jump at the player
            Jump();
        }
        if (canAttack&&!onGround&&isJumping)
        {
            animator.SetTrigger("Attack1");
        }
    }

    private void Jump()
    {
        // Disable NavMeshAgent and start jump animation
        navMeshAgent.enabled = false;
        animator.SetTrigger("Jump");

        // Apply a force to the enemy to simulate the jump
        Rigidbody rb = GetComponent<Rigidbody>();
        if (facingRight)
        {
            rb.AddForce(new Vector2(distanceToPlayer*-1, jumpDistance), ForceMode.Impulse);
        }
        else if (!facingRight)
        {
            rb.AddForce(new Vector2(distanceToPlayer, jumpDistance), ForceMode.Impulse);
        }

        isJumping = true;
        
        // Call a function to re-enable NavMeshAgent after the jump animation
        StartCoroutine(EnableNavMeshAgentAfterJump());
    }

    IEnumerator EnableNavMeshAgentAfterJump()
    {
        // Wait for the jump animation to finish
        yield return new WaitForSeconds(2f);/* Duration of jump animation */

        // Re-enable NavMeshAgent and reset jump flag
        navMeshAgent.enabled = true;
        isJumping = false;
    }

    public void Flip() //--Code for flipping the sprite
    {
        

        facingRight = !facingRight;
        Vector3 thisScale = transform.localScale;
        thisScale.x *= -1;
        transform.localScale = thisScale;
    }
}
