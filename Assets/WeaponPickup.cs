using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyState;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<EnemyState>().currentState == currentStateEnum.retreat)
            {
                PickupWeapon(other.gameObject,2);
            }
        }
    }

    public void PickupWeapon(GameObject holder,int attachPointIndex) {
        pickedUp = true;
        this.gameObject.transform.parent.SetParent(holder.gameObject.transform.GetChild(attachPointIndex).transform.transform);
        spriteRenderer.enabled = false;
        holder.GetComponent<WeaponHolder>().EquipWeapon(index,this.gameObject);
        //this.gameObject.transform.parent.gameObject.SetActive(false);
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
