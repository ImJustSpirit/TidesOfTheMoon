using UnityEngine;
using UnityEngine.Rendering;

public class CursorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30));
    }
}
