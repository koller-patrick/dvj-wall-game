using UnityEngine;

/// <summary>
/// Ensures that the background object follows the camera's position.
/// Allows for an optional offset to adjust the background's relative position.
/// </summary>
public class BackgroundFollower : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera; // The camera the background will follow

    [Header("Offset Settings")]
    public Vector3 offset; // Optional offset for the background's position

    /// <summary>
    /// Initializes the script by setting the reference to the main camera if not explicitly assigned.
    /// </summary>
    void Start()
    {
        if (mainCamera == null)
        {
            // Automatically find the main camera if none is assigned
            mainCamera = Camera.main;
        }
    }

    /// <summary>
    /// Updates the position of the background object to match the camera's position in the LateUpdate phase.
    /// </summary>
    void LateUpdate()
    {
        // Calculate the new position based on the camera's position and the optional offset
        Vector3 newPosition = mainCamera.transform.position + offset;

        // Ensure the background stays at its original Z position (e.g., for parallax or rendering order)
        newPosition.z = 0;

        // Apply the new position to the background
        transform.position = newPosition;
    }
}
