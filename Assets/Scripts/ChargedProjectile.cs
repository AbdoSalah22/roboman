using UnityEngine;

public class ChargedProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 20;
    private int direction;
    public AudioClip laserSound;

    void Start()
    {
        SoundManager.instance.PlaySFX(laserSound); // Play laser sound
        Destroy(gameObject, 10); // Destroy projectile after `lifetime` seconds
    }

    public void SetDirection(int dir)
    {
        direction = dir;
        // Optional: Flip sprite based on direction
        if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Update()
    {
        // Move the projectile
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            // Assuming enemies have a method to take damage
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            // Note: Don't destroy the projectile here since it should continue until hitting ground
        }
    }
}