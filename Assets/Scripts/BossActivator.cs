using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject boss;  // Reference to the Boss GameObject

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);  // Enable the Boss
            gameObject.SetActive(false);  // Optional: Disable the door after triggering
        }
    }
}
