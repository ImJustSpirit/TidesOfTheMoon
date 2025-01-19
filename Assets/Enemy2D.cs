using UnityEditor;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    public GameObject player;
    private Vector2 areaX = new (3f, 8f);
    private Vector2 areaY = new (4.5f, -4.5f);

    private GameObject bulletPrefab;

    private float moveTime = 4f;
    
    private Vector3 targetPos;

    void Start()
    {
        bulletPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player Related/Bullet.prefab", typeof(GameObject)) as GameObject;

        Invoke(nameof(Move), Random.Range(moveTime / 2f, moveTime));
        InvokeRepeating(nameof(Attack), 1f, 4f);
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime);
    }

    void Move()
    {
        targetPos = new Vector3(Random.Range(areaX.x, areaX.y), Random.Range(areaY.x, areaY.y), 0f);

        Invoke(nameof(Move), Random.Range(moveTime / 2f, moveTime));
    }

    void Attack()
    {
        float attack = Random.Range(0f, 10f);

        if (attack > 3)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.damage = 1f;
            bullet.owner = gameObject;
            bullet.transform.localScale = new Vector3(0.3f, 0.15f, 0.3f);
            bullet.GetComponent<Rigidbody>().linearVelocity = Vector2.left * 10f;
        }
        else if (attack > 1)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.damage = 2f;
            bullet.owner = gameObject;
            bullet.transform.localScale = new Vector3(0.6f, 0.3f, 0.6f);
            bullet.GetComponent<Rigidbody>().linearVelocity = Vector2.left * 5f;
        }
        else
        {
            Vector3 angle = (Vector3.left + new Vector3(0, 0.3f, 0)).normalized;
            for (int i = 0; i < 3; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.damage = 1f;
                bullet.owner = gameObject;
                bullet.transform.localScale = new Vector3(0.3f, 0.15f, 0.3f);
                bullet.GetComponent<Rigidbody>().linearVelocity = angle * 10f;
                
                angle = (angle - new Vector3(0, 0.3f, 0)).normalized;
            }
        }
    }
}
