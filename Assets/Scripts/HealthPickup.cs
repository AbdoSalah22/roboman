using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 25;

    public bool enableHoverEffect = true;

    public float hoverHeight = 0.7f;

    public float hoverSpeed = 2f;

    public bool enableRotation = true;

    public float rotationSpeed = 90f;

    private Vector3 startPosition;
    private float timeOffset;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2); // Random start position in sine wave
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure there's a Collider2D component
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
    }

    void Update()
    {
        if (enableHoverEffect)
        {
            // Smooth floating motion
            float yOffset = Mathf.Sin((Time.time + timeOffset) * hoverSpeed) * hoverHeight;
            transform.position = startPosition + Vector3.up * yOffset;
        }

        if (enableRotation)
        {
            // Smooth rotation
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            
            if (player != null && player.getHealth() < player.getMaxHealth())
            {
                // Only heal if player isn't at full health
                player.Heal(healthAmount);
                
                // Optional: Add visual feedback before destroying
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }
                
                Destroy(gameObject);
            }
        }
    }
}