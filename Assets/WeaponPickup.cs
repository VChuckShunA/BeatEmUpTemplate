using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public bool pickedUp;
    public int index;
    [SerializeField]private GameObject weaponObject;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer=this.gameObject.GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag=="Player")
        {
            pickedUp=true;
            this.gameObject.transform.parent.SetParent(other.gameObject.transform.GetChild(6).transform.transform);
            spriteRenderer.enabled=false;
            other.GetComponent<WeaponHolder>().EquipWeapon(index);
        }
    }
}
