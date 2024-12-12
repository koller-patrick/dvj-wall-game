using UnityEngine;

public class CameraSideViewController : MonoBehaviour
{
    public Transform player;
    public float horizontalOffset = 2f;
    public float verticalOffset = 1f;
    public float zoomLevel = 5f; // Lower values = zoomed in, higher values = zoomed out

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam != null)
        {
            cam.orthographicSize = zoomLevel;
        }
    }

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned in CameraController script.");
            return;
        }

        // New target position with offset
        Vector3 targetPosition = new Vector3(
            player.position.x + horizontalOffset,
            player.position.y + verticalOffset,
            transform.position.z // Keep the camera's current Z position
        );

        transform.position = targetPosition;
    }
}
