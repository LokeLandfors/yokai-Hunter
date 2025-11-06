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
{   //det här scriptet är äckligt variabel och skräp fyllt jag är så ledsen för den som läser detta

    // Movement
    public bool faceright;
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
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;

    //funktions-variabler (jag har ingen aning om vad de kallas)
    public int walkDir => faceright ? 1 : -1;
    //^^^ automatiskt ändra värde i förhållande till faceright
    public virtual bool TouchingWall => Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(walkDir,0), 0.3f, groundLayer) ? true : false;
    //^^^ Kolla om man rör en vägg åt det håll fienden går
    public virtual bool TouchingGround => Physics2D.OverlapCircle(groundCheck.transform.position, 0.05f, groundLayer) ? true : false;
    //^^^ Kolla om man är på marken med en overlap circle
    public virtual bool atLedge => Physics2D.Raycast((Vector2)transform.position + new Vector2(walkDir, 0), Vector2.down, 1.5f) ? true : false;
    //^^^ kolla om fienden är vid en kant

    public virtual bool atDropDown => Physics2D.Raycast((Vector2)transform.position + new Vector2(walkDir, 0), Vector2.down, 3.5f) ? true : false;
    //^^^ kolla om fienden är vid en kant som kan hoppas av från

    // Combat
    public bool melee; //kan göra melee attacker
    public float MeleeDist; //hur nära för att göra melee attack
    public float meleeAttackDist; //hur långt meleeattack når
    public float meleeCool; //Melee cooldown
    float activeMeleeCool;
    bool activeMelee; //aktiverat melee

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

    void Start()
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
            if (melee && (player.transform.position - transform.position).magnitude < meleeAttackDist && TouchingGround)
            {
                currentspeed = 0;
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
        if (TouchingWall && !TouchingGround) //vid vägg men i luften, kan vara fast
    {
        if (jumps && activeJumpCool <= 0) //försök hoppa över om det går
        {
            Jump();
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
        if (TouchingGround && activeJumpCool >= 0)
        {
            rb.linearVelocityY = jumpForce;
            activeJumpCool = jumpCooldown;
        }
    }

    public virtual void LongJump() //ifall spelaren är på en plattform
    {
        if (TouchingGround && activeJumpCool >= 0)
        {
            rb.linearVelocity = new Vector2(agrospeed*jumpForce , jumpForce);
            activeJumpCool = jumpCooldown;
            //activeLongJump = true;
            print("Big ass jump");
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
        Vector2 plrOffset = new Vector2(transform.position.y - player.position.y, transform.position.x - player.position.x);
        float glen = (player.position - transform.position).magnitude;
        print(plrOffset);
        print(glen);
        //fett lång if men basically kollar ifall spelaren är för långt uppe eller nere över enemy
        /*if ((plrOffset.y < transform.position.y + 2 || plrOffset.y > transform.position.y -2))
        {
            if (plrOffset.x < transform.position.x + 2 || plrOffset.x > transform.position.x - 2) return;
            //^^^ avsluta om spelaren är för nära på x-axeln, då den antagliggen är i princip över enemy då den kommer börja "spasma"
        }*/
        faceright = point.x - transform.position.x > 0 ? true : false;
        if (searching)
        {
            Walk();
            if (TouchingWall && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
            {
                Jump();
            }
            else if (atLedge && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
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
        if (TouchingWall && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
        {
            Jump();
        }
        else if (atLedge && jumps && player.position.y >= transform.position.y - 0.1f && activeJumpCool <= 0)
        {
            LongJump();
        }

        Walk();
    }
}