using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpKickChecker : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        { 
            this.GetComponentInParent<JumpEnemyAI>().canAttack=true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GetComponentInParent<JumpEnemyAI>().canAttack = false;
        }
    }

}

