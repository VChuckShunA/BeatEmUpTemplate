using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 5;
    public float buff = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RetreatCollider")
        { 
            other.GetComponentInParent<EnemyState>().TookDamage(damage+buff);
        }
    }
}
