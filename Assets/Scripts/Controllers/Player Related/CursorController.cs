using UnityEngine;
using UnityEngine.Rendering;

public class CursorController : MonoBehaviour
{
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30));
    }
}
