using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Access the player's health and trigger death
            other.GetComponent<PlayerController>().Die();
        }
    }
}
