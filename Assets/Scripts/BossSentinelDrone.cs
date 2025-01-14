using System.Collections;
using UnityEngine;

public class BossSentinelDrone : MonoBehaviour
{
    private int maxHealth = 200;
    private int currentHealth;

    public float hoverSpeed = 2f;
    public float hoverRange = 3f;
    private Vector3 startPosition;
    private bool movingUp = true;

    public GameObject bulletPrefab;
    public Transform firePoint;
    private float fireRate = 0.5f;
    private float nextFireTime;

    public GameObject spreadShotPrefab;
    public float dashSpeed = 8f;
    public float dashCooldown = 3f;
    private bool isDashing;

    private Transform player;
    private Rigidbody2D rb;

    private bool isPhaseTwo = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;

        HoverMovement();
        AttackPattern();
    }

    void HoverMovement()
    {
        float newY = transform.position.y + (movingUp ? hoverSpeed : -hoverSpeed) * Time.deltaTime;
        if (newY > startPosition.y + hoverRange)
            movingUp = false;
        else if (newY < startPosition.y - hoverRange)
            movingUp = true;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void AttackPattern()
    {
        if (player != null)
        {
            if (Time.time >= nextFireTime)
            {
                if (isPhaseTwo)
                {
                    StartCoroutine(SpreadShot());
                }
                else
                {
                    ShootAtPlayer();
                }
                nextFireTime = Time.time + 1f / fireRate;
            }

            if (isPhaseTwo && !isDashing)
            {
                StartCoroutine(DashAttack());
            }
        }
    }

    void ShootAtPlayer()
    {
        if ( player != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 6f;
        }
    }

    IEnumerator SpreadShot()
    {
        int bulletCount = 8;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulDirX = Mathf.Cos((angle * Mathf.PI) / 180f);
            float bulDirY = Mathf.Sin((angle * Mathf.PI) / 180f);
            Vector2 bulMoveVector = new Vector2(bulDirX, bulDirY).normalized;

            GameObject bul = Instantiate(spreadShotPrefab, firePoint.position, Quaternion.identity);
            bul.GetComponent<Rigidbody2D>().velocity = bulMoveVector * 4f;

            angle += angleStep;
        }
        yield return null;
    }

    IEnumerator DashAttack()
    {
        isDashing = true;
        Vector2 dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= maxHealth / 2 && !isPhaseTwo)
        {
            EnterPhaseTwo();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void EnterPhaseTwo()
    {
        isPhaseTwo = true;
        fireRate *= 1.0f;
        dashSpeed *= 1.25f;
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        // Add death animation or explosion effect here
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(10);
            Destroy(other.gameObject);
        }
    }
}
