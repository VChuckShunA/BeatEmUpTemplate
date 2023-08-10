using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAttack : MonoBehaviour
{
    /// <summary>
    /// This script handles attack collisions for Grunt
    /// </summary>
    public int damage;
 
    private void OnTriggerEnter(Collider other)
    {
        Grunt enemy = other.GetComponent<Grunt>();
        Player player = other.GetComponent<Player>();
        //Friendly fire
        if (enemy != null)
        {
            enemy.TookDamage(damage);
        }
        //Damaging player
        if (player != null)
        {
            player.TookDamage(damage);
        }
    }

}
