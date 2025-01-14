using UnityEngine;

public class CubeController : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GetComponent<HitProcesser>().ProcessHit(other.gameObject);
        }
    }
}
