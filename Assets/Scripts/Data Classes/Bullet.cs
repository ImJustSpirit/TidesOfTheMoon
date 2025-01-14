using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        Invoke("Timeout", 5);
    }

    private void Timeout()
    {
        Destroy(gameObject);
    }
}
