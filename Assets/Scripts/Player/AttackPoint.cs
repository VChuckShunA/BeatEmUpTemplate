using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    [SerializeField] public bool available=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="RetreatCollider")
        {
            available = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RetreatCollider")
        {
            available = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "RetreatCollider")
        {
            if (other.GetComponentInParent<EnemyState>().isDead)
            {
                available = false;
            }
        }
    }
}
