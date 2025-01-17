using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float collisionDamage;
    public Transform playerTransform;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>()?.TakeDamage(collisionDamage);
        }
    }
}