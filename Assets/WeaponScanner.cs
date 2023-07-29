using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponScanner : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponsInRange;
    [SerializeField] public GameObject closestWeapon;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Update Weapons in Range
        weaponsInRange.RemoveAll(g => g.GetComponentInChildren<WeaponPickup>().pickedUp);
        //get the distance to the closest weapon and assign that to closestWeapon
        float minDistance = float.MaxValue;
        closestWeapon = null;
        foreach (GameObject weapon in weaponsInRange)
        {
            float distance = Vector3.Distance(weapon.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestWeapon = weapon;
            }
        }
        if (weaponsInRange.Count == 0)
        { 
            closestWeapon = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
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
