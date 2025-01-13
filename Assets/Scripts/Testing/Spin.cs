using UnityEngine;

public class Spin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(10 * Time.deltaTime, 20 * Time.deltaTime, 30 * Time.deltaTime);
    }
}
