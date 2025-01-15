using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public float damage;
    public bool isPlayerBullet;
    public bool isPenetrative;

    private void Start()
    {
        Invoke("Timeout", 5);
    }

    private void Timeout()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((isPlayerBullet && other.tag == "Enemy") || (!isPlayerBullet && other.tag == "Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage);

            if (!isPenetrative) { Destroy(gameObject); }
        }
    }
}
