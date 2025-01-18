using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool shootAtCusor = false;

    public float forwardSpeed = 20f;
    public float shipRotateSpeed = 450f;
    public float viewConstraint = 10f;
    public float cameraSpeed = 10f;

    // Rolling
    public float RollCooldown = 0.5f;
    private float leftRollTimer;
    private float rightRollTimer;
    public float rollBoost = 2f;
    public bool hasRolled = false;

    public GameObject cursor;
    public GameObject hitPointObject;

    // Bullet Variables
    public GameObject playerBullet;
    public float shootCooldown = 0.1f;
    private float shootTimer;
    public float damage = 1f;
    public float bulletSpeed = 30f;
    private bool shootSide = false;
    private Vector3 bulletOffset;
    public GameObject LeftMarker;
    public GameObject RightMarker;
    
    private Camera cam;
    [SerializeField] private LayerMask layerMask;
    public GameObject playerCrosshair;
    private RawImage XHAIRrawImage;

    public float stillTimer = -5f;

    private void Start()
    {
        //Application.targetFrameRate = 180;
        
        cam = Camera.main;

        XHAIRrawImage = playerCrosshair.GetComponent<RawImage>();

        //GetComponent<Rigidbody>().linearVelocity = Vector3.forward * forwardSpeed;
    }

    // OLD MOVEMENT SYSTEM
    /* void Update()
    {
        // WASD
        if ((Input.GetKey(KeyCode.W) || Gamepad.current.leftStick.up.isPressed) && transform.position.y < 2)
        {
            // Movement
            transform.position += new Vector3(0, controlledSpeed * Time.deltaTime, 0);
            if (transform.position.y > 2) { transform.position = new Vector3(transform.position.x, 2, transform.position.z); }

            //Lean
            shipRotationX -= shipRotateSpeed * Time.deltaTime;
            if (shipRotationX < -45) { shipRotationX = -45; }
            
            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }
        if ((Input.GetKey(KeyCode.S) || Gamepad.current.leftStick.down.isPressed) && transform.position.y > -2)
        {
            // Movement
            transform.position -= new Vector3(0, controlledSpeed * Time.deltaTime, 0);
            if (transform.position.y < -2) { transform.position = new Vector3(transform.position.x, -2, transform.position.z); }

            //Lean
            shipRotationX += shipRotateSpeed * Time.deltaTime;
            if (shipRotationX > 45) { shipRotationX = 45; }
            
            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }
        if ((Input.GetKey(KeyCode.D) || Gamepad.current.leftStick.right.isPressed) && transform.position.x < 4)
        {
            // Movement
            transform.position += new Vector3(controlledSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x > 4) { transform.position = new Vector3(4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ -= shipRotateSpeed * Time.deltaTime;
            
            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }
        if ((Input.GetKey(KeyCode.A) || Gamepad.current.leftStick.left.isPressed) && transform.position.x > -4)
        {
            // Movement
            transform.position -= new Vector3(controlledSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x < -4) { transform.position = new Vector3(-4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ += shipRotateSpeed * Time.deltaTime;
            
            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }

        //Roll
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (leftRollTimer < RollCooldown) 
            {
                Debug.Log("Left Roll");
                controlledSpeed = controlledSpeed * rollBoost;
                shipRotationZ += -360;
                leftRollTimer = 100;
                hasRolled = true;
            }
            else 
            {
                leftRollTimer = 0;
                hasRolled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (rightRollTimer < RollCooldown)
            {
                Debug.Log("Right Roll");
                transform.position += new Vector3(controlledSpeed * Time.deltaTime, 0, 0);
                controlledSpeed = controlledSpeed * rollBoost;
                shipRotationZ += 360;
                rightRollTimer = 100;
                hasRolled = true;
            }
            else
            {
                rightRollTimer = 0;
                hasRolled = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) { controlledSpeed = 10; }

        transform.rotation = Quaternion.Euler(new Vector3(shipRotationX, 0, shipRotationZ));
        leftRollTimer += 1 * Time.deltaTime;
        rightRollTimer += 1 * Time.deltaTime;

        // Auto Zero Rotation
        if (shipRotationX < 0) { shipRotationX += (shipRotateSpeed / 2) * Time.deltaTime; }
        else if (shipRotationX > 0) { shipRotationX -= (shipRotateSpeed / 2) * Time.deltaTime; }
        if (shipRotationZ < -30) { shipRotationZ += (shipRotateSpeed * 2) * Time.deltaTime; }
        else if (shipRotationZ < 0) { shipRotationZ += (shipRotateSpeed / 2) * Time.deltaTime; }
        if (shipRotationZ > 30) { shipRotationZ -= (shipRotateSpeed * 2) * Time.deltaTime; }
        else if (shipRotationZ > 0) { shipRotationZ -= (shipRotateSpeed / 2) * Time.deltaTime; }

        // Rotation Snap, only snaps an axis when related inputs are off
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            if ((shipRotationX < 0.5f) && (shipRotationX > -0.5f)) { shipRotationX = 0; }
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if ((shipRotationZ < 0.5f) && (shipRotationZ > -0.5f)) { shipRotationZ = 0; }
        }

        // Zooms camera out when still
        // if (stillTimer > -15) { stillTimer -= 1f * Time.deltaTime; }
        // cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam.transform.position.x, cam.transform.position.y, stillTimer), 3f * Time.deltaTime);
        // cam.fieldOfView =  Mathf.Lerp(cam.fieldOfView, 20 + (4 * (stillTimer + 15f)), 3f * Time.deltaTime);
        
        if (shootTimer < shootCooldown)
        {
            shootTimer += Time.deltaTime;
        }

        // Shooting
        // if (Input.GetKey(KeyCode.Space) && shootTimer >= shootCooldown)
        // {
        //     Shoot(false);
        // }

        if (Input.GetMouseButton(0) && shootTimer >= shootCooldown)
        {
            Shoot(true);
        }

        UpdateCrosshair();
    } */

    // NEW MOVEMENT SYSTEM
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(-90f * Gamepad.current.leftStick.ReadValue().y, 90f * Gamepad.current.leftStick.ReadValue().x, 0));
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        if (Vector3.Distance(Camera.main.transform.position, transform.position) > viewConstraint){Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z-10), cameraSpeed * Time.deltaTime);}
        else if (Vector3.Distance(Camera.main.transform.position, transform.position) > viewConstraint / 2){ Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Mathf.Lerp(Camera.main.transform.position.z, transform.position.z, cameraSpeed * Time.deltaTime)); } }

    void Shoot(bool atCursor)
    {
        shootTimer = 0;
        GetComponent<AudioSource>().Play();
        
        if (shootSide)
        {
            bulletOffset = RightMarker.transform.position;
            shootSide = false;
        }
        else
        {
            bulletOffset = LeftMarker.transform.position;
            shootSide = true;
        }

        GameObject newBullet = Instantiate(playerBullet, bulletOffset, Quaternion.Euler(90, 0, 0));
        newBullet.GetComponent<MeshRenderer>().material.color = Color.cyan;
        newBullet.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan * 5);
        newBullet.GetComponent<Bullet>().damage = 1;
        newBullet.GetComponent<Bullet>().isPlayerBullet = true;

        if (atCursor)
        {
            ShootAtCursor(newBullet);
        }
        else
        {
            newBullet.GetComponent<Rigidbody>().linearVelocity = Vector3.forward * bulletSpeed;
        }
    }
    void ShootAtCursor(GameObject bullet)
    {
        //Gets the position of the mouse in the world
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z + 100000));
        
        // Calculate the direction from the ship to the mouse position
        Vector3 direction = (mouseWorldPosition - cam.transform.position).normalized;

        //Fires a ray to the intended hit point from the camera
        Physics.Raycast(cam.transform.position, direction, out RaycastHit hit, Mathf.Infinity, layerMask);
        
        // Perform a raycast to check if it hits something
        if (hit.collider is not null)
        {
            
            
            // Point towards the hit point
            bullet.transform.LookAt(hit.collider.transform.position); // Hi it's Oliver I made it aim at the object directly
            bullet.transform.rotation = Quaternion.Euler(bullet.transform.rotation.eulerAngles + new Vector3(90, 0, 0));

            // Set the velocity to shoot towards the hit point
            bullet.GetComponent<Rigidbody>().linearVelocity = (hit.collider.transform.position - bullet.transform.position).normalized * bulletSpeed;
        }
        else
        {
            // In case no hit occurs, set a default direction for the bullet
            Vector3 fallbackPoint = cam.transform.position + (direction * 1000); // Arbitrary far point
            bullet.transform.LookAt(fallbackPoint);
            bullet.transform.rotation = Quaternion.Euler(bullet.transform.rotation.eulerAngles + new Vector3(90, 0, 0));
            bullet.GetComponent<Rigidbody>().linearVelocity = direction * bulletSpeed;
        }
    }

