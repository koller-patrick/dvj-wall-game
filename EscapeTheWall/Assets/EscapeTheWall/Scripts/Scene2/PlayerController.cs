using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float JumpForce = 1f;
    public float FallMultiplier = 2.5f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver())
            return;

        if (!isGrounded)
        {
            // Make player fall
            rb.AddForce(new Vector2(0, -1 * FallMultiplier));
        }

        Move();
        HandleJump();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        rb.AddForce(new Vector2(move * MoveSpeed, 0));
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
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
    }
}
