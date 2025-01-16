using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage;
    public float speed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}