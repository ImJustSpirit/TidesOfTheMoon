using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private float shipRotationX = 0f;
    private float shipRotationZ = 0f;
    public float shipRotateSpeed = 450f;
    public float RollCooldown = 0.5f;
    private float leftRollTimer;
    private float rightRollTimer;
    public float rollBoost = 2f;


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

    private void Start()
    {
        //Application.targetFrameRate = 180;
    }

    void Update()
    {
        // WASD
        if (Input.GetKey(KeyCode.W) && transform.position.y < 2)
        {
            // Movement
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            if (transform.position.y > 2) { transform.position = new Vector3(transform.position.x, 2, transform.position.z); }

            //Lean
            shipRotationX -= shipRotateSpeed * Time.deltaTime;
            if (shipRotationX < -45) { shipRotationX = -45; }
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -2)
        {
            // Movement
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            if (transform.position.y < -2) { transform.position = new Vector3(transform.position.x, -2, transform.position.z); }

            //Lean
            shipRotationX += shipRotateSpeed * Time.deltaTime;
            if (shipRotationX > 45) { shipRotationX = 45; }
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 4)
        {
            // Movement
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x > 4) { transform.position = new Vector3(4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ -= shipRotateSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -4)
        {
            // Movement
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x < -4) { transform.position = new Vector3(-4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ += shipRotateSpeed * Time.deltaTime;
        }

        //Roll
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (leftRollTimer < RollCooldown) 
            {
                Debug.Log("Left Roll");
                speed = speed * rollBoost;
                shipRotationZ += -360;
                leftRollTimer = 100;
            }
            else { leftRollTimer = 0; }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (rightRollTimer < RollCooldown)
            {
                Debug.Log("Right Roll");
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                speed = speed * rollBoost;
                shipRotationZ += 360;
                rightRollTimer = 100;
            }
            else { rightRollTimer = 0; }
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) { speed = 10; }

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
        
        if (shootTimer < shootCooldown)
        {
            shootTimer += Time.deltaTime;
        }

        // Shooting
        if (Input.GetKey(KeyCode.Space) && shootTimer>=shootCooldown)
        {
            shootTimer = 0;
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
            newBullet.GetComponent<Bullet>().damage = 1;
            newBullet.GetComponent<Bullet>().isPlayerBullet = true;
            newBullet.GetComponent<Rigidbody>().linearVelocity = Vector3.forward * bulletSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
