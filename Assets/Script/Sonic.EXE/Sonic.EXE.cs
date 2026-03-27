using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SonicEXE : MonoBehaviour
{
    public Transform player;
    public float speed = 6f;

    [Header("Sprites")]
    public Sprite[] flyingSprites;
    public Sprite[] stunnedSprites;
    public float frameRate = 0.1f;

    private SpriteRenderer renderer;
    private Sprite[] currentFrames;
    private int frameIndex;
    private float timer;
    private bool isStunned = false;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();

        // Ensure Collider2D is trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Ensure Rigidbody2D exists and is kinematic
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        SetAnimation(flyingSprites);
    }

    void Update()
    {
        Animate();
        if (player == null || isStunned) return;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    public void Stun(float duration)
    {
        if (isStunned) return;
        isStunned = true;
        SetAnimation(stunnedSprites);
        Invoke(nameof(Recover), duration);
    }

    void Recover()
    {
        isStunned = false;
        SetAnimation(flyingSprites);
    }

    void SetAnimation(Sprite[] frames)
    {
        currentFrames = frames;
        frameIndex = 0;
        timer = 0;
    }

    void Animate()
    {
        if (currentFrames == null || currentFrames.Length == 0) return;
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % currentFrames.Length;
            renderer.sprite = currentFrames[frameIndex];
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isStunned && col.CompareTag("Player"))
        {
            PlayerDeath death = col.GetComponent<PlayerDeath>();
            if (death != null)
                death.Die();
        }
    }
}