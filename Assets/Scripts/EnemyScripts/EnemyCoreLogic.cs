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

public class EnemyCoreLogic : MonoBehaviour //Av edwin
{   //jag vet inte vem som ska läsa det här scriptet men jag ber så mycket om ursäkt för att du behöver göra det 

    // Movement
    [Header("Movement settings")]
    bool faceright;
    public float roamspeed; //vanlig gå hastighet
    public float agrospeed; //gå hastighet när ser spelare
    public bool jumps; //kan hoppa
    public float jumpForce;
    public bool wallclimbs; //kan klättra väggar
    float currentspeed;
    float jumpCooldown = 0.5f;
    float activeJumpCool = 0;
    bool activeLongJump = false;

    // Physics och collisions
    [Header("Collison checking")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header("Platforming and movement checks")]
    public float wallCheckOffset;
    public Vector2 wallCheckSize;

    public Vector2 groundCheckSize;

    public float ledgeCheckOffset;
    public float ledgeCheckReach;

    public float dropDownReach;

    //funktions-variabler (jag har ingen aning om vad de kallas)
    virtual public int walkDir => faceright ? 1 : -1;
    //^^^ automatiskt ändra värde i förhållande till faceright
    public virtual bool TouchingWall => Physics2D.OverlapBox((Vector2)transform.position + new Vector2(wallCheckOffset*walkDir,0), wallCheckSize, groundLayer) ? true : false;
    //^^^ Kolla om man rör en vägg åt det håll fienden går
    public virtual bool TouchingGround => Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize,0, groundLayer) ? true : false;
    //^^^ Kolla om man är på marken med en overlapox
    public virtual bool atLedge => Physics2D.Raycast((Vector2)transform.position + new Vector2(ledgeCheckOffset * walkDir, 0), Vector2.down, ledgeCheckReach, groundLayer).collider ? false : true;
    //^^^ kolla om fienden är vid en kant

    public virtual bool atDropDown => Physics2D.Raycast((Vector2)transform.position + new Vector2(walkDir, 0), Vector2.down, dropDownReach, groundLayer).collider ? true : false;
    //^^^ kolla om fienden är vid en kant som är låg nog att hoppa från

    // Combat
    public bool melee; //kan göra melee attacker
    public float MeleeDist; //hur nära för att göra melee attack
    public float meleeCool; //Melee cooldown
    public float activeMeleeCool;
    public bool activeMelee; //aktiverat melee

    public bool ranged; //kan använda distansvapen
    public float fireRange; //hur långt bort den får skjuta mot spelaren
    public bool walkableranged; //kan gå medans skjuter

    // Playerdetection
    public float visDist; //hur långt spelaren syns
    bool seePlr => SearchTarget() ? true: false;
    bool following = false; //ifall den ser och följer efter spelaren
    bool searching = false; //^^^ fast den inte ser spelaren
    [SerializeField] float maxSearchTime; //hur länge den får försöka nå sista sedda position
    float activeSearchTime;
    Vector2 lastseen; //spelarens sista sedda position
    Transform player;
    public LayerMask playerLayer; //coolt rim grej



    public virtual void MeleeAttack()
    {

    }

    public virtual void RangedAttack()
    {

    }

    public virtual void Start()
    {
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
            if (melee && (player.transform.position - transform.position).magnitude < MeleeDist && TouchingGround)
            {
                currentspeed = 0;
                print("mele");
                MeleeAttack();
                return;
            }
            else if (ranged && (player.transform.position - transform.position).magnitude < fireRange && TouchingGround)
            {
                currentspeed = 0;
                RangedAttack();
                return;
            }
            else
            {
            following = true;
            currentspeed = agrospeed;
            FollowTarget();
            }
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
            currentspeed = roamspeed;
            following = false;
            searching = false;
            Roam();
        }
        activeLongJump = !TouchingGround;
        activeJumpCool = Mathf.Clamp(activeJumpCool - Time.deltaTime,0,jumpCooldown);
        activeSearchTime = Mathf.Clamp(activeSearchTime - Time.deltaTime, 0, maxSearchTime);
    }

    public virtual void Walk()
    {
        if (TouchingWall && !TouchingGround && !searching && !following) //vid vägg men i luften, kan vara fast
        {
            if (jumps && activeJumpCool <= 0) //försök hoppa över om det går
            {
                LongJump();
            }
            else //byt riktning och gå bort
            {
                faceright = !faceright;
                Walk();
            }
        }
        else
        {
            rb.linearVelocityX = !activeLongJump ? currentspeed / 5 * walkDir : rb.linearVelocityX;
        }
    }

    public virtual void Jump()
    {
        if (TouchingGround && activeJumpCool <= 0)
        {
            rb.linearVelocityY = jumpForce;
            activeJumpCool = jumpCooldown;
        }
    }

    public virtual void LongJump() //ifall spelaren är på en plattform
    {
        if (TouchingGround && activeJumpCool <= 0)
        {
            rb.linearVelocity = new Vector2(agrospeed*jumpForce*walkDir, jumpForce);
            activeJumpCool = jumpCooldown;
            activeLongJump = true;

        }
    }

    public virtual void Roam() //Patrullering typ, gå fram och tillbaks
    {
        currentspeed = roamspeed;
        Walk();
        faceright = TouchingWall || !atDropDown && TouchingGround ? !faceright : faceright;
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

    void GoToPoint(Vector2 point) //egentliggen själva leta funktionen men orka byta namn nu
    {

        if (transform.position.x - player.position.x < 1 && transform.position.y - player.position.y > 3)
            //^^^ spelaren borde synas ifall de är så nära på x, annars är de på olika plattformar och movementet är för primitivt för att hantera det
        {
            activeSearchTime = 0; //sluta söka direkt
            searching = false;
            return;
        }

        faceright = point.x - transform.position.x > 0 ? true : false;
        if (searching)
        {
            Walk();
            if ((TouchingWall || atLedge) && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
            {
                LongJump();
            }

        }
    }

    public virtual void FollowTarget() //följ spelaren
    {
        following = true;
        lastseen = player.position;
        faceright = lastseen.x - transform.position.x > 0 ? true : false;
        Walk();

        if (!TouchingGround)
        {
            return;
        }
        else if (TouchingWall && TouchingGround && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
        {
            Jump();
        }
        else if (atLedge && TouchingGround && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
        {
            LongJump();
        }

    }
}