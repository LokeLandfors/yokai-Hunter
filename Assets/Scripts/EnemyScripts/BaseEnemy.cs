using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class BaseEnemy : EnemyCoreLogic
    //så jag typ återanvänder den här koden till alla fiender istället för att uppdatera enemy logic för orka asså holy shit
{
    [SerializeField] Vector2 hitboxsize;
    [SerializeField] float hitboxoffset;
    [SerializeField] float meleeDmg;
    [SerializeField] float meleeDelay; //hur länge tills försöka skada

    SpriteRenderer sprite;
    Animator animator;

    public override void Start()
    {
        base.Start();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    public override void Update()
    {
        sprite.flipX = !faceright;
        base.Update();
        activeMeleeCool -= Time.deltaTime;
    }

    public override void MeleeAttack()
    {
        if (activeMeleeCool > 0 || activeMelee) return;

        //activeMelee = true;
        StartCoroutine(meleeDetect());
    }

    IEnumerator meleeDetect()
    {
        {
            animator.SetBool("Attacking", true); //spela attack animation

            activeMelee = true;
            yield return new WaitForSecondsRealtime(meleeDelay); //vänta attackdelay tiden innan hitbox sätts på

            Collider2D hitObj = Physics2D.OverlapBox(
                (Vector2)transform.position + new Vector2(hitboxoffset * walkDir, 0) , hitboxsize, 0, playerLayer);
            

            print(hitObj);

            if (hitObj != null && hitObj.CompareTag("Player") && !hitObj.GetComponent<PlayerMovement>().isDashing)
            {
                hitObj.GetComponent<PlayerRespawn>().playerHealth -= meleeDmg; // Deal damage to player
            }

            // After the attack, reset the cooldown and the attacking flag
            activeMeleeCool = meleeCool;
            activeMelee = false; // Attack is done
            animator.SetBool("Attacking", false);
        }

        void OnDrawGizmos()
        {
            if (activeMelee)
            {
                Gizmos.color = Color.red;
                Vector2 colliderCenter = (Vector2)transform.position + new Vector2(hitboxoffset * walkDir, 0); // Position of the collider
                Gizmos.DrawWireCube(colliderCenter, hitboxsize); // Size and position of the box
            }
        }
    }
}
