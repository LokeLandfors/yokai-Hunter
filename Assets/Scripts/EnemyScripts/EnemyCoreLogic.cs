using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyCoreLogic : MonoBehaviour //allt det här av Edwin tror jag
{
    public int maxhealth;
    int health;

    //movement
    public bool faceright;
    public float roamspeed; //vanlig gå hastighet
    public float agrospeed; //gå hastighet när ser spelare
    public bool jumps; //kan hoppa
    public float jumpForce;
    public bool wallclimbs; //kan klättra väggar
    float currentspeed;
    float jumpCooldown = 0.5f;
    float activeJumpCool = 0;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;

    public int walkDir => faceright ? 1 : -1;
    //^^^ automatiskt ändra värde i förhållande till faceright
    public virtual bool TouchingWall => Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(walkDir,0), 0.2f, groundLayer) ? true : false;
    //^^^ Kolla om man rör en vägg åt det håll fienden går
    public virtual bool TouchingGround => Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundLayer) ? true : false;
    //^^^ Kolla om man är på marken med en overlap circle
    public virtual bool atLedge => Physics2D.Raycast((Vector2)transform.position + new Vector2(walkDir, 0), Vector2.down, 1.3f) ? false : true;
    //^^^ kolla om fienden är vid en kant

    //combat
    public bool melee; //kan göra melee attacker
    public float meleeRange; //hur långt meleeattack når
    public bool ranged; //kan använda distansvapen
    public float fireRange; //hur långt bort den får skjuta mot spelaren

    //detection
    public float visDist; //hur långt spelaren syns
    bool seePlr => SearchTarget() ? true: false;
    bool following = false; //ifall den ser och följer efter spelaren
    bool searching = false; //^^^ fast den inte ser spelaren
    [SerializeField] float maxSearchTime; //hur länge den får försöka nå sista sedda position
    float activeSearchTime;
    Vector2 lastseen; //spelarens sista sedda position
    Transform player;
    public LayerMask playerLayer; //holing shits det rimmar!!!!

    public virtual void MeleeAttack()
    {

    }

    public virtual void RangedAttack()
    {

    }

    void Start()
    {
        health = maxhealth;
        player = GameObject.Find("Player").transform;
        currentspeed = roamspeed;
        //gör slumpmässigt vilket håll fienden spawnar åt
        int rand = Random.Range(0, 1);
        if (rand == 1) faceright = true; else faceright = false;
    }

    public virtual void Update()
    {
        if (seePlr) //ser spelaren och börjar sitt förföljande
        {
            following = true;
            currentspeed = agrospeed;
            FollowTarget();
        }
        else if (following && !seePlr) //om den har tappat syn av fienden så börja sök funktionen
        {
            searching = true;
            following = false;
            currentspeed = agrospeed;
            activeSearchTime = maxSearchTime;
        }
        else if(searching && activeSearchTime > 0) //sök tills search time är 0
        {
            GoToPoint(lastseen);
        }
        else
        {
            following = false;
            searching = false;
            Roam();
        }

        /*if (seePlr) //ser spelaren och börjar sitt förföljande
        {
            following = true;
            currentspeed = agrospeed;
            FollowTarget();
        }
        else if (following) //om den har tappat syn av fienden
        {
            searching = true;
            following = false;
            currentspeed = agrospeed;
            activeSearchTime = maxSearchTime;
            GoToPoint(lastseen);
        }
        else
        {
            following = false;
        }
        if (!seePlr && !searching) //Roam
        {
            currentspeed = roamspeed;
            Roam();
        }
        else if (!seePlr && following) //Search
        {
            currentspeed = agrospeed;
            following = false;
            searching = true;
        }
        else //follow
        {
            FollowTarget();
        }*/
        currentspeed = TouchingGround ? currentspeed : 0; //kan inte ändra hastighet i luften
        activeJumpCool = Mathf.Clamp(activeJumpCool - Time.deltaTime,0,jumpCooldown);
        activeSearchTime = Mathf.Clamp(activeSearchTime - Time.deltaTime, 0, maxSearchTime);
    }

    public virtual void Walk()
    {
        rb.linearVelocityX = currentspeed * walkDir;
    }

    public virtual void Jump()
    {
        if (TouchingGround && activeJumpCool >= 0)
        {
            rb.linearVelocity = Vector2.one * jumpForce;
            activeJumpCool = jumpCooldown;
        }
    }

    public virtual void Roam() //Patrullering typ, gå fram och tillbaks
    {

        currentspeed = roamspeed;
        Walk();
        faceright = TouchingWall || atLedge && TouchingGround ? !faceright : faceright;
        /*if (seePlr) //ser spelaren och börjar sitt förföljande
        {
            following = true;
            currentspeed = agrospeed;
            FollowTarget();
        }
        else if (following) //om den har tappat syn av fienden
        {
            searching = true;
            following = false;
            currentspeed = agrospeed;
            activeSearchTime = maxSearchTime;
            GoToPoint(lastseen);
        }
        else
        {
            following = false;
        }*/
    }

    public virtual bool SearchTarget() //kolla om target är synlig
    {
        RaycastHit2D searchRay = Physics2D.Raycast(transform.position, player.position - transform.position, visDist, playerLayer);
        Debug.DrawRay(transform.position, player.position - transform.position);
        if (searchRay.collider) //player seen
        {
            if (searchRay.collider.gameObject.name == "Player")
            {
                return true;
            }
        }
        return false;

    }

    void GoToPoint(Vector2 point)
    {
        faceright = point.x - transform.position.x > 0 ? true : false;
        if (searching)
        {
            Walk();
            if ((TouchingWall || atLedge) && jumps) Jump();
        }
    }

    public virtual void FollowTarget() //följ spelaren
    {
        following = true;
        lastseen = player.position;
        faceright = lastseen.x - transform.position.x > 0 ? true : false;
        Walk();
        if ((TouchingWall || atLedge) && jumps && player.transform.position.y >= transform.position.y -.1f) Jump();
    }
}