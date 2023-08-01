using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour {
	public GameObject enemyPrefab;

	public bool lastEnemy =false;

	void Awake()
	{
		enemyPrefab.SetActive (false);
	}


	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		if(other.tag=="EnemySpawnTrigger")
		{
			enemyPrefab.SetActive (true);
			if (lastEnemy == true) 
			{
				Stats enemyStats = enemyPrefab.GetComponent<Stats> ();
				enemyStats.enemyToWin = true;
			}
		}
	}
}
