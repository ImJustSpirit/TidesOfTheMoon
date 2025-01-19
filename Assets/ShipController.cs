using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    //Input
    private IA_Player input;
    private Vector2 move;
    
    //Components
    private Rigidbody rb;
    private Camera cam;
    private GameObject bulletPrefab;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 7f;
    
    [Header("Weapons")]
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float bulletSpeed = 10f;

    void Awake()
    {
        input = new IA_Player();
        input.Enable();
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        
        cam = Camera.main;
        
        shootCooldown = fireRate;
    }

    private void OnEnable()
    {
        input.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        input.Player.Shoot.performed -= OnShoot;
    }

    void Start()
    {
        bulletPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player Related/Bullet.prefab", typeof(GameObject)) as GameObject;
    }
    
    void Update()
    {
        HandleMovement();
        
        shootCooldown -= Time.deltaTime;
    }

    //Called by Unity Input System
    void OnMove(InputValue value)
    {
        Debug.Log("Player Moved");
        move = value.Get<Vector2>();
    }
    
    private void HandleMovement()
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        
        Vector3 targetSpeed = move * moveSpeed;
        Vector3 speedDiff = targetSpeed - rb.linearVelocity;
        float accelerationRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? acceleration : deceleration;
        Vector3 movement = speedDiff * accelerationRate * Time.deltaTime;

        rb.linearVelocity += movement;
        
        Vector2 minBounds = cam.ViewportToWorldPoint(new Vector2(0.05f, 0.05f));
        Vector2 maxBounds = cam.ViewportToWorldPoint(new Vector2(0.95f, 0.95f));
        gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(gameObject.transform.position.y, minBounds.y, maxBounds.y));
    }

    //Called by Unity Input System
    void OnShoot(InputAction.CallbackContext context)
    {
        if (shootCooldown > 0) return;
        
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.Euler(Vector3.forward * 90));
        bullet.GetComponent<Rigidbody>().linearVelocity = Vector3.right * bulletSpeed;

        shootCooldown = fireRate;
    }
}
