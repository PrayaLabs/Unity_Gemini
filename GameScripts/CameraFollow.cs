using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Drag your Sphere here
    public Vector3 offset = new Vector3(0, 5, -7);

    [Range(1f, 20f)]
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;

        // Always look at the target
        transform.LookAt(target);
    }
}