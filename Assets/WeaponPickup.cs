using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public bool pickedUp;

    [SerializeField]private GameObject weaponObject;
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log(other.gameObject.transform.GetChild(6));
        }
    }
}
