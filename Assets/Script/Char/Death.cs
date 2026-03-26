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

        // Fall down off screen
        while (transform.position.y > -10f) // arbitrary bottom of screen
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        // Load Bad End scene
        SceneManager.LoadScene("Bad End");
    }
}