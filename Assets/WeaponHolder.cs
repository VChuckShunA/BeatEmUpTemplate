using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] public GameObject[] weapons;
    [SerializeField] private int weaponIndex;
    [SerializeField] public GameObject currentWeapon;
    [SerializeField] private GameObject weaponDeatchPoint;
    [SerializeField] private bool hasWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        { 
            DropWeapon();
        }
    }
    public void EquipWeapon(int index,GameObject weapon)
    {
        weapons[index].gameObject.SetActive(true);
        weaponIndex = index;
        currentWeapon = weapon;
        hasWeapon = true;
    }



    public void DropWeapon() {
        if (hasWeapon)
        {
            Vector3 dropPosition = weaponDeatchPoint.transform.position;
            Quaternion dropRotation = weapons[weaponIndex].gameObject.transform.rotation;
            weapons[weaponIndex].gameObject.SetActive(false);
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.gameObject.transform.parent.position = dropPosition;
            //currentWeapon.gameObject.transform.rotation = dropRotation;
            currentWeapon.GetComponent<WeaponPickup>().DropWeapon();
            hasWeapon = false;
        }
    }
}
