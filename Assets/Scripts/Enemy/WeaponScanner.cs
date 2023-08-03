using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponScanner : MonoBehaviour
{
    /// <summary>
    /// This script is used by the Enemy AI to scan for the weapons in the game
    /// </summary>
    [SerializeField] private List<GameObject> weaponsInRange;
    [SerializeField] public GameObject closestWeapon;
    

    void Update()
    {
        //Update Weapons in Range
        weaponsInRange.RemoveAll(g => g.GetComponentInChildren<WeaponPickup>().pickedUp);
        //get the distance to the closest weapon and assign that to closestWeapon
        float minDistance = float.MaxValue;
        closestWeapon = null;

        //Choosing the closest weapon from all the available weapons
        foreach (GameObject weapon in weaponsInRange)
        {
            float distance = Vector3.Distance(weapon.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestWeapon = weapon;
            }
        }
        //removing the closest weapons
        if (weaponsInRange.Count == 0)
        { 
            closestWeapon = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Adding Weapons to the weaponsInRage list
        if (other.tag == "Weapon")
        {
            //Add to weapons in Range

            if (!weaponsInRange.Contains(other.gameObject))
            {
                weaponsInRange.Add(other.gameObject);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //check weapons in Range for other and remove it
        if (weaponsInRange.Contains(other.gameObject))
        { 
            weaponsInRange.Remove(other.gameObject);
        }

    }

}
