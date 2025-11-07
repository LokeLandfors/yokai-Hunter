using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class BaseEnemy : EnemyCoreLogic
{
    [SerializeField] Vector2 hitboxsize;
    [SerializeField] float hitboxoffset;
    [SerializeField] float meleeDmg;
    [SerializeField] float meleeDelay; //hur länge tills försöka skada

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        activeMeleeCool -= Time.deltaTime;
    }

    public override void MeleeAttack()
    {
        print("POW");
        if (activeMeleeCool > 0 || activeMelee) return;

        //activeMelee = true;
        StartCoroutine(meleeDetect());
    }

    IEnumerator meleeDetect()
    {
        {
            activeMelee = true; // Set flag to indicate the attack is in progress
            yield return new WaitForSecondsRealtime(meleeDelay); // Wait for the delay (animation or cooldown)

            // Only perform the attack once (this is to prevent continuous damage)
            Collider2D hitObj = Physics2D.OverlapBox(
                (Vector2)transform.position + new Vector2(hitboxoffset * walkDir, 0),
                hitboxsize,
                0,
                playerLayer
            );

            print(hitObj);

            if (hitObj != null && hitObj.CompareTag("Player"))
            {
                hitObj.GetComponent<PlayerRespawn>().playerHealth -= meleeDmg; // Deal damage to player
            }

            // After the attack, reset the cooldown and the attacking flag
            activeMeleeCool = meleeCool;
            activeMelee = false; // Attack is done
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
