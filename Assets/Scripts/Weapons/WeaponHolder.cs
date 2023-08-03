using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    /// <summary>
    /// This script is used by the player and the enemy to hold weapons
    /// </summary>
    [SerializeField] public GameObject[] weapons;
    [SerializeField] private int weaponIndex;
    [SerializeField] public GameObject currentWeapon;
    [SerializeField] private GameObject weaponDeatchPoint;
    [SerializeField] private bool hasWeapon;
   
    // Update is called once per frame
    void Update()
    {
        //Dropping weapons
        if (Input.GetButtonDown("Fire2"))
        { 
            DropWeapon();
        }
    }

    //Equipping Weapons
    public void EquipWeapon(int index,GameObject weapon)
    {
        weapons[index].gameObject.SetActive(true);
        weaponIndex = index;
        currentWeapon = weapon;
        hasWeapon = true;
    }


    //Dropping weapons
    public void DropWeapon() {
        if (hasWeapon)
        {
            Vector3 dropPosition = weaponDeatchPoint.transform.position;
            Quaternion dropRotation = weapons[weaponIndex].gameObject.transform.rotation;
            weapons[weaponIndex].gameObject.SetActive(false);
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.gameObject.transform.parent.position = dropPosition;
            currentWeapon.GetComponent<WeaponPickup>().DropWeapon();
            hasWeapon = false;
        }
    }
}
