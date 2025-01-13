using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private int facingDirection = 1; // 1 for right, -1 for left

    private int currentHealth = 80; // Current health
    public int maxHealth = 100; // Maximum health

    public float dashSpeed = 18f;  // Dash speed
    public float dashDuration = 0.3f; // Duration of the dash
    private bool isDashing;
    private float dashTime;

    public AudioClip JumpSound;

    public AudioClip DamageSound;

    public AudioClip DeathSound;

    public float wallBounceForce = 5f; // Force applied when bouncing off walls
    private bool isHittingWall = false;
    // private bool isJumping = false;
    private bool wasGrounded;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        wasGrounded = true;
    }

    void Update()
    {
        if (!isHittingWall) // Only allow movement control when not hitting a wall
        {
            Move();
        }
        Jump();
        Shoot();
        Dash();
        UpdateAnimator();
    }

    void Move()
    {
        if (isDashing) return;

        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (horizontal != 0)
        {
            facingDirection = (int)Mathf.Sign(horizontal);
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }



    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            SoundManager.instance.PlaySFX(JumpSound);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            isGrounded = false; // Immediately set to not grounded
            wasGrounded = false;
        }
    }

    void Shoot()
    {
        // Shoot a projectile when Fire1 is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("attack1");
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Projectile>().SetDirection(facingDirection); // Set projectile direction
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) // Trigger dash with Shift
        {
            isDashing = true;
            animator.SetTrigger("dash");  // Trigger dash animation
            dashTime = Time.time + dashDuration;
            rb.velocity = new Vector2(facingDirection * dashSpeed, rb.velocity.y);  // Dash in facing direction
        }

        // End dash after dash duration
        if (isDashing && Time.time >= dashTime)
        {
            isDashing = false;
        }
    }

    void UpdateAnimator()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        // Track when we first hit the ground
        if (isGrounded && !wasGrounded)
        {
            wasGrounded = true;
            animator.ResetTrigger("jump"); // Only reset jump trigger when landing
        }
        else if (!isGrounded)
        {
            wasGrounded = false;
        }

        animator.SetBool("isRunning", horizontal != 0);
        animator.SetBool("isGrounded", isGrounded);
    }

    public void TakeDamage(int damage)
    {
        SoundManager.instance.PlaySFX(DamageSound); // Play damage sound
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("takeDamage");
        }
    }

     public void Die()
    {

        SoundManager.instance.PlaySFX(DeathSound); // Play death sound
        // Trigger the death animation
        animator.SetTrigger("die");

        // Disable player controls
        this.enabled = false;
        rb.velocity = Vector2.zero;  // Stop movement

        // Start the coroutine to destroy the player after a delay
        StartCoroutine(DeathDelay());
    }

    // Coroutine to wait before destroying the player
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(1f);  // Wait for 1 second
        Destroy(gameObject);  // Destroy the player object
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                float angle = Vector2.Angle(contact.normal, Vector2.up);
                
                if (angle < 45f) // Collision from above/below
                {
                    isGrounded = true;
                    isHittingWall = false;
                }
                else // Collision from sides (wall)
                {
                    if (!isHittingWall)
                    {
                        isHittingWall = true;
                        float bounceDirection = Mathf.Sign(contact.normal.x);
                        rb.velocity = new Vector2(bounceDirection * wallBounceForce, rb.velocity.y);
                        facingDirection = (int)bounceDirection;
                        transform.localScale = new Vector3(facingDirection, 1, 1);
                        StartCoroutine(ResetWallHit());
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bool foundGroundContact = false;
            
            foreach (ContactPoint2D contact in collision.contacts)
            {
                float angle = Vector2.Angle(contact.normal, Vector2.up);
                if (angle < 45f)
                {
                    foundGroundContact = true;
                    isGrounded = true;
                    break;
                }
            }
            
            // If no ground contact points found, we might be against a wall
            if (!foundGroundContact && !isHittingWall)
            {
                isGrounded = false;
                // Start wall hit routine if we're pressing against the wall
                float horizontal = Input.GetAxis("Horizontal");
                if (Mathf.Abs(horizontal) > 0.1f)
                {
                    isHittingWall = true;
                    StartCoroutine(ResetWallHit());
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            isHittingWall = false;
        }
    }

    private IEnumerator ResetWallHit()
    {
        yield return new WaitForSeconds(0.2f); // Increased slightly to prevent rapid wall sticking
        isHittingWall = false;
    }

    public int getHealth()
    {
        return currentHealth;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }
}
