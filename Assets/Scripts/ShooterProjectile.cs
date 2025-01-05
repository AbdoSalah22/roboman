using UnityEngine;

public class ShooterProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, 8); // Destroy projectile after `lifetime` seconds
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy projectile on collision
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
