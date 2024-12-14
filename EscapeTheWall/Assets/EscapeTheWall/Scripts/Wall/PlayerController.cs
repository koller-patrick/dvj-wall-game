using UnityEngine;

/// <summary>
/// Controls player movement, jumping, climbing, and interactions with objects in a 2D game environment.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;       // Horizontal movement speed
    public float ClimbSpeed = 5f;      // Climbing speed
    public float FallMultiplier = 2.5f; // Additional downward force for faster falling
    public float JumpForce = 1f;       // Upward force applied when jumping

    private Rigidbody2D rb;           // Reference to the Rigidbody2D component
    private Animator animator;        // Reference to the Animator component
    private bool canClimb;            // Tracks if the player can climb (near a climbable surface)
    private bool isGrounded;          // Tracks if the player is touching the ground

    /// <summary>
    /// Initializes the player controller by fetching necessary components.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Updates the player's movement and interaction logic every frame.
    /// Prevents interaction if the game is over.
    /// </summary>
    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver())
        {
            return; // Exit if the game is over
        }

        Move();
        HandleClimb();
        HandleJump();
    }

    /// <summary>
    /// Handles horizontal movement of the player based on input.
    /// </summary>
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Get input for horizontal movement (-1 to 1)
        rb.velocity = new Vector2(moveInput * MoveSpeed, rb.velocity.y); // Apply movement to the Rigidbody
        animator.SetTrigger("Walk"); // Trigger the walking animation
    }

    /// <summary>
    /// Handles climbing logic when the player is near a climbable surface.
    /// </summary>
    void HandleClimb()
    {
        if (canClimb)
        {
            // Climb the wall if vertical input is provided
            float climbInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, climbInput * ClimbSpeed);
        }
        else if (!isGrounded && !Input.GetButtonDown("Jump"))
        {
            // Apply downward force for faster falling
            rb.AddForce(Vector2.down * FallMultiplier, ForceMode2D.Impulse);
        }

        // Reset rotation to ensure the player stays upright
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Handles jumping logic when the player is grounded and not climbing.
    /// </summary>
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !canClimb)
        {
            // Apply upward force to make the player jump
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Detects collision with ground or climbable walls to update movement states.
    /// </summary>
    /// <param name="collision">Collision data from the 2D physics system.</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Enable climbing when colliding with a wall
            canClimb = true;
            rb.gravityScale = 0; // Disable gravity while climbing
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Mark the player as grounded
            isGrounded = true;
        }
    }

    /// <summary>
    /// Updates player state when leaving a collision.
    /// </summary>
    /// <param name="collision">Collision data from the 2D physics system.</param>
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Disable climbing when leaving the wall
            canClimb = false;
            rb.gravityScale = 1; // Re-enable gravity
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Mark the player as not grounded
            isGrounded = false;
            rb.gravityScale = 1; // Ensure gravity is enabled
        }
    }

    /// <summary>
    /// Handles interactions when the player enters a trigger zone.
    /// </summary>
    /// <param name="other">Collider data from the 2D physics system.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("VisionCone"))
        {
            // Trigger game over if detected by a vision cone
            Debug.Log("Player has been seen --> game over!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetGameOver();
                rb.velocity = Vector2.zero; // Stop movement
            }
        }
        else if (other.CompareTag("Consumable"))
        {
            // Consume the item if tagged as "Consumable"
            Consumable consumable = other.GetComponent<Consumable>();
            if (consumable != null)
            {
                consumable.Consume();
            }
        }
        else if (other.CompareTag("BarbedWire"))
        {
            // Trigger game over if the player touches barbed wire
            Debug.Log("Player has touched barbed wire --> game over!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetGameOver();
                rb.velocity = Vector2.zero; // Stop movement
            }
        }
    }
}
