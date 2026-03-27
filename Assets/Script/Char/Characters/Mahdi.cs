using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerDeath))]
public class Mahdi : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float acceleration = 8f;
    public float jumpForce = 12f;
    public float skidThreshold = 5f;

    [Header("Ability")]
    public float stunRadius = 3f;
    public string enemyTag = "Enemy"; // tag-based detection
    public AudioSource stunSound;
    public Color stunGizmoColor = new Color(1, 0, 0, 0.3f); // semi-transparent red

    [Header("Sprites")]
    public SpriteRenderer renderer;
    public Sprite[] idleSprites;
    public Sprite[] walkSprites;
    public Sprite[] runSprites;
    public Sprite[] jumpSprites;
    public Sprite[] doubleJumpSprites;
    public Sprite[] skidSprites;
    public Sprite deathSprite;
    public float frameRate = 0.1f;

    [Header("Particles")]
    public ParticleSystem runParticles;

    private Rigidbody2D rb;
    private PlayerDeath deathComponent;
    private int jumpCount = 0;
    private bool facingRight = true;
    private bool isDead = false;

    // Animation
    private Sprite[] currentFrames;
    private int frameIndex;
    private float timer;

    // State control
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool isSkidding = false;
    private float moveInput = 0f;
    private float lastMoveInput = 0f;
    private float currentSpeed = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        deathComponent = GetComponent<PlayerDeath>();
        if (deathComponent != null)
            deathComponent.deathSprite = deathSprite;
    }

    void Update()
    {
        if (isDead) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        HandleMovement();
        HandleJump();
        Animate();
    }

    void HandleMovement()
    {
        // Smooth acceleration toward target speed
        float targetSpeed = Mathf.Abs(moveInput) * runSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (moveInput > 0) facingRight = true;
        else if (moveInput < 0) facingRight = false;
        renderer.flipX = !facingRight;

        // Skid detection
        isSkidding = Mathf.Abs(moveInput) > 0.1f &&
                     Mathf.Sign(moveInput) != Mathf.Sign(lastMoveInput) &&
                     Mathf.Abs(rb.linearVelocity.x) > skidThreshold;

        lastMoveInput = moveInput;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (jumpCount == 0)
            {
                Jump();
                jumpCount++;
                isJumping = true;
            }
            else if (jumpCount == 1)
            {
                Jump();
                jumpCount++;
                isDoubleJumping = true;
                StunEnemies();
            }
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void StunEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stunRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                SonicEXE exe = hit.GetComponent<SonicEXE>();
                if (exe != null)
                    exe.Stun(2f);
            }
        }
        stunSound?.Play();
    }

    void Animate()
    {
        // Decide which animation to play based on state
        Sprite[] targetFrames = idleSprites;

        if (isSkidding) targetFrames = skidSprites;
        else if (isDoubleJumping) targetFrames = doubleJumpSprites;
        else if (isJumping) targetFrames = jumpSprites;
        else
        {
            float absSpeed = Mathf.Abs(rb.linearVelocity.x);
            if (absSpeed < 0.1f) targetFrames = idleSprites;
            else if (absSpeed < runSpeed * 0.6f) targetFrames = walkSprites;
            else targetFrames = runSprites;
        }

        if (currentFrames != targetFrames)
        {
            currentFrames = targetFrames;
            frameIndex = 0;
            timer = 0;
        }

        // Animate frames
        if (currentFrames.Length == 0) return;
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % currentFrames.Length;
            renderer.sprite = currentFrames[frameIndex];
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            isJumping = false;
            isDoubleJumping = false;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        deathComponent?.Die();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = stunGizmoColor;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
}