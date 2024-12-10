using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public float rotationSpeed = 20f; // Speed of rotation in degrees per second
    public float maxRotation = 110f; // Maximum rotation angle

    private float currentRotation = 0f; // Tracks the current rotation
    private bool rotatingForward = true; // Direction of rotation

    void Update()
    {
        RotateGuard();
    }

    void RotateGuard()
    {
        float rotationStep = rotationSpeed * Time.deltaTime; // Calculate rotation amount for this frame

        if (rotatingForward)
        {
            currentRotation += rotationStep;
            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotation -= rotationStep;
            if (currentRotation <= -maxRotation)
            {
                currentRotation = -maxRotation;
                rotatingForward = true;
            }
        }

        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}