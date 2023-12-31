using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour 
{
	/// <summary>
	/// This script handles the enemy walk logic when the enemy is moving towards the player
	/// </summary>
	//accesing states
	EnemyState enemeyState;
	//Enemy Speed variables
	public float enemySpeed; //Assign a value here for the movement to work. It's set to 0 by default
	public float enemyCurrentSpeed;


	//Animating Enemy Movement - Variables
	public GameObject spriteObject;//using this to access the enemy's sprite rendered (drag and drop it)
	Animator animator; //using this to acces the animator (Drag and Drop)
	//FLipping
	public bool facingRight = true;

	UnityEngine.AI.NavMeshAgent navMeshAgent; //Initializing our Navmesh Agent
	EnemySight enemySight; //Initializing a variable to access the EnemySight.cs script

	// Use this for initialization
	void Awake () 
	{ 
		
		//Inititilizing Navmesh 
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> (); //Acessing Navmesh Agent
		enemySight = GetComponent<EnemySight> (); //Accessing the EnemySight.cs script

		//Initializing Animator
		animator = spriteObject.GetComponent<Animator>();//accessing the animator

		navMeshAgent.speed = enemySpeed; //Assign the enemy speed to the navmesh speed
		enemeyState =GetComponent<EnemyState> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (enemeyState.currentState == EnemyState.currentStateEnum.walk) {
			if (!enemeyState.isDead && !enemeyState.tookDamage)
			{
                Walk(); //Calling the walk function
            }
		}
		else if (enemeyState.currentState == EnemyState.currentStateEnum.idle) {
			//Stop ();
		}
	

		
	}

	//Walking towards the player
	void Walk()
	{
		//Flipping the sprites
		if (enemySight.playerOnRight == true && facingRight == true) {
			Flip ();
		} else if (enemySight.playerOnRight == false && !facingRight) 
		{
			Flip ();
		} 

		enemyCurrentSpeed=navMeshAgent.velocity.sqrMagnitude;
		if (enemySight.target) // Checking if the enemy sight Script has detected the target
		{

            navMeshAgent.SetDestination(enemySight.target.transform.position); // Move to the player's location
            navMeshAgent.updateRotation = false; //prevent the enemy sprite from rotating

        }
    }

	void Stop()
	{
		navMeshAgent.ResetPath ();
	}

	public void Flip() //--Code for flipping the sprite
	{
		//Alternative Flip code. Comment out facingRight value to initialize this
		//*WARNING! : This code affects the hit box.*
		/*facingRight = !facingRight;
		spriteRenderer.flipX = !spriteRenderer.flipX;
		*/

		facingRight = !facingRight;
		Vector3 thisScale = transform.localScale;
		thisScale.x *= -1;
		transform.localScale = thisScale;
	}

	//This function is called as an animation Event
    public void ZeroSpeed()
    {
        navMeshAgent.speed = 0;
    }

    //This function is called as an animation Event
    public void ResetSpeed()
	{
        navMeshAgent.speed = enemySpeed;
    }
}
