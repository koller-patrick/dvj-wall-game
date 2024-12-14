using UnityEngine;

/// <summary>
/// Handles the player's movement and interactions with objects in a 2D space.
/// Inherits from the MovingPieces class for positional checks.
/// </summary>
public class PlayerMovement : MovingPieces
{
    // Reference to the Animator component
    private Animator animator;

    // Indicates whether the player is currently moving
    private bool isMoving = false;

    /// <summary>
    /// Initializes the player's animator and sets its speed.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from the player.");
            return;
        }

        animator.speed = 2; // Speed up animations if needed
    }

    /// <summary>
    /// Handles player input, movement, and interactions with objects.
    /// </summary>
    void Update()
    {
        // Proceed only if the player is not already moving and the game is not over
        if (!isMoving && !GameManager.Instance.IsGameOver())
        {
            Vector2 movementDirection = Vector2.zero;

            // Detect movement input
            if (Input.GetKeyDown(KeyCode.W))
            {
                movementDirection.y = 1; // Move up
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movementDirection.y = -1; // Move down
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                movementDirection.x = -1; // Move left
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                movementDirection.x = 1; // Move right
            }

            // If movement input is detected
            if (movementDirection != Vector2.zero)
            {
                isMoving = true; // Lock movement

                // Calculate the target position
                Vector2 targetPosition = (Vector2)transform.position + movementDirection;

                // Check for objects at the target position
                GameObject objectAtTarget = WhatInThisPosition(targetPosition);
                if (objectAtTarget == null)
                {
                    // No object in the way, move the player
                    animator.SetTrigger("Walk");

                    // Rotate the player to face the movement direction
                    float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg - 90f;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);

                    // Update the player's position
                    transform.position = targetPosition;
                }
                else if (objectAtTarget.CompareTag("Consumable"))
                {
                    // If the object is a consumable, consume it and move
                    Consumable consumable = objectAtTarget.GetComponent<Consumable>();
                    if (consumable != null)
                    {
                        consumable.Consume();
                        transform.position = targetPosition;
                    }
                    else
                    {
                        Debug.LogWarning("Object tagged as Consumable is missing a Consumable component.");
                    }
                }

                isMoving = false; // Unlock movement after action
            }
        }
    }
}


