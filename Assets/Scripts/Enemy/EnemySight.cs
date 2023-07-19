using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] public bool playerInSight;
    [SerializeField] public GameObject player;

    public GameObject target;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject == player) {
            playerInSight = true;
            target = player;

		}
    }

    private void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			playerInSight = false;
            target = null;
		}
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
