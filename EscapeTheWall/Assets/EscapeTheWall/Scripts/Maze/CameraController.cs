using UnityEngine;

// This script adjusts a material's mask center based on the player's position in the viewport.
public class CameraController : MonoBehaviour
{
    // Reference to the material used for masking.
    public Material maskMaterial;

    // Reference to the player's transform.
    public Transform player;

    void Update()
    {
        // Convert the player's world position to viewport coordinates (normalized screen coordinates).
        Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);

        // Set the mask material's "_Center" parameter to the player's viewport position.
        // The material shader should handle "_Center" for masking effects.
        maskMaterial.SetVector("_Center", new Vector4(playerViewportPos.x, playerViewportPos.y, 0, 0));
    }
}