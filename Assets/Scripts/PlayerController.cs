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

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        Shoot();
        Dash();
        UpdateAnimator();
    }

    void Move()
    {
        if (isDashing) return;  // Skip movement if dashing

        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Flip the sprite based on movement direction
        if (horizontal != 0)
        {
            facingDirection = (int)Mathf.Sign(horizontal); // 1 for right, -1 for left
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }

    void Jump()
    {
        // Check for jumping input and whether the player is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            SoundManager.instance.PlaySFX(JumpSound); // Play jump sound
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump"); // Trigger jump animation
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

        // Update the animator parameters
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
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
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
