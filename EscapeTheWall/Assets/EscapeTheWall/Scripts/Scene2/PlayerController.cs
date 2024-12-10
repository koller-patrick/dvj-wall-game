using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float ClimbSpeed = 5f;
    public float FallMultiplier = 2.5f;

    private Rigidbody2D rb;
    private bool canClimb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver())
        {
            return;
        }

        Move();
        HandleClimb();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * MoveSpeed, rb.velocity.y);
    }

    void HandleClimb()
    {
        if (canClimb)
        {
            float climbInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, climbInput * ClimbSpeed);
        }
        else if (!isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, (-1) * FallMultiplier);
        }

        transform.rotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canClimb = true; // Allow climbing when touching a wall
            rb.gravityScale = 0; // Disable gravity while climbing
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canClimb = false; // Disable climbing when not touching a wall
            rb.gravityScale = 1; // Re-enable gravity
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("VisionCone"))
        {
            Debug.Log("Player has been seen --> game over!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetGameOver();
            }
        }
        else if (other.CompareTag("Consumable"))
        {
            Consumable consumable = other.GetComponent<Consumable>();
            consumable.Consume();
        }
    }
}