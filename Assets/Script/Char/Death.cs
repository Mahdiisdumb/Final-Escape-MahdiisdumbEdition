using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Sprite deathSprite;
    public AudioSource deathAudio; // assign a death sound here
    public float floatUpDistance = 1f; // how high the sprite goes
    public float floatUpSpeed = 2f;
    public float fallSpeed = 5f;
    public float delayBeforeScene = 5f; // wait 5 seconds before changing scene

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Change sprite to death
        renderer.sprite = deathSprite;

        // Disable all other scripts on this object
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this) s.enabled = false;
        }

        // Play death sound
        if (deathAudio != null) deathAudio.Play();

        // Start death animation coroutine
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        Vector3 startPos = transform.position;
        Vector3 upPos = startPos + Vector3.up * floatUpDistance;

        // Float up
        while (transform.position.y < upPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, upPos, floatUpSpeed * Time.deltaTime);
            yield return null;
        }

        // Optional: small pause at top
        yield return new WaitForSeconds(0.5f);

        // Fall down smoothly
        float elapsed = 0f;
        float totalFallTime = 2f; // total time to fall
        Vector3 fallStart = transform.position;
        Vector3 fallEnd = new Vector3(fallStart.x, fallStart.y - 5f, fallStart.z); // arbitrary distance down

        while (elapsed < totalFallTime)
        {
            transform.position = Vector3.Lerp(fallStart, fallEnd, elapsed / totalFallTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Wait fixed delay before scene change
        yield return new WaitForSeconds(delayBeforeScene);

        // Load Bad End scene
        SceneManager.LoadScene("Bad End");
    }
}