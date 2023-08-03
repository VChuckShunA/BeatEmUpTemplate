using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyState;

public class WeaponPickup : MonoBehaviour
{
    /// <summary>
    /// This script allows weapons to be picked up and held by both the player and the enemy
    /// </summary>
    public bool pickedUp;
    public int index;
    [SerializeField]private GameObject weaponObject;
    private SpriteRenderer spriteRenderer;
    private Quaternion initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer=this.gameObject.GetComponentInParent<SpriteRenderer>();
        initialRotation=this.gameObject.transform.rotation;
    }

  

    private void OnTriggerStay(Collider other)
    {
        //Uses the enemy's retreat collider to  check if the enemy has collided with the weapon
        if (other.tag == "RetreatCollider")
        {
            if (other.GetComponentInParent<EnemyState>().currentState == currentStateEnum.retreat) //Checks if the enemy is retreating. The enemy can only pickup weapons when retreating
            {
                //Picking up weapons
                PickupWeapon(other.gameObject.transform.parent.gameObject,2);
            }
        }
    }

    //Picking up weapon
    public void PickupWeapon(GameObject holder,int attachPointIndex) {
        pickedUp = true;
        this.gameObject.transform.parent.SetParent(holder.gameObject.transform.GetChild(attachPointIndex).transform.transform);//This assigns the weapon to the Enemy/Player's and attachpoint
        spriteRenderer.enabled = false; //disables the sprite
        holder.GetComponent<WeaponHolder>().EquipWeapon(index,this.gameObject); //Equips Weapon
    }

    //Dropping Weapon
    public void DropWeapon()
    {
        this.gameObject.transform.parent.gameObject.SetActive(true);
        this.gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.transform.parent.gameObject.transform.parent = null;
        this.gameObject.transform.position = this.gameObject.transform.parent.gameObject.transform.localPosition;
        this.gameObject.transform.rotation = initialRotation;
        pickedUp =false;
    }
}
