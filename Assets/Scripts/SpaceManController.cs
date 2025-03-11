using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceManController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform feetCheck;  // an empty object at the feet to check ground
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal move
        float horizontal = Input.GetAxisRaw("Horizontal") * moveSpeed;
        // Keep existing Y velocity for gravity
        rb.linearVelocity = new Vector2(horizontal, rb.linearVelocity.y);

        // Ground check (optional)
        isGrounded = Physics2D.OverlapCircle(feetCheck.position, 0.1f, groundLayer);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
