using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Grunt
{
    /// <summary>
    /// This Script handles the logic for a boss enemy that throws boomerangs
    /// This Script inherits from the Grunt Script, not monobehaviour
    /// </summary>
    public GameObject boomerang;
    public float minBoomerangTime, maxBoomerangTime;


    // Use this for initialization
    void Awake()
    {
        Invoke("ThrowBoomerang", Random.Range(minBoomerangTime, maxBoomerangTime));
    }

    void ThrowBoomerang()
    {
        if (!isDead)
        {
            anim.SetTrigger("Boomerang");
            GameObject tempBoomerang = Instantiate(boomerang, transform.position, transform.rotation); //Instantiating the boomerang
            //Throwing the boomerang in the other direction if not facing right
            if (facingRight)
            {
                tempBoomerang.GetComponent<Boomerang>().direction = 1;
            }
            else
            {
                tempBoomerang.GetComponent<Boomerang>().direction = -1;
            }
            Invoke("ThrowBoomerang", Random.Range(minBoomerangTime, maxBoomerangTime));
        }
    }
}
