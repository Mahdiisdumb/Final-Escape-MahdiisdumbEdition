using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerDeath))]
public class Mahdi : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ability")]
    public float stunRadius = 3f;
    public LayerMask enemyLayer;
    public AudioSource stunSound;

    [Header("Sprites")]
    public SpriteRenderer renderer;
    public Sprite[] idleSprites;
    public Sprite[] walkSprites;
    public Sprite[] jumpSprites;
    public Sprite[] doubleJumpSprites;
    public Sprite[] skidSprites;
    public Sprite deathSprite; // Mahdi's death sprite
    public float frameRate = 0.1f;

    [Header("Particles")]
    public ParticleSystem runParticles;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private bool facingRight = true;
    private bool isDead = false;

    // Animation
    private Sprite[] currentFrames;
    private int frameIndex;
    private float timer;

    // PlayerDeath reference
    private PlayerDeath deathComponent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        deathComponent = GetComponent<PlayerDeath>();
        if (deathComponent != null)
        {
            // Set Mahdi's death sprite in the generic PlayerDeath component
            deathComponent.deathSprite = deathSprite;
        }
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleAbility();
        Animate();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (move > 0) facingRight = true;
        if (move < 0) facingRight = false;
        renderer.flipX = !facingRight;

        // Choose animation
        if (Mathf.Abs(move) < 0.01f)
        {
            SetAnimation(idleSprites);
            runParticles?.Stop();
        }
        else
        {
            SetAnimation(walkSprites);
            if (runParticles != null && !runParticles.isPlaying) runParticles.Play();
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount == 0)
            {
                Jump();
                jumpCount++;
                SetAnimation(jumpSprites);
            }
            else if (jumpCount == 1)
            {
                Jump();
                jumpCount++;
                SetAnimation(doubleJumpSprites);
                StunEnemies();
            }
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void HandleAbility()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetAnimation(skidSprites);
            StunEnemies();
        }
    }

    void StunEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stunRadius, enemyLayer);
        foreach (var hit in hits)
        {
            SonicEXE exe = hit.GetComponent<SonicEXE>();
            if (exe != null)
            {
                exe.Stun(2f); // stun for 2 seconds
            }
        }
    }

    void SetAnimation(Sprite[] frames)
    {
        if (currentFrames == frames) return;
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
            timer = 0;
            frameIndex = (frameIndex + 1) % currentFrames.Length;
            renderer.sprite = currentFrames[frameIndex];
        }
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) jumpCount = 0;
    }

    // --- Call this to kill Mahdi ---
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Let PlayerDeath handle everything
        if (deathComponent != null)
        {
            deathComponent.Die();
        }
        else
        {
            Debug.LogError("No PlayerDeath component attached!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
}