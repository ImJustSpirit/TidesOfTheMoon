using System;
using UnityEngine;

public class meleeEnemy : Enemy
{
    public bool canLatch = false;
    public float movementSpeed = 1f;

    //Latching
    public bool latched = false;
    private bool check1 = false;
    private bool check2 = false;
    private bool check3 = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 9) && (canLatch))
        { 
            latched = true;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!latched) { transform.position = Vector3.Lerp(transform.position, playerTransform.position, movementSpeed * Time.deltaTime); }
        else
        { 
            transform.position = playerTransform.position;
            if (playerTransform.GetComponent<PlayerController>().hasRolled == false) { check1 = true; }
            if ((playerTransform.GetComponent<PlayerController>().hasRolled == true) && check1) { check2 = true; }
            if ((playerTransform.GetComponent<PlayerController>().hasRolled == false) && check2) { check3 = true; }
            if ((playerTransform.GetComponent<PlayerController>().hasRolled == true) && check3) { Destroy(gameObject); }
        }
    }
}
