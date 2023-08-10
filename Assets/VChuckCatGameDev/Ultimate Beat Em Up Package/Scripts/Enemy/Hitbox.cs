using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    /// <summary>
    /// This script handles Attack Collisions for both Player and Enemies
    /// </summary>
    public float damage = 5;
    public float buff = 0;
    public bool knockDownAttack; //used to determine if the attack can knock back the player/enemy
    private void OnTriggerEnter(Collider other)
    {
        //Attacking the Enemy
        if (other.tag == "RetreatCollider")
        {
            if (this.gameObject.transform.parent.tag != "Enemy")//This check is to avoid friendyly fire between enemies
            {
                if (knockDownAttack) //apply knock down if the attack is a knockdown Attack
                {
                    other.GetComponentInParent<EnemyState>().knockedDown = true;
                }
                else
                {
                    other.GetComponentInParent<EnemyState>().TookDamage(damage + buff); //Apply Damage to the Enemy
                }
            }
            
        }
        //Attacking the Grunt
        if (other.GetComponent<Grunt>())
        {
            other.GetComponent<Grunt>().TookDamage(damage+buff); //Apply Damage to the grunt
        }
        //Attacking the Player
        if (other.tag == "Player")
        {
            if (knockDownAttack)//apply knock down if the attack is a knockdown Attack
            {
                other.GetComponent<Player>().knockedDown = true;    
            }
            else
            {
                other.GetComponent<Player>().TookDamage(damage + buff); //Apply Damage to the Player
            }
           
        }
    }
}
