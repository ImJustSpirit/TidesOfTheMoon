using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 10;
    private float shipRotationX = 0;
    private float shipRotationZ = 0;
    public float shipRotateSpeed = 450;

    // Bullet Variables
    public GameObject playerBullet;
    public float shootCooldown = 0.1f;
    private float shootTimer;
    public float damage = 1;
    public float bulletSpeed = 30;
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
            if (shipRotationZ < -30) { shipRotationZ = -30; }
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -4)
        {
            // Movement
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x < -4) { transform.position = new Vector3(-4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ += shipRotateSpeed * Time.deltaTime;
            if (shipRotationZ > 30) { shipRotationZ = 30; }
        }

        transform.rotation = Quaternion.Euler(new Vector3(shipRotationX, 0, shipRotationZ));

        // Auto Zero Rotation
        if (shipRotationX < 0) { shipRotationX += (shipRotateSpeed / 2) * Time.deltaTime; }
        else if (shipRotationX > 0) { shipRotationX -= (shipRotateSpeed / 2) * Time.deltaTime; }
        if (shipRotationZ < 0) { shipRotationZ += (shipRotateSpeed / 2) * Time.deltaTime; }
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

        // Shootning
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
            newBullet.GetComponent<Rigidbody>().linearVelocity = Vector3.forward * bulletSpeed;
        }
    }
}
