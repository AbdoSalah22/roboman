using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public Transform player; // Player's position
    public float detectionRadius = 10f; // How far the enemy can spot the player
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Where to spawn the projectiles
    public float shootInterval = 1f; // Time interval between shots

    private float nextShootTime = 0f;

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius) // If the player is within the detection range
        {
            if (Time.time >= nextShootTime) // Shoot every `shootInterval` seconds
            {
                ShootProjectile();
                nextShootTime = Time.time + shootInterval; // Reset shoot time
            }
        }
    }

    void ShootProjectile()
    {
        // Create the projectile and point it at the player
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Calculate direction to the player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Add force to projectile towards player
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f; // Adjust speed as necessary
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
