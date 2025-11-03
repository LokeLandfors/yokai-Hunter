using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpForce = 10f;

    private bool isFacingRight = true;



    private Rigidbody2D rb;

    private bool isGrounded;

    [SerializeField, Range(0, 10)] float groundCheckOffset;

    public Slider usageWheel;
    private Animator animator;
    public SpriteRenderer sprite;
    public Animator anim;
    playerSlide slide;
    public SpriteRenderer regularCallspr;

    private bool canJump = true;
    private bool isJumping = false;

    public Vector3 respawnPoint;
    public GameObject fallDetector;
    [SerializeField] private Transform groundCheck;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.4f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.8f;
    private Vector2 wallJumpingPower = new Vector2(4f, 8f);

    private bool isWallSliding;
    private float wallSlidingSpeed = 0.8f;
    private float horizontal;


    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        slide = GetComponent<playerSlide>();
        respawnPoint = transform.position;


    }

    public void Reset()
    {
        transform.position = respawnPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision" + collision.gameObject.name);
        if (collision.gameObject.GetComponent<Interactable>() == true)
        {
            collision.gameObject.GetComponent<Interactable>().Interact(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        print("trigger" + collision.gameObject.name);
        if (collision.gameObject.GetComponent<Interactable>() == true)
        {
            collision.gameObject.GetComponent<Interactable>().Interact(this);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        float moveDirection = Input.GetAxis("Horizontal");


        Move(moveDirection);

        Jump();


        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
            {
                moveSpeed = walkSpeed;
            }
        }
        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    void Move(float direction)
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (slide.isSliding == false && Mathf.Abs(moveInput) > 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }


        float running = Mathf.Abs(direction * moveSpeed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            /* StartCoroutine(JumpCooldown());*/
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}