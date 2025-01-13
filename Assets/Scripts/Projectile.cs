using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifetime = 5f; // Lifetime in seconds
    private int direction = 1;
    public AudioClip laserSound;

    void Start()
    {
        //SoundManager.instance.PlaySFX(laserSound); // Play laser sound
        Destroy(gameObject, lifetime); // Destroy projectile after `lifetime` seconds
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * direction);
    }

    public void SetDirection(int dir)
    {
        direction = dir;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir; // Flip sprite if necessary
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Get the EnemyHealth component and apply damage
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the projectile upon hitting the enemy
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
