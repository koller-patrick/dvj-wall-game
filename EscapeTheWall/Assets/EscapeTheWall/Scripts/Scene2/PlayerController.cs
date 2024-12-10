using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float ClimbSpeed = 5f;
    public float FallMultiplier = 2.5f;

    private Rigidbody2D rb;
    private bool canClimb;

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
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canClimb = true; // Allow climbing when touching a wall
            rb.gravityScale = 0; // Disable gravity while climbing
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canClimb = false; // Disable climbing when not touching a wall
            rb.gravityScale = 1; // Re-enable gravity
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
    }
}