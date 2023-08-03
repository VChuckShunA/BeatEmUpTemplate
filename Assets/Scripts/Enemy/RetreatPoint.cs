using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatPoint : MonoBehaviour
{
    /// <summary>
    /// This Script helps with the enemy retreat cycle
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag== "RetreatCollider") // Checks if the enemy has arrived at the retreat point
        {
            if (other.GetComponentInParent<EnemyState>().isRetreating) //Checks if the enemy is in the retreat state
            {
                if(other.GetComponentInParent<EnemyState>().retreatObject==this.gameObject) //check if the enemy's retreatpoint is the same as this retreat point. This is to avoid confusion with multiple enemies
                {
                    other.GetComponentInParent<EnemyState>().isRetreating = false; //Set is retreating to false
                    other.GetComponentInParent<EnemyState>().canSpawnRetreatPoint = true; //allows spawning another retreat point
                    other.GetComponentInParent<EnemyState>().retreatCounter++; //Increment retreat counter
                    Destroy(gameObject); //Destroy self, allowing the enemy to retreat to another point
                }
            }
        }
    }
}
