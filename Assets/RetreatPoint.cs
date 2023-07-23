using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag== "RetreatCollider")
        {
            if (other.GetComponentInParent<EnemyState>().isRetreating)
            {
                if(other.GetComponentInParent<EnemyState>().retreatObject==this.gameObject)
                {
                    other.GetComponentInParent<EnemyState>().isRetreating = false;
                    other.GetComponentInParent<EnemyState>().canSpawnRetreatPoint = true;
                    other.GetComponentInParent<EnemyState>().retreatCounter++;
                    Destroy(gameObject);
                }
            }
        }
    }
}
