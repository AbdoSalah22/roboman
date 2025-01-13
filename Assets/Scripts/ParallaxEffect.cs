using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform player; // Reference to the player or camera
    public float[] parallaxScales; // Array for parallax effect intensity per layer
    public float smoothing = 1f; // Smoothing factor (should be > 0)

    private Vector3 previousPlayerPosition;

    void Start()
    {
        // Store the player's initial position
        previousPlayerPosition = player.position;

        // If no parallaxScales are set, initialize with default values
        if (parallaxScales.Length == 0)
        {
            parallaxScales = new float[transform.childCount];
            for (int i = 0; i < parallaxScales.Length; i++)
                parallaxScales[i] = (i + 1) * 0.1f; // Gradual increase in parallax effect
        }
    }

    void Update()
    {
        // Calculate the player's movement since the last frame
        Vector3 deltaMovement = player.position - previousPlayerPosition;

        // Apply parallax to each child layer
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform layer = transform.GetChild(i);
            float parallax = deltaMovement.x * parallaxScales[i];
            Vector3 newPosition = new Vector3(layer.position.x + parallax, layer.position.y, layer.position.z);

            // Smoothly move the layer to the new position
            layer.position = Vector3.Lerp(layer.position, newPosition, smoothing * Time.deltaTime);
        }

        // Update the player's previous position
        previousPlayerPosition = player.position;
    }
}
