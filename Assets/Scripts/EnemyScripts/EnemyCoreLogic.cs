using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderGraph.Internal;
using System.Runtime.InteropServices.WindowsRuntime;

public class EnemyCoreLogic : MonoBehaviour
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
    public float cloaks; //kan bli osynlig
    float currentspeed;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;

    public int walkDir => faceright ? 1 : -1;
    public virtual bool TouchingWall => Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(walkDir,0), 0.2f, groundLayer) ? true : false;
    public virtual bool TouchingGround => Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundLayer) ? true : false;

    //combat
    public bool melee; //kan göra melee attacker
    public float meleeRange; //hur långt meleeattack når
    public bool ranged; //kan använda distansvapen
    public float fireRange; //hur långt bort den får skjuta mot spelaren

    //detection
    public float visDist; //hur långt spelaren syns
    bool seePlr => SearchTarget() ? true: false;
    bool following = false;
    bool searching = false;
    Vector2 lastseen;
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
        player = GameObject.Find("Player").transform;

        //gör slumpmässigt vilket håll fienden spawnar åt
        int rand = Random.Range(0, 1);
        if (rand == 1) faceright = true; else faceright = false;
    }

    private void Update()
    {
        SearchTarget();
        if (!seePlr)
        {
            Roam();
        }
        else
        {
            FollowTarget();
        }
    }

    public virtual void Walk()
    {
        rb.linearVelocityX = roamspeed * walkDir;
    }

    public virtual void Jump()
    {
        if (TouchingGround)
        {
        rb.linearVelocityY = jumpForce; 
        }
    }

    public virtual void Roam() //Gå runt utan mening i livet
    {
        if (seePlr)
        {
            FollowTarget();
        }
        else if (searching)
        {
            GoToPoint(lastseen);
        }
        else
        {
            Walk();
            faceright = TouchingWall ? !faceright : faceright;
        }
    }

    public virtual bool SearchTarget() //kolla om target är synlig
    {
        RaycastHit2D searchRay = Physics2D.Raycast(transform.position, player.position - transform.position, visDist, playerLayer);
        Debug.DrawRay(transform.position, player.position - transform.position);
        if (searchRay.collider) //player seen
        {
            print(searchRay.collider.gameObject.name);
            if (searchRay.collider.gameObject.name == "Player")
            {
                return true;
                following = true;
            }
        }
        else if (following)
        {
            following = false;
            searching = true;
        }
        print("Lost player");
        return false;

    }

    void GoToPoint(Vector2 point)
    {
        faceright = point.x - transform.position.x > 0 ? true : false;
        while (true)
        {
            currentspeed = agrospeed;
            Walk();
            if (TouchingWall) Jump();
            new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public virtual void FollowTarget() //ja du VAD kan den här göra????
    {
        following = true;
        lastseen = player.position;
    }
}
