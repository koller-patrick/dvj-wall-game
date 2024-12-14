using UnityEngine;

/// <summary>
/// Controls a side-view camera that follows the player with configurable offsets and zoom level.
/// </summary>
public class CameraSideViewController : MonoBehaviour
{
    [Header("Player Tracking")]
    public Transform player; // Reference to the player's Transform for tracking

    [Header("Camera Offsets")]
    public float horizontalOffset = 2f; // Horizontal distance from the player
    public float verticalOffset = 1f;   // Vertical distance from the player

    [Header("Camera Zoom")]
    public float zoomLevel = 5f; // Orthographic camera size (lower = closer zoom, higher = farther zoom)

    private Camera cam; // Reference to the Camera component

    /// <summary>
    /// Initializes the camera settings, including the zoom level.
    /// </summary>
    void Start()
    {
        // Get the main camera component
        cam = Camera.main;

        if (cam != null)
        {
            // Set the initial orthographic zoom level
            cam.orthographicSize = zoomLevel;
        }
        else
        {
            Debug.LogError("Main camera not found. Ensure your scene has a camera tagged as MainCamera.");
        }
    }

    /// <summary>
    /// Updates the camera's position to follow the player with the specified offsets.
    /// LateUpdate ensures smooth movement after the player has moved.
    /// </summary>
    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned in CameraSideViewController script.");
            return;
        }

        // Calculate the new target position based on the player's position and offsets
        Vector3 targetPosition = new Vector3(
            player.position.x + horizontalOffset, // Apply horizontal offset
            player.position.y + verticalOffset,   // Apply vertical offset
            transform.position.z                  // Keep the camera's Z position constant
        );

        // Update the camera's position
        transform.position = targetPosition;
    }
}
