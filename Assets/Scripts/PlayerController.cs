using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 10;
    private float shipRotationZ = 0;
    private float shipRotationX = 0;
    public float shipRotateSpeed = 250;

    // Bullet Variables
    public GameObject playerBullet;
    public float shootCooldown = 0.2f;
    private float shootTimer;
    public float damage = 1;
    public float bulletSpeed = 30;
    private bool shootSide = false;
    private float bulletOffset;

    void Update()
    {
        // WASD
        if (Input.GetKey(KeyCode.W) && transform.position.y < 2) {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            if (transform.position.y > 2) { transform.position = new Vector3(transform.position.x, 2, transform.position.z); }

            //Lean
            shipRotationX -= shipRotateSpeed * Time.deltaTime;
            if (shipRotationX < -30) { shipRotationX = -30; }
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -2) {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            if (transform.position.y < -2) { transform.position = new Vector3(transform.position.x, -2, transform.position.z); }

            //Lean
            shipRotationX += shipRotateSpeed * Time.deltaTime;
            if (shipRotationX > 30) { shipRotationX = 30; }
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 4)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x > 4) { transform.position = new Vector3(4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ -= shipRotateSpeed * Time.deltaTime;
            if (shipRotationZ < -30) { shipRotationZ = -30; }
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -4)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x < -4) { transform.position = new Vector3(-4, transform.position.y, transform.position.z); }

            //Lean
            shipRotationZ += shipRotateSpeed * Time.deltaTime;
            if (shipRotationZ > 30) { shipRotationZ = 30; }
        }

        transform.rotation = Quaternion.Euler(new Vector3(shipRotationX, 0, shipRotationZ));

        if (shootTimer < shootCooldown)
        {
            shootTimer += Time.deltaTime;
        }

        // Space
        if (Input.GetKey(KeyCode.Space) && shootTimer>=shootCooldown)
        {
            shootTimer = 0;
            if (shootSide)
            {
                bulletOffset = 1;
                shootSide = false;
            }
            else
            {
                bulletOffset = -1;
                shootSide = true;
            }
            GameObject newBullet = Instantiate(playerBullet, new Vector3(transform.position.x + bulletOffset, transform.position.y, transform.position.z), Quaternion.Euler(90, 0, 0));
            newBullet.GetComponent<Bullet>().damage = 1;
            newBullet.GetComponent<Rigidbody>().linearVelocity = Vector3.forward * bulletSpeed;
        }
    }
}