void UpdateCrosshair()
    {
        //Gets the position of the mouse in the world
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        
        // Calculate the direction from the ship to the mouse position
        Vector3 direction = (mouseWorldPosition - cam.transform.position).normalized;

        //Fires a ray to the intended hit point from the camera
        Physics.Raycast(cam.transform.position, direction, out RaycastHit hit, Mathf.Infinity, layerMask);
            
        // Perform a raycast to check if it hits something
        if (hit.collider != null)
        {
            hitPointObject = hit.collider.gameObject;
            playerCrosshair.transform.position = cam.WorldToScreenPoint(hitPointObject.transform.position);
            Vector3 minScreenPoint = cam.WorldToScreenPoint(hit.collider.bounds.min); // Bottom-left of the bounds
            Vector3 maxScreenPoint = cam.WorldToScreenPoint(hit.collider.bounds.max); // Top-right of the bounds

            // Calculate the width and height in screen space
            float width = maxScreenPoint.x - minScreenPoint.x;
            float height = maxScreenPoint.y - minScreenPoint.y;
            
            float maxLength = Mathf.Max(width / 2f, height / 2f);

            // Update the RawImage size
            //XHAIRrawImage.rectTransform.sizeDelta = new Vector2(maxLength, maxLength);
            
            if (Input.GetMouseButton(0))
            {
                XHAIRrawImage.color = Color.red;
                XHAIRrawImage.transform.Rotate(0, 0, 360 * Time.deltaTime);
                XHAIRrawImage.rectTransform.sizeDelta = new Vector2(maxLength / 1.5f, maxLength / 1.5f);
                //XHAIRrawImage.rectTransform.sizeDelta = new Vector2(25, 25);
                
                transform.LookAt(hit.collider.transform.position);
            }
            else
            {
                XHAIRrawImage.color = Color.green;
                XHAIRrawImage.transform.Rotate(0, 0, 90 * Time.deltaTime);
                XHAIRrawImage.rectTransform.sizeDelta = new Vector2(maxLength, maxLength);
                //XHAIRrawImage.rectTransform.sizeDelta = new Vector2(25, 25);
            }
        }
        else
        {
            hitPointObject = null;
            playerCrosshair.transform.position = cam.WorldToScreenPoint(mouseWorldPosition);

            XHAIRrawImage.color = Color.clear;
            XHAIRrawImage.transform.rotation = quaternion.identity;
            XHAIRrawImage.rectTransform.sizeDelta = new Vector2(10, 10);
            //XHAIRrawImage.rectTransform.sizeDelta = new Vector2(10, 10);
        }
    }
}
