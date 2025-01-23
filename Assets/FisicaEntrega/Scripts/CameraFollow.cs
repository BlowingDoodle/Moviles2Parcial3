using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // Helicopter to follow

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Offset relative to the target
    [SerializeField] private float followSpeed = 5f; // Speed of following the target
    [SerializeField] private float rotationSpeed = 5f; // Speed of rotating the camera

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned to the CameraFollow script.");
            return;
        }

        // Smoothly move the camera to the target position + offset
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to look at the target
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}

