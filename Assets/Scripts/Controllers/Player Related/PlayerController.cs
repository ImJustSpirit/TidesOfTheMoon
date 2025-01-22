using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum Playstyle {ControlShip, ControlCursor, OldWASD, true3D}
    [Header ("Macro Variables")]
    public Playstyle currentPlaystyle = Playstyle.ControlShip;

    public float forwardSpeed = 20f;
    public float shipRotateSpeed = 450f;

    public GameObject cursor;
    public GameObject hitPointObject;

    [Header ("Shooting Variables")]
    public GameObject playerBullet;
    public float shootCooldown = 0.1f;
    [SerializeField] private float shootTimer;
    public float damage = 1f;
    public float bulletSpeed = 30f;
    private bool shootSide = false;
    private Vector3 bulletOffset;
    public GameObject leftMarker;
    public GameObject rightMarker;

    private Camera cam;
    [SerializeField] private LayerMask layerMask;
    public GameObject playerCrosshair;
    private RawImage XHAIRrawImage;

    [Header ("Playstyle - ControlCursor")]
    public float stillTimer = -5f;
    [SerializeField] private float rotationLimit = 45f;
    [SerializeField] private float followCursorSpeed = 1f;
    [SerializeField] private float shipAimYOffset = 0.5f;
    
    [Header ("ControlCursor CircleRestriction")]
    [SerializeField] private bool aimRestriction = false;
    [SerializeField] private float aimRestrictionRadius = 800f;
    
    [Header ("Playstyle - ControlShip")]
    [SerializeField] private Vector2 currentShipRotation;
    [SerializeField] private Vector2 movementVector;
    
    [Header ("Playstyle - OldWASD")]
    [SerializeField] private float shipRotationX;
    [SerializeField] private float shipRotationY;
    [SerializeField] private float shipRotationZ;
    [SerializeField]  float controlledSpeed = 10f; 
    public float rollCooldown = 0.5f;
    [SerializeField] private float leftRollTimer;
    [SerializeField] private float rightRollTimer;
    public float rollBoost = 2f;
    public bool hasRolled = false;
    
    [Header ("Playstyle - true3D")]
    //[SerializeField] private float forwardSpeed = 10f; // Speed at which the ship moves forward
    [SerializeField] private float rotationSpeed = 50f; // Speed at which the ship rotates
    private Vector2 moveInput; // Variable to store the input from the left analog stick

    private void Start()
    {
        //Application.targetFrameRate = 180;

        cam = Camera.main;

        XHAIRrawImage = playerCrosshair.GetComponent<RawImage>();

        GetComponent<Rigidbody>().linearVelocity = Vector3.forward * forwardSpeed;
    }

    // Update function now diverts to another function depending on chosen playstyle
    void Update()
    {
        switch (currentPlaystyle)
        {
            case Playstyle.ControlShip:
                ControlShipUpdate();
                break;
            case Playstyle.ControlCursor:
                ControlCursorUpdate();
                break;
            case Playstyle.OldWASD:
                OldWASDUpdate();
                break;
            case Playstyle.true3D:
                true3DUpdate();
                break;
            default:
                Debug.LogError("Invalid Playstyle");
                break;
        }
    }
    
    // Playstyle - ControlShip
    #region Playstyle ControlShip
    void ControlShipUpdate()
    {
        UpdateCrosshair();
        
        // Vertical Movement
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) { movementVector.y = 0; }
        else if (Input.GetKey(KeyCode.W)) { movementVector.y = 1; }
        else if (Input.GetKey(KeyCode.S)) { movementVector.y = -1; }
        else { movementVector.y = 0; }
        
        // Horizontal Movement
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) { movementVector.x = 0; }
        else if (Input.GetKey(KeyCode.D)) { movementVector.x = 1; }
        else if (Input.GetKey(KeyCode.A)) { movementVector.x = -1; }
        else { movementVector.x = 0; }
            
        // Diagonal Movement
        if (movementVector.x == 1 && movementVector.y == 1) { movementVector = new Vector2(0.707f, 0.707f); }
        else if (movementVector.x == 1 && movementVector.y == -1) { movementVector = new Vector2(0.707f, -0.707f); }
        else if (movementVector.x == -1 && movementVector.y == 1) { movementVector = new Vector2(-0.707f, 0.707f); }
        else if (movementVector.x == -1 && movementVector.y == -1) { movementVector = new Vector2(-0.707f, -0.707f); }
        
        //Moving
        //Debug.Log(movementVector);
        
        // Would Really like a better way of using both controller and keyboard inputs, this isn't ideal.
        // Should be able to apply 'Gamepad.current.leftStick.ReadValue()' to movementVector, but I need to think of a way to do it so that it doesn't always override the keyboard input.
        if (Gamepad.current != null) { currentShipRotation = Vector2.Lerp(currentShipRotation, Gamepad.current.leftStick.ReadValue(), shipRotateSpeed * Time.deltaTime); }
        else { currentShipRotation = Vector2.Lerp(currentShipRotation, movementVector, shipRotateSpeed * Time.deltaTime); }
        
        // Rotation Limit
        transform.rotation = Quaternion.Euler(-70f * currentShipRotation.y, 70f * currentShipRotation.x, 0);
       
        // Move Forward
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        
        // Camera Constraints
        if (transform.position.x - cam.transform.position.x < -4.5f) { cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x - 4.5f, 0.3f * Time.deltaTime), cam.transform.position.y, cam.transform.position.z);}
        else if (transform.position.x - cam.transform.position.x > 4.5f) { cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x + 4.5f, 0.3f * Time.deltaTime), cam.transform.position.y, cam.transform.position.z);}
        if (transform.position.y - cam.transform.position.y < -2.5f) { cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Lerp(cam.transform.position.y, transform.position.y - 2.5f, 0.3f * Time.deltaTime), cam.transform.position.z);}
        else if (transform.position.y - cam.transform.position.y > 2.5f) { cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Lerp(cam.transform.position.y, transform.position.y + 2.5f, 0.3f * Time.deltaTime), cam.transform.position.z);}

        // Shooting
        if (shootTimer < shootCooldown) { shootTimer += Time.deltaTime; }
        if (Input.GetKey(KeyCode.Space) && shootTimer >= shootCooldown) { Shoot(false); } // Forward Shooting
        if (Input.GetMouseButton(0) && shootTimer >= shootCooldown) { Shoot(true); } // Cursor Shooting
    }
    #endregion
    
    // Playstyle - ControlCursor
    #region Playstyle ControlCursor
    private void ControlCursorUpdate()
    {
        UpdateCrosshair();

        var CrosshairPositionButForwards = cam.ScreenToWorldPoint(new Vector3(playerCrosshair.transform.position.x,
            playerCrosshair.transform.position.y, playerCrosshair.transform.position.z + 10));
        var ShipPositionButScreenSpace =
            cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + shipAimYOffset,
                transform.position.z));



        transform.LookAt(CrosshairPositionButForwards);

        if (ShipPositionButScreenSpace.x < playerCrosshair.transform.position.x)
        {
            float difference = Mathf.Abs(ShipPositionButScreenSpace.x - playerCrosshair.transform.position.x) / 10f;
            if (difference > 1)
            {
                transform.position += new Vector3((followCursorSpeed * difference) * Time.deltaTime, 0, 0);
            }
        }

        if (ShipPositionButScreenSpace.x > playerCrosshair.transform.position.x)
        {
            float difference = Mathf.Abs(ShipPositionButScreenSpace.x - playerCrosshair.transform.position.x) / 10f;
            if (difference > 1)
            {
                transform.position += new Vector3(-(followCursorSpeed * difference) * Time.deltaTime, 0, 0);
            }
        }

        if (ShipPositionButScreenSpace.y < playerCrosshair.transform.position.y)
        {
            float difference = Mathf.Abs(ShipPositionButScreenSpace.y - playerCrosshair.transform.position.y) / 10f;
            if (difference > 1)
            {
                transform.position += new Vector3(0, (followCursorSpeed * difference) * Time.deltaTime, 0);
            }
        }

        if (ShipPositionButScreenSpace.y > playerCrosshair.transform.position.y)
        {
            float difference = Mathf.Abs(ShipPositionButScreenSpace.y - playerCrosshair.transform.position.y) / 10f;
            if (difference > 1)
            {
                transform.position += new Vector3(0, -(followCursorSpeed * difference) * Time.deltaTime, 0);

            }
        }

        if (shootTimer < shootCooldown)
        {
            shootTimer += Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && shootTimer >= shootCooldown)
        {
            Shoot(true);
        }
    }
    #endregion

    // Playstyle - OldWASD
    #region Playstyle OldWASD
    private void OldWASDUpdate()
    {
        // Doesn't work properly as shipRotateSpeed is too low
        
        // WASD
        if (Input.GetKey(KeyCode.W) && transform.position.y < 2)
        {
            // Movement
            transform.position += new Vector3(0, controlledSpeed * Time.deltaTime, 0);
            if (transform.position.y > 2) { transform.position = new Vector3(transform.position.x, 2, transform.position.z); }

            //Lean
            shipRotationX -= shipRotateSpeed * Time.deltaTime;
            if (shipRotationX < -45) { shipRotationX = -45; }

            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -2)
        {
            // Movement
            transform.position -= new Vector3(0, controlledSpeed * Time.deltaTime, 0);
            if (transform.position.y < -2) { transform.position = new Vector3(transform.position.x, -2, transform.position.z); }

            //Lean
            shipRotationX += shipRotateSpeed * Time.deltaTime;
            if (shipRotationX > 45) { shipRotationX = 45; }

            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 4)
        {
            // Movement
            transform.position += new Vector3(controlledSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x > 4) { transform.position = new Vector3(4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ -= shipRotateSpeed * Time.deltaTime;

            if (stillTimer < -5) { stillTimer += 30f * Time.deltaTime; }
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -4)
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
            if (leftRollTimer < rollCooldown)
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
            if (rightRollTimer < rollCooldown)
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

        if (shootTimer < shootCooldown) { shootTimer += Time.deltaTime; }

        // Shooting
        if (Input.GetKey(KeyCode.Space) && shootTimer >= shootCooldown) { Shoot(false); }
        if (Input.GetMouseButton(0) && shootTimer >= shootCooldown) { Shoot(true); }

        UpdateCrosshair();
    }
    #endregion
    
    // Playstyle - true3D
    private void true3DUpdate()
    {
        // Move the ship forward continuously
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Rotate the ship based on input
        //transform.rotation += Quaternion.Euler(moveInput.y * rotationSpeed * Time.deltaTime, 0, 0); // Up/down rotation
        
        transform.Rotate(moveInput.y * -rotationSpeed * Time.deltaTime, 0, 0);
        transform.Rotate(0, moveInput.x * rotationSpeed * Time.deltaTime, 0);
        //float roll = -moveInput.x * rotationSpeed * Time.deltaTime; // Left/right rotation

        // Apply the rotation to the ship
        //transform.Rotate(pitch, 0, roll);

        if (Gamepad.current != null && Gamepad.current.rightTrigger.isPressed)
        {
            Shoot(false);
        }
    }
    
    // This method is called by the Input System when the left analog stick is used
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    void Shoot(bool atCursor)
    {
        shootTimer = 0;
        GetComponent<AudioSource>().Play();

        if (shootSide)
        {
            bulletOffset = rightMarker.transform.position;
            shootSide = false;
        }
        else
        {
            bulletOffset = leftMarker.transform.position;
            shootSide = true;
        }

        GameObject newBullet = Instantiate(playerBullet, bulletOffset, Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
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
            newBullet.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;
        }
    }

    void ShootAtCursor(GameObject bullet)
    {
        //Gets the position of the mouse in the world
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            transform.position.z + 100000));

        // Calculate the direction from the ship to the mouse position
        Vector3 direction = (mouseWorldPosition - cam.transform.position).normalized;

        //Fires a ray to the intended hit point from the camera
        Physics.Raycast(cam.transform.position, direction, out RaycastHit hit, Mathf.Infinity, layerMask);

        // Perform a raycast to check if it hits something
        if (hit.collider is not null)
        {


            // Point towards the hit point
            bullet.transform.LookAt(hit.collider.transform
                .position); // Hi it's Oliver I made it aim at the object directly
            bullet.transform.rotation = Quaternion.Euler(bullet.transform.rotation.eulerAngles + new Vector3(90, 0, 0));

            // Set the velocity to shoot towards the hit point
            bullet.GetComponent<Rigidbody>().linearVelocity =
                (hit.collider.transform.position - bullet.transform.position).normalized * bulletSpeed;
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
        Vector3 mouseWorldPosition =
            cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));

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
            if (Input.GetMouseButton(0))
            {
                hitPointObject = null;
                playerCrosshair.transform.position = cam.WorldToScreenPoint(mouseWorldPosition);

                XHAIRrawImage.color = Color.white;
                //XHAIRrawImage.transform.rotation = quaternion.identity;
                XHAIRrawImage.transform.Rotate(0, 0, 360 * Time.deltaTime);
                XHAIRrawImage.rectTransform.sizeDelta = new Vector2(40, 40);
                //XHAIRrawImage.rectTransform.sizeDelta = new Vector2(10, 10);
            }
            else
            {
                hitPointObject = null;
                playerCrosshair.transform.position = cam.WorldToScreenPoint(mouseWorldPosition);

                XHAIRrawImage.color = Color.white;
                //XHAIRrawImage.transform.rotation = quaternion.identity;
                XHAIRrawImage.transform.Rotate(0, 0, 90 * Time.deltaTime);
                XHAIRrawImage.rectTransform.sizeDelta = new Vector2(50, 50);
                //XHAIRrawImage.rectTransform.sizeDelta = new Vector2(10, 10);
            }
        }

        if (aimRestriction)
        {
            ClampCrosshairToCircle();
        }
    }

    void ClampCrosshairToCircle()
    {
        Vector3 playerScreenPosition =
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + shipAimYOffset, transform.position.z)); // Convert player's current position to screen space
        Vector3 crosshairScreenPosition = new Vector3(
            playerCrosshair.transform.position.x,
            playerCrosshair.transform.position.y,
            playerCrosshair.transform.position.z
        );

        // Calculate the direction and distance between the player and the crosshair
        Vector3 direction = crosshairScreenPosition - playerScreenPosition;
        float distance = direction.magnitude;

        // Check if the distance is greater than the circle radius (aimRestrictionRadiusRadius units)
        float maxRadius = aimRestrictionRadius;
        if (distance > maxRadius)
        {
            // Clamp the position to the circle's boundary
            crosshairScreenPosition = playerScreenPosition + (direction.normalized * maxRadius);
        }

        // Convert back to world space and update crosshair position
        playerCrosshair.transform.position = new Vector3(
            crosshairScreenPosition.x,
            crosshairScreenPosition.y,
            0);
    }
}
