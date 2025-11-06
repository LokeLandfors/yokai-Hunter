using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShadowWalkerLogic : EnemyCoreLogic
{
    public int damage = 2; //Skada som spelaren ska ta när Enemy rör honom
    public float attackTime = 2f;
    protected virtual bool MeleeAttack(bool returnFoundHit)
    {
        return false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerRespawn player = collision.gameObject.GetComponent<PlayerRespawn>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
    private IEnumerator EnemyMeleeAttack()
    {
        bool hit = false;
        while (activeMeleeCool > 0)
        {
            MeleeAttack(hit);
        activeMeleeCool -= Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
