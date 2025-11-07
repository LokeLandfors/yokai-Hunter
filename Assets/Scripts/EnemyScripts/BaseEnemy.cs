using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
        meleeDetect();
    }

    IEnumerator meleeDetect()
    {
        print("POW");
        if (activeMeleeCool > 0) yield break;
        activeMelee = true;
        yield return new WaitForSecondsRealtime(meleeDelay);
        Collider2D hitObj = Physics2D.OverlapBox(new Vector2(hitboxoffset * walkDir, 0), hitboxsize, 0, 8);
        print(hitObj);
        activeMeleeCool = meleeCool;
        if (hitObj == null)
        {
            yield break;
        }
        else
        {
            hitObj.GetComponent<PlayerRespawn>().playerHealth -= meleeDmg;
        }
    }

}
