﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.PlayerInput;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    Vector2 move;
    float maxSpeed = 10;
    public Transform groundcheck;
    public LayerMask whatIsGround;
    public float jumpForce = 1000;
    public GameObject puff;
    public AudioClip jumpSound;
    public AudioClip landingSound;

    private float groundRadius = 0.5f;
    public bool grounded = true;
    private Transform gfx;
    Rigidbody2D rb;

    Damageable dmg;

    float jumpGrav = 2.5f;
    float fallGrav = 6f;

    Animator anim;

    LayerMask origLayer;
    LayerMask ignorePlats;

    // Use this for initialization
    void Start() {
        gfx = transform.Find( "GFX" );
        anim = gfx.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dmg = GetComponent<Damageable>();
        origLayer = gameObject.layer;
        ignorePlats = 14;
    }

    void Update() {

        //if( ( grounded ) && ( Input.GetKeyDown( KeyCode.Space ) || Input.GetButtonDown( "Jump" ) ) ) {
        //    Vector3 newPos = gameObject.transform.position;
        //    newPos.y -= 0.4f;
        //    newPos.z -= 0.5f;

        //    GameObject smokePuff = Instantiate( puff, newPos, Quaternion.identity ) as GameObject;
        //    Destroy( smokePuff, 1f );

        //    AudioSource.PlayClipAtPoint( jumpSound, Camera.main.transform.position );

        //    anim.SetBool( "Ground", false );
        //    rb.velocity = new Vector2( rb.velocity.x, 0 );
        //    rb.AddForce( new Vector2( 0, jumpForce ) );
        //}

        //when jump is released upward velocity is dampened
        if( Input.GetKeyUp( KeyCode.Space ) || Input.GetButtonUp( "Jump" ) ) {
            Vector2 v = rb.velocity;
            v.y *= 0.3f;
            if( v.y > 0 ) {
                rb.velocity = v;
            }
            rb.gravityScale = fallGrav;
        }

        if( Input.GetKeyDown( KeyCode.S ) ) {
            gameObject.layer = ignorePlats;
        }
        else if( Input.GetKeyUp( KeyCode.S ) ) {
            gameObject.layer = origLayer;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        grounded = Physics2D.OverlapCircle( groundcheck.position, groundRadius, whatIsGround );
        anim.SetBool( "Ground", grounded );

        if( grounded ) {
            rb.gravityScale = jumpGrav;
        }

        anim.SetFloat( "vSpeed", rb.velocity.y );

        //float move = Input.GetAxis( "Horizontal" );
        Move();

        anim.SetFloat( "Speed", Mathf.Abs( move.x ) );

        //rb.velocity = new Vector2( move * maxSpeed, rb.velocity.y );
    }

    void OnMove( InputValue value ) {
        move = value.Get<Vector2>();
    }
    void OnJump() {
        anim.SetBool( "Ground", false );
        rb.velocity = new Vector2( rb.velocity.x, 0 );
        rb.AddForce( new Vector2( 0, jumpForce ) );
    }
    void OnShoot() {
        Attack();
    }

    void Move() {
        float xMovement = move.x;

        rb.velocity = new Vector2( xMovement * maxSpeed, rb.velocity.y );
    }

    void Attack() {
        Debug.Log( "Shooting!(temp)" );
    }


    void OnCollisionEnter2D( Collision2D collision ) {
        if( collision.gameObject.name.Contains( "Grass" ) ) {
            //AudioSource.PlayClipAtPoint( landingSound, Camera.main.transform.position );
        }
    }
}