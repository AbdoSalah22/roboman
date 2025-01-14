using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgrounds;  // Array for background layers
    public float[] parallaxScales;   // Parallax effect for each layer
    public float smoothing = 1f;     // Smoothing effect

    private Transform cam;           // Reference to the main camera
    private Vector3 previousCamPos;  // Camera's position in the last frame

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float targetX = backgrounds[i].position.x + parallax;
            Vector3 targetPos = new Vector3(targetX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
