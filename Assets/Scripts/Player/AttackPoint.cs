using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    /// <summary>
    /// This Script is used by the Enemy AI to navigate to the Player's Attack point
    /// The Enemy has a collider called Retreat collider that is used specifically for navigation purposes
    /// </summary>
    [SerializeField] public bool available=true;

    private void OnTriggerEnter(Collider other)
    {
        //If the retreat collider enters the attack point, the attack point becomes unavailable
        if (other.tag=="RetreatCollider")
        {
            available = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If the retreat collider enters the attack point, the attack point becomes available
        if (other.tag == "RetreatCollider")
        {
            available = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the the enemy in the attack point dies, the attackpoint becomes available
        if (other.tag == "RetreatCollider")
        {
            if (other.GetComponentInParent<EnemyState>().isDead)
            {
                available = false;
            }
        }
    }
}
