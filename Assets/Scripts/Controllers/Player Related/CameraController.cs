using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform desiredCameraTransform; // Reference to the player's transform
    public float heightOffset = 2f; // Height offset from the player
    public float distanceOffset = 5f; // Distance offset from the player
    public float smoothSpeed = 1.25f; // Smoothness factor for movement
    public float rotationSmoothSpeed = 1.25f; // Smoothness factor for rotation
    
    

    void FixedUpdate()
    {
        if (desiredCameraTransform != null)
        {
            // Smoothly interpolate between current position and desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredCameraTransform.position, smoothSpeed);
            
            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;
            
            // Smoothly interpolate the camera's rotation towards the desired rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredCameraTransform.rotation, rotationSmoothSpeed);
        }
    }
}