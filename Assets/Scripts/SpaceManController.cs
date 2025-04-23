using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(GatherInput))]
public class SpaceManController : MonoBehaviour
{
    [Header("Walk")]
    public float walkSpeed = 3f;

    [Header("Jump‑Charge")]
    public float chargeStep = 0.5f;   // power per frame
    public float maxJumpPower = 20f;
    [Range(0.1f, 10f)] public float minJumpPower = 0.5f;   // must exceed to fire

    public Vector2 minJumpForce = new Vector2(3f, 6f);    // arc at 1 %
    public Vector2 maxJumpForce = new Vector2(7f, 13f);   // arc at 100 %

    [Header("Ground Check")]
    public Transform leftRay;
    public Transform rightRay;
    public float rayLen = 0.1f;
    public LayerMask groundLayer;

    [Header("Physics Materials")]
    public PhysicsMaterial2D preJumpMat;   // friction 0
    public PhysicsMaterial2D normalMat;    // default grip

    [SerializeField] private float wallBouncePower = 1.5f; // tweak 0.5-2
    [SerializeField] float wallNormalThreshold = 0.9f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    GatherInput gi;
    Animator anim;

    bool grounded;
    bool charging;
    bool directionHeld;
    float jumpPower;
    int faceDir = 1;          // −1 / +1

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gi = GetComponent<GatherInput>();
        anim = GetComponent<Animator>();    // optional
    }

    void FixedUpdate()
    {
        CheckGround();
        HandleWalk();
        HandleJump();
        if (anim) UpdateAnimator();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Look at the first contact normal
        Vector2 n = col.GetContact(0).normal;

        // Only bounce if the surface is nearly vertical (|n.x| close to 1)
        if (Mathf.Abs(n.x) < wallNormalThreshold) return;

        // Horizontal kick away from the wall, keep current vertical speed
        float bounceX = wallBouncePower * Mathf.Sign(n.x); // n.x>0 => hit left wall
        rb.linearVelocity = new Vector2(bounceX, rb.linearVelocity.y);
    }

    /* ---------------- Ground check ---------------- */
    void CheckGround()
    {
        bool l = Physics2D.Raycast(leftRay.position, Vector2.down, rayLen, groundLayer);
        bool r = Physics2D.Raycast(rightRay.position, Vector2.down, rayLen, groundLayer);
        grounded = l || r;
#if UNITY_EDITOR
        Debug.DrawRay(leftRay.position, Vector2.down * rayLen, l ? Color.green : Color.red);
        Debug.DrawRay(rightRay.position, Vector2.down * rayLen, r ? Color.green : Color.red);
#endif
    }

    /* ---------------- Walking --------------------- */
    void HandleWalk()
    {
        if (grounded && !charging)
        {
            rb.linearVelocity = new Vector2(gi.valueX * walkSpeed, rb.linearVelocity.y);
            if (gi.valueX != 0) faceDir = gi.valueX < 0 ? -1 : 1;
        }
        sr.flipX = faceDir == -1;   // safe flip
    }

    /* ---------------- Jump‑charge ----------------- */
    void HandleJump()
    {
        if (gi.jumpInput && grounded && !charging)        // start charge
        {
            charging = true;
            jumpPower = 0f;
            directionHeld = Mathf.Abs(gi.valueX) > 0.01f;
            rb.linearVelocity = Vector2.zero;
            rb.sharedMaterial = preJumpMat;               // no friction
        }

        if (charging && gi.jumpInput)   {                  // hold
            jumpPower = Mathf.Min(jumpPower + chargeStep, maxJumpPower);
            if (!directionHeld && Mathf.Abs(gi.valueX) > 0.01f)
                directionHeld = true; 
        }

        if (charging && (!gi.jumpInput || jumpPower >= maxJumpPower))
        {
            if (jumpPower >= minJumpPower)
            {
                float t = jumpPower / maxJumpPower;       // 0‑1
                float horiz = directionHeld ? Mathf.Lerp(minJumpForce.x, maxJumpForce.x, t) * faceDir : 0f;    // no dir held → vertical
                
                Vector2 launch = new Vector2(
                horiz, Mathf.Lerp(minJumpForce.y, maxJumpForce.y, t));

                rb.linearVelocity = launch;
            }

            rb.sharedMaterial = normalMat;                // restore
            charging = false;
            jumpPower = 0f;
        }

        if (!grounded && rb.linearVelocity.y < -1f)             // falling
            rb.sharedMaterial = normalMat;
    }

    /* -------------- Animator hooks --------------- */
    void UpdateAnimator()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("vSpeed", rb.linearVelocity.y);
        anim.SetBool("grounded", grounded);
        anim.SetBool("preJump", charging);
    }
}
