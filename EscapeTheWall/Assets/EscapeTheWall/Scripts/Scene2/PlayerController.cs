using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float ClimbSpeed = 5f;
    public float FallMultiplier = 2.5f;
    public float JumpForce = 1f;

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
        HandleJump();
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
        else if (!isGrounded && !Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.down * FallMultiplier, ForceMode2D.Impulse);
        }

        transform.rotation = Quaternion.identity;
    }

    void HandleJump()
    {
        // Check if the player presses the jump key (Space by default)
        if (Input.GetButtonDown("Jump") && isGrounded && !canClimb)
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canClimb = true;
            rb.gravityScale = 0;
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
            canClimb = false;
            rb.gravityScale = 1;
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            rb.gravityScale = 1;
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