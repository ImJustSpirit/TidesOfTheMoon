using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour
{
    private new Renderer renderer;
    private Color originalColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public void OnHit()
    {
        /*StopAllCoroutines(); // Stop any existing color transition
        StartCoroutine(HitColorCoroutine());*/
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

    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines(); // Stop any existing color transition
        StartCoroutine(HitColorCoroutine());
    }
}