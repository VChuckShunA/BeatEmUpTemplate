using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 5;
    public float buff = 0;
    public bool knockDownAttack;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RetreatCollider")
        {
            if (knockDownAttack)
            {
                other.GetComponentInParent<EnemyState>().knockedDown = true;
            }
            else
            {
                other.GetComponentInParent<EnemyState>().TookDamage(damage + buff);
            }
        }
        if (other.tag == "Player")
        {
            if (knockDownAttack)
            {
                other.GetComponent<Player>().knockedDown = true;    
            }
            else
            {
                other.GetComponent<Player>().TookDamage(damage + buff);
            }
           
        }
    }
}
