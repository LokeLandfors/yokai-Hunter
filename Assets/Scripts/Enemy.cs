using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.2f;
    public float obstacleCheckDistance = 0.5f;

    private Rigidbody2D rb;
    private Transform player;
    private bool isFacingRight = false;
    private bool isGrounded = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform obstacleCheck;

    private Animator anim;
    private float attackCooldown = 1.5f;
    private bool canAttack = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("No GameObject named 'Player' found in scene!");
    }

    void Update()
    {
        if (player == null) return;

        CheckGround();
        FollowPlayer();
        CheckObstacle();
    }

    void CheckGround()
    {
        // Check if grounded
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    void CheckObstacle()
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

    void FollowPlayer()
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

    private System.Collections.IEnumerator Attack()
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

    private void OnDrawGizmosSelected()
    {
        // Debug rays for ground and obstacle checks
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

        if (obstacleCheck != null)
        {
            Vector3 dir = isFacingRight ? Vector3.right : Vector3.left;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(obstacleCheck.position, obstacleCheck.position + dir * obstacleCheckDistance);
        }
    }
}
