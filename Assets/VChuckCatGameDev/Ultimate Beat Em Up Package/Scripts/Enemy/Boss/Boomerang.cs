using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    /// <summary>
    /// This script handles the Boomerang
    /// </summary>
    public int direction = 1;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MoveBoomerang());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //moving the Boomerang
        rb.velocity = new Vector3(6 * direction, 0, 2 * direction);

    }

    IEnumerator MoveBoomerang()
    {
        //redirecting the Boomerang
        yield return new WaitForSeconds(2f);
        direction *= -1;
    }
}
