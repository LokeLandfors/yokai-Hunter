using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.2f;
    public float obstacleCheckDistance = 0.5f;
    [SerializeField] float attackCooldown;
    private bool canAttack = true;

    private Rigidbody2D rb;
    private Transform player;
    private bool isFacingRight = false;
    private bool isGrounded = false;

    [Header("Platforming settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform obstacleCheck;

    private Animator anim;


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            print("No player found");
    }

    public virtual void Update()
    {
        if (player == null) return;

        CheckGround();
        FollowPlayer();
        CheckObstacle();
    }

    public virtual void CheckGround()
    {
        // Check if grounded
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    public virtual void CheckObstacle()
    {
        // detect bump or obstacle
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(obstacleCheck.position, direction, obstacleCheckDistance, groundLayer);

        if (hit.collider != null && isGrounded)
        {
            // Jump over small bump
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public virtual void SearchPlayer()
    {

    }

    public virtual void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Attack when close
        if (distance <= attackRange)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (canAttack)
                StartCoroutine(Attack());
            return;
        }

        // Move toward player
        if (distance > attackRange)
        {
            float direction = player.position.x > transform.position.x ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

            // Flip sprite
            if (direction > 0 && !isFacingRight)
                Flip();
            else if (direction < 0 && isFacingRight)
                Flip();
        }
    }

    public virtual System.Collections.IEnumerator Attack()
    {
        canAttack = false;
        Debug.Log("Enemy Attacks!");

        if (anim != null)
            anim.SetTrigger("Attack");

        // LÄGG TILL DAMAGE HÄR

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }    
}
