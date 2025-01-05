using UnityEngine;

public class ChaserAI : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public Transform waypoint1, waypoint2; // Assign waypoints in the Inspector
    public float patrolSpeed = 3f;
    public float chaseSpeed = 6f;
    public float detectionRadius = 20f;
    public float stopChaseRadius = 25f;
    public int explosionDamage = 10;

    private Vector3 nextWaypoint;
    private bool isChasing = false;

    [SerializeField] GameObject explosion;

    void Start()
    {
        nextWaypoint = waypoint1.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > stopChaseRadius)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextWaypoint, patrolSpeed * Time.deltaTime);

        // Switch waypoints when the enemy reaches the current one
        if (Vector2.Distance(transform.position, nextWaypoint) < 0.5f)
        {
            nextWaypoint = nextWaypoint == waypoint1.position ? waypoint2.position : waypoint1.position;
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(explosionDamage);
            }

            // Trigger explosion effect (optional)
            Explode();

            // Destroy the enemy
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Debug.Log("Enemy exploded!");
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopChaseRadius);
    }
}
