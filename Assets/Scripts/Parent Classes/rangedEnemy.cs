using Unity.VisualScripting;
using UnityEngine;

public class rangedEnemy : Enemy
{
    // Assign these in EnemyManager, default values below should NOT typically be used
    public float shootRate = 1f;
    public float bulletSpeed = 10f;
    private float shootTimer = 0f;
    public float bulletDamage = 1f;
    public Vector3 TargetLocation;
    
    public GameObject bulletPrefab;

    void Start()
    {
        shootTimer = Random.Range(0f, shootRate);
    }
    
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootRate)
        {
            TargetLocation = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), playerTransform.position.z);
            
            Invoke("ShootBullet", 0f);
            Invoke("ShootBullet", 0.1f);
            Invoke("ShootBullet", 0.2f);
            shootTimer = 0f;
        }
    }

    void ShootBullet()
    {
        if (playerTransform == null) return;
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<Bullet>().owner = gameObject;
        
        //Vector3 skewedTarget = new Vector3(playerTransform.position.x + Random.Range(-1f, 1f), playerTransform.position.y + Random.Range(-1f, 1f), playerTransform.position.z);
        Vector3 direction = (TargetLocation - transform.position).normalized;
        bullet.transform.LookAt(TargetLocation);
        bullet.transform.rotation = Quaternion.Euler(bullet.transform.rotation.eulerAngles + new Vector3(90, 0, 0));
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}
