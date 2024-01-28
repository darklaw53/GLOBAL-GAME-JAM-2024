using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] backgrounds; // Array of the parallax backgrounds to be scrolled
    public float[] parallaxScales;  // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    // How smooth the parallax effect will be. Higher means smoother.

    private Transform cam;          // Reference to the main camera's transform
    private Vector3 previousCamPos; // The position of the camera in the previous frame

    public Transform skyBox;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;

        // Assigning default parallax scales if not set
        if (parallaxScales == null || parallaxScales.Length == 0)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < parallaxScales.Length; i++)
            {
                parallaxScales[i] = i + 1; // Default to 1, 2, 3, ... for each layer
            }
        }
    }

    void Update()
    {
        skyBox.position = new Vector2(cam.position.x, skyBox.position.y);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i] * -1;

            // Set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}