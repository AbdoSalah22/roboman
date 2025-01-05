using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 5; // The maximum health of the enemy

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play any death animation or sound here if needed
        Debug.Log("Enemy destroyed!");
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
