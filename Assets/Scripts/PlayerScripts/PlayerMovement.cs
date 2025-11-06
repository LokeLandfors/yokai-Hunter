using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool canMove = true;
    public float moveSpeed = 5f;
    public float walkSpeed = 5f;
    public float runSpeed = 5f;
    public float jumpForce = 10f;

    private bool isFacingRight = true;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canJump = true;
    private bool isJumping = false;
    private float horizontal;

    [SerializeField, Range(0, 10)] float groundCheckOffset;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;

    public Slider usageWheel;
    private Animator animator;
    public SpriteRenderer sprite;
    public Animator anim;
    playerSlide slide;
    [SerializeField] PlayerAttack atkCode;
    public SpriteRenderer regularCallspr;

    public Vector3 respawnPoint;
    public GameObject fallDetector;

    [Header("Wall Jump Settings")]
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.4f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.8f;
    private Vector2 wallJumpingPower = new Vector2(4f, 8f);
    private bool isWallSliding;
    private float wallSlidingSpeed = 2.5f; // smoother slide av wayk

    // wall jump cooldown av wayk
    [SerializeField] float wallJumpCooldown = 0.6f;
    private bool canWallJump = true;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool isDashing = false;
    private bool canDash = true;

    void Start() // wayk
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        slide = GetComponent<playerSlide>();
        respawnPoint = transform.position;
    }

    public void Reset() // wayk
    {
        transform.position = respawnPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision) // wayk
    {
        if (collision.gameObject.GetComponent<Interactable>() == true)
        {
            collision.gameObject.GetComponent<Interactable>().Interact(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // wayk
    {
        if (collision.gameObject.GetComponent<Interactable>() == true)
        {
            collision.gameObject.GetComponent<Interactable>().Interact(this);
        }
    }

    private bool IsGrounded() // wayk
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled() // wayk
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    void Update()
    {
        // ta bort kontroller under dash // wayk
        if (isDashing) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        float moveDirection = Input.GetAxis("Horizontal");

        Move(moveDirection);
        Jump();
        WallSlide();
        WallJump();

        if (!isWallJumping)
            Flip();

        // --- DASH INPUT --- (loke)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing && slide.isSliding == false && atkCode.isAttacking == false)
        {
            StartCoroutine(PerformDash());
        }

        // wayk
        if (slide != null && Input.GetKeyDown(KeyCode.LeftControl))
        {
            slide.isSliding = true;
        }
    }

    void Move(float direction) // wayk
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (canMove == true && (slide == null || slide.isSliding == false))
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void Jump() // wayk
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()
            )
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void WallSlide() // wayk
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(0f, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }


    Vector2 lastWallJPos;
    private void WallJump() // wayk
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -Mathf.Sign(transform.localScale.x);
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && canWallJump && (transform.position.x <= lastWallJPos.x - 1 || transform.position.x >= lastWallJPos.x + 1)) // fantastic tweaks av Edwin
        {
            StartCoroutine(WallJumpCooldownRoutine()); // start cooldown av wayk


            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            lastWallJPos = transform.position;
            // flip
            if ((isFacingRight && wallJumpingDirection < 0) || (!isFacingRight && wallJumpingDirection > 0))
            {
                Flip();
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private IEnumerator WallJumpCooldownRoutine() // wayk
    {
        canWallJump = false;
        yield return new WaitForSeconds(wallJumpCooldown);
        canWallJump = true;
    }

    private void StopWallJumping() // wayk
    {
        isWallJumping = false;
    }

    private void Flip() // wayk
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // --- DASH LOGIC --- (loke) (stretch av wayk)
    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 stretchedScale = new Vector3(originalScale.x * 1.2f, originalScale.y, originalScale.z);
        transform.localScale = stretchedScale;

        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        transform.localScale = originalScale;

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(wallCheck.position, 0.2f); //Rita wallcheck cirkel
    }
}
