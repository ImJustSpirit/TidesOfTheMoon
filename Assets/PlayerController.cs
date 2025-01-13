using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && transform.position.y < 2) {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            if (transform.position.y > 2) { transform.position = new Vector3(transform.position.x, 2, transform.position.z); }
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -2) {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            if (transform.position.y < -2) { transform.position = new Vector3(transform.position.x, -2, transform.position.z); }
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 4)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x > 4) { transform.position = new Vector3(4, transform.position.y, transform.position.z); }
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -4)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x < -4) { transform.position = new Vector3(-4, transform.position.y, transform.position.z); }
        }
    }
}
