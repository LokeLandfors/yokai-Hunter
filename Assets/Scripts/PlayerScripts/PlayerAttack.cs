using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    // gjort av wayk
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float CooldownBtwAttacks = 0.15f;
    [SerializeField] private playerSlide slideCode;
    [SerializeField] private PlayerMovement moveCode;
    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private float attackDelay = 0.8f; 
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool canAttack = true;

    private RaycastHit2D[] hits;
    private Animator anim;
    private float attackTimeCounter;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame() && canAttack && !slideCode.isSliding && !moveCode.isDashing)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private void Attack()
    {
        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.Damage(damageAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        slideCode.isSliding = false;
        moveCode.isDashing = false;
        isAttacking = true;
        moveCode.canMove = false;
        attackTimeCounter = 0f;

        anim.SetTrigger("attack");


        if (attackDelay > 0f)
            yield return new WaitForSeconds(attackDelay);

        Attack();


        if (attackDuration > attackDelay)
            yield return new WaitForSeconds(attackDuration - attackDelay);

        moveCode.canMove = true;
        isAttacking = false;
        slideCode.isSliding = false;
        moveCode.isDashing = false;

        yield return new WaitForSeconds(CooldownBtwAttacks);
        canAttack = true;
    }
}
