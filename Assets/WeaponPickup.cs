using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        { 
            
        }
    }

    public void PickupWeapon(GameObject holder) {
        pickedUp = true;
        this.gameObject.transform.parent.SetParent(holder.gameObject.transform.GetChild(6).transform.transform);
        spriteRenderer.enabled = false;
        holder.GetComponent<WeaponHolder>().EquipWeapon(index,this.gameObject);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

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
