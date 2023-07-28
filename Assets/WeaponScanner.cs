using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScanner : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponsInRange;
    [SerializeField] private GameObject closestWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get the distance to the closest weapon and assign that to closestWeapon
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            //Add to weapons in Range
            if (!other.GetComponentInChildren<WeaponPickup>().pickedUp)
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
