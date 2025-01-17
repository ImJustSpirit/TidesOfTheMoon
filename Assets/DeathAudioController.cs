using UnityEngine;

public class DeathAudioController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<AudioSource>().Play();
        Invoke ("Timeout", GetComponent<AudioSource>().clip.length);
    }

    void Timeout()
    {
        Destroy(gameObject);
    }
}
