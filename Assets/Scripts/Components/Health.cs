using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;

    public void TakeDamage(float bulletDamage)
    {
        health -= bulletDamage;
        if (health <= 0)
        {
            // Temporary but this should be an event for the main class controller to receive and die
            Destroy(gameObject);
        }

        GetComponent<HitProcesser>().ProcessHit();
    }
}
