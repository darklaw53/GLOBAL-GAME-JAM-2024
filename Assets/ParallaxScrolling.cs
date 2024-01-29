using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] backgrounds;
    public float[] parallaxScales;
    public float smoothing = 1f;

    private Transform cam;
    private Vector3 previousCamPos;

    public Transform skyBox;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;

        if (parallaxScales == null || parallaxScales.Length == 0)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < parallaxScales.Length; i++)
            {
                parallaxScales[i] = i + 1;
            }
        }
    }

    void FixedUpdate()
    {
        skyBox.position = new Vector2(cam.position.x, skyBox.position.y);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i] * -1;

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}