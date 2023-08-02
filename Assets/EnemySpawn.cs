using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float maxZ, minZ;
    public GameObject[] enemy;
    public int numberOfEnemies;
    public float spawnTime;
    public GameObject leftBorder, rightBorder;

    private int currentEnemies;
    // Start is called before the first frame update
    void Start()
    {
        leftBorder = transform.Find("LeftBoundary").gameObject;
        rightBorder = transform.Find("LeftBoundary").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemies >= numberOfEnemies)
        {
            int enemies = FindObjectsOfType<Grunt>().Length + FindObjectsOfType<Enemy>().Length;
            if (enemies <= 0)
            {
                CameraFollow.isFollowing = true;
                gameObject.SetActive(false);
            }
        }
    }

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
        if (other.CompareTag("Player"))
        {

            CameraFollow.isFollowing = false;
            //FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;
            SpawnEnemy();
        }
    }
}