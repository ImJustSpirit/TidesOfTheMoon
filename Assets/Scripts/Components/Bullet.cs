using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public float damage;
    public bool isPenetrative;
    public Color color = Color.red;

    private LayerMask layerMask;

    public GameObject owner;

    private float lifetime = 5f;

    /*public Bullet(float damage, bool isPenetrative, GameObject owner, Color color)
    {
        this.damage = damage;
        this.isPenetrative = isPenetrative;
        this.owner = owner;
        
        GetComponent<MeshRenderer>().material.color = color;
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color * 5);
    }*/
    
    
    private void Start()
    {
        if (owner == null)
        {
            Debug.LogWarning("Bullet has no owner assigned! Game object will be destroyed.");
            Destroy(gameObject);
        }
        
        layerMask = owner.layer;
        
        Invoke(nameof(Timeout), lifetime);
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.color = color;
        rend.material.SetColor("_EmissionColor", color * 5f);
    }

    private void Timeout()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health) && other.gameObject.layer != layerMask)
        {
            health.TakeDamage(damage);

            if (!isPenetrative) Destroy(gameObject);
        }
    }
}
