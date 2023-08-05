using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    /// <summary>
    /// This script handles the enemy spawn system
    /// </summary>
    public float maxZ, minZ; // the  boundaries from which the enemies will spawn
    public GameObject[] enemy;
    public int numberOfEnemies;
    public float spawnTime;

    private int currentEnemies;

    void Update()
    {
        //if all the spawn enemies have been defeated, the camera will start following the player again
        if (currentEnemies >= numberOfEnemies)
        {
            int enemies = FindObjectsOfType<Grunt>().Length+ FindObjectsOfType<EnemyState>().Length;
            Debug.Log(enemies);
            if (enemies <= 0)
            {
                CameraFollow.isFollowing = true;
                gameObject.SetActive(false);
            }
        }
    }

    //Enemy spawn logic
    void SpawnEnemy()
    {
        bool positionX = Random.Range(0, 2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);
        if (positionX)
        {
            spawnPosition = new Vector3(transform.position.x + 10, 0, spawnPosition.z);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x - 10, 0, spawnPosition.z);
        }
        Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPosition, Quaternion.identity);
        currentEnemies++;
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Spawn enemies if the player collides with this object
        if (other.CompareTag("Player"))
        {

            CameraFollow.isFollowing = false;
            SpawnEnemy();
        }
    }
}
