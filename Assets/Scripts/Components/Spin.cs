using UnityEngine;

public class Spin : MonoBehaviour
{
    public float SpinStrength;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(SpinStrength * 10 * Time.deltaTime, SpinStrength * 20 * Time.deltaTime, SpinStrength * 30 * Time.deltaTime);
    }
}
