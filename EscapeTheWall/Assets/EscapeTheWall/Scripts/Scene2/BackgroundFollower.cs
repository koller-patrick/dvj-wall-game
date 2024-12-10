using UnityEngine;

public class BackgroundFollower : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 offset; // Optional, if you want the background to have a certain offset.

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        // Match the background position to the camera position, plus any offset
        transform.position = mainCamera.transform.position + offset;
    }
}