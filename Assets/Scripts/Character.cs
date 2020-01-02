using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    [SerializeField] PlayerHandler ph;
    Vector2 move;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] Transform groundcheck;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float jumpForce = 1000;
    float coyoteTime;
    [SerializeField] float maxCoyote = 0.3f;
    [SerializeField] GameObject puff;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landingSound;

    bool canSpawn = false;
    public bool canInput = false;

    WeaponGrabber grabber;

    private float groundRadius = 0.35f;
    bool grounded = true;
    private Transform gfx;
    Rigidbody2D rb;

    Damageable dmg;

    float deadTimer;
    float spawnTimer = 3.0f;

    bool attacking = false;
    [SerializeField]
    Gun gun;

    float jumpGrav = 3f;
    float fallGrav = 7f;

    Animator anim;

    LayerMask origLayer;
    LayerMask ignorePlats = 14;

    // Use this for initialization
    void Start() {
        canInput = true;
        gfx = transform.Find( "GFX" );
        anim = gfx.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dmg = GetComponent<Damageable>();
        origLayer = gameObject.layer;
        grabber = transform.GetChild( 2 ).GetChild( 0 ).GetComponent<WeaponGrabber>();
        anim.SetBool( "Dead", false );

        coyoteTime = maxCoyote;

        jumpGrav = rb.gravityScale;
    }

    private void Update() {
        if( dmg.isDead ) {
            deadTimer += Time.deltaTime;

            if( deadTimer >= spawnTimer ) {
                canSpawn = true;
            }
        }

        if( !grounded && coyoteTime >= 0 ) {
            coyoteTime -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if( !canInput ) {
            if( grabber.weaponHeld ) {
                grabber.Interact();
            }
            anim.SetBool( "Dead", true );
            move = Vector2.zero;
        }

        grounded = Physics2D.OverlapCircle( groundcheck.position, groundRadius, whatIsGround );
        anim.SetBool( "Ground", grounded );

        if( grounded ) {
            rb.gravityScale = jumpGrav;
            coyoteTime = maxCoyote;
        }

        Move();
        Attack();

        anim.SetFloat( "vSpeed", rb.velocity.y );
        anim.SetFloat( "Speed", Mathf.Abs( move.x ) );
    }

    public void SetPlayerHandler( PlayerHandler playerHandler ) {
        ph = playerHandler;
    }

    void Respawn() {
        canInput = true;
        canSpawn = false;
        attacking = false;
        deadTimer = 0;
        dmg.Replenish();
        anim.SetBool( "Dead", false );
        anim.SetBool( "Ground", true );

        transform.position = ph.GetRandSpawnPos();
    }

    void OnMove( InputValue value ) {
        if( canInput ) {
            move = value.Get<Vector2>();

            if( move.y < -0.5f ) {
                gameObject.layer = ignorePlats;
                rb.gravityScale = fallGrav;
                if( rb.velocity.y >= -1 ) {
                    rb.velocity = new Vector2( rb.velocity.x, rb.velocity.y - 1 );
                }
            }
            else {
                gameObject.layer = origLayer;
            }
        }
        else if( canSpawn ) {
            Respawn();
        }
    }
    void OnJump() {
        if( canInput && ( grounded || coyoteTime > 0 ) ) {
            grounded = false;
            anim.SetBool( "Ground", false );
            rb.velocity = new Vector2( rb.velocity.x, 0 );
            rb.AddForce( new Vector2( 0, jumpForce ) );
            coyoteTime = 0;
        }
        else if( canSpawn ) {
            Respawn();
        }
    }
    void OnJumpRelease() {
        if( canInput ) {
            Vector2 v = rb.velocity;
            v.y *= 0.3f;
            if( v.y > 0 ) {
                rb.velocity = v;
            }
            rb.gravityScale = fallGrav;
        }
    }
    void OnShoot() {
        if( canInput ) {
            attacking = !attacking;
        }
    }
    void OnShootRelease() {
        if( canInput ) {
            attacking = !attacking;
            StopAttack();
        }
    }
    void OnInteract() {
        if( canInput ) {
            gun = grabber.Interact();
        }
    }

    void Move() {
        if( canInput ) {
            float xMovement = move.x;

            rb.velocity = new Vector2( xMovement * maxSpeed, rb.velocity.y );
        }
    }

    void Attack() {
        if( canInput ) {
            if( attacking && gun ) {
                gun.Shoot();

                if( gun.currentAmmo <= 0 ) {
                    StartCoroutine( DropWeapon() );
                }
            }
        }
    }
    void StopAttack() {
        if( canInput ) {
            if( gun ) {
                gun.StopShooting();
            }
        }
    }

    IEnumerator DropWeapon() {
        gun = null;
        yield return new WaitForSeconds( 0.01f );
        gun = grabber.Interact();
    }


    void OnCollisionEnter2D( Collision2D collision ) {
        //if( collision.gameObject.name.Contains( "Grass" ) ) {
        //    AudioSource.PlayClipAtPoint( landingSound, Camera.main.transform.position );
        //}
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere( groundcheck.position, groundRadius );
    }
}
