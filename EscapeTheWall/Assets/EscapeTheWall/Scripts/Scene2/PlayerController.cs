using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float ClimbForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameOver())
        {
            Move();

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Climb();
            }
        }
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * MoveSpeed, 0);
    }

    void Climb()
    {
        rb.AddForce(new Vector2(rb.velocity.x, ClimbForce), ForceMode2D.Impulse);
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
            GameManager.Instance.SetGameOver();
        }
    }
}
