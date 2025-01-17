using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float health;
    public GameObject deathAudio;
    
    private new Renderer renderer;
    private Color originalColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }
    
    public void TakeDamage(float bulletDamage)
    {
        health -= bulletDamage;
        if (health <= 0)
        {
            if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene("Lv_01");
            }
            
            Instantiate(deathAudio, transform.position, Quaternion.identity); // Play the death sound effect ( if it exists)
            
            // Temporary but this should be an event for the main class controller to receive and die
            Destroy(gameObject);
        }

        ProcessHit();
    }
    
    public void ProcessHit()
    {
        //GetComponent<AudioSource>()?.Play(); // Play the hit sound effect
        StopAllCoroutines(); // Stop any existing color transition
        StartCoroutine(HitColorCoroutine());
    }
    
    private IEnumerator HitColorCoroutine()
    {
        // Turn the entity red
        renderer.material.color = new Color(1, 0.25f, 0.25f);
        yield return new WaitForSeconds(0.05f); // Be the above color for X many seconds

        // Fade back to original color
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f) // Fade from the hit color to the original color in seconds
        {
            renderer.material.color = Color.Lerp(new Color(1, 0.25f, 0.25f), originalColor, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = originalColor;
    }
}
