using UnityEngine;

public class CubeController : MonoBehaviour
{
    void Start()
    {
        float RandValue = Random.Range(3f, 10f);
        transform.localScale = new Vector3(RandValue, RandValue, RandValue);
    }
}
