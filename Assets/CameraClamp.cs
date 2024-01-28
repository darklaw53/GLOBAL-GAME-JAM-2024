using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraClamp : MonoBehaviour
{
    public Transform topLeftLimit; 
    public Transform bottomRightLimit;

    public Transform target;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;

        float clampedX = Mathf.Clamp(targetPosition.x, topLeftLimit.position.x + camHalfWidth, bottomRightLimit.position.x - camHalfWidth);
        float clampedY = Mathf.Clamp(targetPosition.y, bottomRightLimit.position.y + camHalfHeight, topLeftLimit.position.y - camHalfHeight);

        Vector3 targetPositionClamped = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPositionClamped, ref velocity, smoothTime);
    }

    void OnDrawGizmos()
    {
        if (topLeftLimit != null && bottomRightLimit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(topLeftLimit.position, new Vector3(bottomRightLimit.position.x, topLeftLimit.position.y, 0));
            Gizmos.DrawLine(topLeftLimit.position, new Vector3(topLeftLimit.position.x, bottomRightLimit.position.y, 0));
            Gizmos.DrawLine(new Vector3(bottomRightLimit.position.x, topLeftLimit.position.y, 0), bottomRightLimit.position);
            Gizmos.DrawLine(new Vector3(topLeftLimit.position.x, bottomRightLimit.position.y, 0), bottomRightLimit.position);
        }
    }
}