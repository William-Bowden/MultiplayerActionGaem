using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.PlayerInput;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    Vector2 move;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] Transform groundcheck;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float jumpForce = 1000;
    [SerializeField] GameObject puff;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landingSound;

    public bool canInput = false;

    WeaponGrabber grabber;

    private float groundRadius = 0.35f;
    bool grounded = true;
    private Transform gfx;
    Rigidbody2D rb;

    Damageable dmg;

    bool attacking = false;
    Gun gun;

    float jumpGrav = 3f;
    float fallGrav = 7f;

    Animator anim;

    LayerMask origLayer;
    LayerMask ignorePlats;

    // Use this for initialization
    void Start() {
        canInput = true;
        gfx = transform.Find( "GFX" );
        anim = gfx.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dmg = GetComponent<Damageable>();
        origLayer = gameObject.layer;
        ignorePlats = 14;
        grabber = transform.GetChild( 2 ).GetChild( 0 ).GetComponent<WeaponGrabber>();
        anim.SetBool( "Dead", false );

        jumpGrav = rb.gravityScale;
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
        else {
            anim.SetBool( "Dead", false );
        }

        grounded = Physics2D.OverlapCircle( groundcheck.position, groundRadius, whatIsGround );
        anim.SetBool( "Ground", grounded );

        if( grounded ) {
            rb.gravityScale = jumpGrav;
        }

        Move();
        Attack();

        anim.SetFloat( "vSpeed", rb.velocity.y );
        anim.SetFloat( "Speed", Mathf.Abs( move.x ) );
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
    }
    void OnJump() {
        if( canInput && grounded ) {
            anim.SetBool( "Ground", false );
            rb.velocity = new Vector2( rb.velocity.x, 0 );
            rb.AddForce( new Vector2( 0, jumpForce ) );
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
    void OnInteract() {
        if( canInput ) {
            grabber.Interact();

            if( !gun ) {
                gun = GetComponentInChildren<Gun>();
            }
            else {
                gun = null;
            }
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
            }
        }
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
