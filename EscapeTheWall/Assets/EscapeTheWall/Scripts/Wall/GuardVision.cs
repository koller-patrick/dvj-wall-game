using UnityEngine;

/// <summary>
/// Controls the guard's vision cone or head rotation by oscillating between two angles.
/// </summary>
public class GuardVision : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 20f; // Speed at which the guard rotates (degrees per second)
    public float maxRotation = 110f; // Maximum rotation angle (in degrees) in either direction from the origin

    // Internal state
    private float currentRotation = 0f; // Tracks the guard's current rotation angle
    private bool rotatingForward = true; // Tracks the rotation direction (true = increasing, false = decreasing)

    /// <summary>
    /// Updates the guard's rotation every frame.
    /// </summary>
    void Update()
    {
        RotateGuard();
    }

    /// <summary>
    /// Rotates the guard back and forth between the specified maximum angles.
    /// </summary>
    void RotateGuard()
    {
        // Calculate the rotation step for the current frame
        float rotationStep = rotationSpeed * Time.deltaTime;

        if (rotatingForward)
        {
            // Increase the rotation angle
            currentRotation += rotationStep;

            // Check if the guard has reached the maximum rotation angle
            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation; // Clamp the rotation to the maximum
                rotatingForward = false; // Reverse the rotation direction
            }
        }
        else
        {
            // Decrease the rotation angle
            currentRotation -= rotationStep;

            // Check if the guard has reached the minimum rotation angle
            if (currentRotation <= -maxRotation)
            {
                currentRotation = -maxRotation; // Clamp the rotation to the minimum
                rotatingForward = true; // Reverse the rotation direction
            }
        }

        // Apply the rotation to the guard's transform
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
