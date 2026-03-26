using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SonicEXE : MonoBehaviour
{
    [Header("Movement")]
    public Transform player;
    public float speed = 6f;

    [Header("Stun")]
    public float stunDuration = 2f;

    [Header("Sprites")]
    public SpriteRenderer renderer;
    public Sprite[] flyingSprites;
    public Sprite[] stunnedSprites;
    public float frameRate = 0.1f;

    private bool isStunned = false;

    // Animation
    private Sprite[] currentFrames;
    private int frameIndex;
    private float timer;

    void Start()
    {
        SetAnimation(flyingSprites);
    }

    void Update()
    {
        Animate();

        if (isStunned || player == null) return;

        // Move toward player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // Flip sprite based on player position
        if (player.position.x > transform.position.x) renderer.flipX = false;
        else renderer.flipX = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !isStunned)
        {
            col.GetComponent<PlayerDeath>()?.Die();
        }
    }

    // Call this to stun SonicEXE
    public void Stun(float duration)
    {
        if (isStunned) return;

        isStunned = true;
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        SetAnimation(stunnedSprites);

        // Optionally disable collider so player can't touch while stunned
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        yield return new WaitForSeconds(duration);

        if (col != null) col.enabled = true;

        SetAnimation(flyingSprites);
        isStunned = false;
    }

    private void SetAnimation(Sprite[] frames)
    {
        if (currentFrames == frames) return;
        currentFrames = frames;
        frameIndex = 0;
        timer = 0;
    }

    private void Animate()
    {
        if (currentFrames == null || currentFrames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0;
            frameIndex = (frameIndex + 1) % currentFrames.Length;
            renderer.sprite = currentFrames[frameIndex];
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // optional stun radius indicator
    }
}