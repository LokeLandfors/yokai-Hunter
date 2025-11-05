using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSlide : MonoBehaviour
{

    public bool isSliding = false;
    public PlayerMovement PL;
    public Rigidbody2D rgb;
    public CapsuleCollider2D regularCall;
    public BoxCollider2D slideCall;
    public SpriteRenderer regularCallspr;
    public SpriteRenderer slideCallspr;
    private bool canSlide = true;
    public float slidespeed = 1800f;
    [SerializeField] GameObject slideCol;
    [SerializeField] private PlayerMovement moveCode;
    [SerializeField] PlayerAttack AttackCode;
    private bool IsFacingRight = true; // För Slide


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            if (canSlide == true && moveCode.isDashing == false && AttackCode.isAttacking == false)
            {
                PerfromSlide();
            }
        if (Input.GetKeyDown(KeyCode.A))
        {
            IsFacingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            IsFacingRight = true;
        }
    }


    private void PerfromSlide()
    {
        isSliding = true;
        regularCall.enabled = false;
        regularCallspr.enabled = false;
        slideCall.enabled = true;
        slideCallspr.enabled = true;

        if (IsFacingRight == true)
        {
            rgb.AddForce(Vector2.right * slidespeed);
            slideCol.GetComponent<SpriteRenderer>().flipY  = false; 
        }
        else
        {
            rgb.AddForce(Vector2.left * slidespeed);
            slideCol.GetComponent<SpriteRenderer>().flipY = false;
        }
        StartCoroutine("stopSlide");
    }

    IEnumerator stopSlide()
    {
        canSlide = false;
        yield return new WaitForSeconds(0.8f);
        regularCall.enabled = true;
        regularCallspr.enabled = true;
        slideCall.enabled = false;
        slideCallspr.enabled = false;
        isSliding = false;
        yield return new WaitForSeconds(0.5f);
        canSlide = true;
    }

}
