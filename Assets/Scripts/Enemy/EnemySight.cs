using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour 
{
	public bool playerInSight; //Check if the player is in sight
	public bool playerOnRight; //boolean to trigger left or right 
	public GameObject target; // Game objec to identify the the target
	public float targetDistance;//target distance

	public GameObject player; //Game object to identify the player
	public bool targetsAvailable;

	private EnemyState enemyState;

	//Variables to to check if the player is on the left or the right of the enemy
	 Vector3 playerRelativePosition; //use to actually calculate left or right

	//Front and back Targets
	 [SerializeField] GameObject frontTarget;
	 [SerializeField] GameObject backTarget;

	//Distance to targes
	 float distanceToFrontTarget;
	 float distanceToBackTarget;

	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player"); //Find Game object tagged as player
        enemyState=GetComponent<EnemyState> ();	
        //Finding Front and Back Targets
        frontTarget =GameObject.Find("AttackPointR");
		backTarget=GameObject.Find("AttackPointL");
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!frontTarget.GetComponent<AttackPoint>().available && !backTarget.GetComponent<AttackPoint>().available)
		{
			targetsAvailable = false;
		}
		else { 
			targetsAvailable=true;
		}
		//Player relative position
		playerRelativePosition = player.transform.position-gameObject.transform.position;//relative position=player position - enemy position

		if(playerRelativePosition.x<0)
		{
			playerOnRight = false;
		}
		else if(playerRelativePosition.x>0)
		{
			playerOnRight = true;
		}

		//Calculating target position
		distanceToFrontTarget=Vector3.Distance(frontTarget.transform.position,gameObject.transform.position);
		distanceToBackTarget=Vector3.Distance(backTarget.transform.position,gameObject.transform.position);
		//compare distances
		if (targetsAvailable)
		{
			if (distanceToFrontTarget < distanceToBackTarget && frontTarget.GetComponent<AttackPoint>().available)
			{
				target = frontTarget;

				//calculat target distance
				targetDistance = Vector3.Distance(target.transform.position, gameObject.transform.position);
			}
			else if (distanceToFrontTarget > distanceToBackTarget && backTarget.GetComponent<AttackPoint>().available)
			{
				target = backTarget;

				//calculat target distance
				targetDistance = Vector3.Distance(target.transform.position, gameObject.transform.position);
			}

		}
		else {
			target = null;
			targetDistance = 0;
        }
		

	}



	//Check to see if the player has entered the sphere with the trigger enabled
	void OnTriggerStay(Collider other)//Checking for the enemy sphere colliders trigger
	{
		if (other.gameObject == player) //Check to see if the player is inside the sphere
		{
			playerInSight = true; //Toggle ON player in sight
		}
	}
	//Check to see if the player has exited the sphere with the trigger enabled
	void OnTriggerExit(Collider other)//Checking for the enemy sphere colliders trigger
	{
		if (other.gameObject == player) //Check to see if the player is outside the sphere
		{
			playerInSight = false; //Toggle OFF player in sight
		}
	}
}
