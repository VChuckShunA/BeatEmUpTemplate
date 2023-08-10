using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour 
{
	/// <summary>
	/// This script handles the UI
	/// It is used by both the enemies and the player
	/// NOTE: it is important to set the starting health to the same health as the Grunt/Enemy health in the inspector for each player/enemy
	/// </summary>
	public float startingHealth;
	public float health;

	public bool displayUI;

	public Slider healthSlider;
	public GameObject healthUI;

	public bool enemyToWin;

	void Awake()
	{
		health = startingHealth;
	}
	void Update()
	{
		//Updating Player HUD
		if(gameObject.tag==("Player"))
		{
			healthUI = GameObject.FindGameObjectWithTag ("PlayerHealthUI");
			healthSlider = healthUI.gameObject.transform.GetChild (0).GetComponent<Slider> ();
			if (healthSlider.maxValue == 0)
				healthSlider.maxValue = startingHealth;
			healthSlider.value = health;
		}

		//Updating Enemy HUD
		if (gameObject.tag == ("Enemy") && displayUI == true) {
			healthUI = GameObject.FindGameObjectWithTag ("EnemyHealthUI");
			healthSlider = healthUI.gameObject.transform.GetChild (0).GetComponent<Slider> ();
			if (healthSlider.maxValue == 0)
				healthSlider.maxValue = startingHealth;
			healthSlider.value = health;
		}

		//Death
		if (health <= 0) 
		{
			Destroy (gameObject.transform.parent.gameObject);
			if(enemyToWin==true) // slow down time if enemyToWin has been killed
			{
				Time.timeScale=.5f;
			}
		}


	}


}
