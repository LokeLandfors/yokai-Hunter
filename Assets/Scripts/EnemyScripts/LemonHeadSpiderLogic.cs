using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LemonHeadSpiderLogic : BaseEnemy
{
    public override void Walk()
    {
            LongJump(); //har ingen walk anim så den hoppar lol
    }

    public override void FollowTarget()
    {
        LongJump();
    }

    public override void Roam()
    {
        currentspeed = roamspeed;
        Walk();
        faceright = TouchingWall || !atDropDown && TouchingGround ? !faceright : faceright;
    }

    public override void GoToPoint(Vector2 point)
    {
        if (transform.position.x - player.position.x < 1 && transform.position.y - player.position.y > 3)
        //^^^ spelaren borde synas ifall de är så nära på x, annars är de på olika plattformar och movementet är för primitivt för att hantera det
        {
            searching = false;
            return;
        }

        faceright = point.x - transform.position.x > 0 ? true : false;
        if (searching)
        {
            LongJump();

        }
    }
}
