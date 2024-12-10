using UnityEngine;

public class PlayerMovement : MovingPieces
{
    private Animator animator;
    private bool isMoving = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is not already moving
        if (!isMoving && !GameManager.Instance.IsGameOver())
        {
            Vector2 addThisVector = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.W))
            {
                addThisVector.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                addThisVector.y = -1;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                addThisVector.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                addThisVector.x = 1;
            }

            if (addThisVector != Vector2.zero)
            {
                // Set isMoving to true to prevent multiple moves at once
                isMoving = true;

                // Calculate new position
                Vector2 newPosition = (Vector2)transform.position + addThisVector;

                // Check what is in the target position
                GameObject whatIsInFront = WhatInThisPosition(newPosition);
                if (whatIsInFront == null)
                {
                    // Move to the new position if there's nothing in front
                    transform.position = newPosition;
                }
                else if (whatIsInFront.CompareTag("Consumable"))
                {
                    // If there's a consumable, consume it and move
                    Consumable consumable = whatIsInFront.GetComponent<Consumable>();
                    consumable.Consume();
                    transform.position = newPosition;
                }

                // Set isMoving to false after finishing the movement (simulating the move completion)
                isMoving = false;
            }
        }
    }
}


