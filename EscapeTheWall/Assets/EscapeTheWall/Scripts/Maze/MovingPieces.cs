using UnityEngine;

/// <summary>
/// Provides functionality to detect objects at a given position in a 2D space.
/// </summary>
public class MovingPieces : MonoBehaviour
{
    /// <summary>
    /// Checks what object is present at the given position, excluding the current object.
    /// </summary>
    /// <param name="position">The world position to check for objects.</param>
    /// <returns>The first game object found at the specified position, or null if none are found.</returns>
    protected GameObject WhatInThisPosition(Vector2 position)
    {
        // Get all colliders at the given position
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);

        // Iterate through the colliders to find an object that is not this game object
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return collider.gameObject; // Return the first valid game object
            }
        }

        // Return null if no valid game objects are found
        return null;
    }
}
