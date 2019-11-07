using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{

    public float pickupRadius = 1.0f;

    Gun gun;
    [SerializeField]
    Transform origParent;

    SpriteRenderer sr;

    float maxRemovalTimer = 5.0f;
    float removalTimer = 5.0f;
    float maxDistance = 50.0f;

    [SerializeField]
    bool triggerable = true;
    int numCollisions = 0;
    float shootTimer = 0.1f;
    public float maxShootTimer = 0.1f;

    public bool onStand = false;

    [SerializeField]
    bool Available = true;
    public bool IsAvailable() {
        return Available;
    }
    public void MakeAvailable() {
        Available = true;
        onStand = true;
        gameObject.layer = 16;
        rb.bodyType = RigidbodyType2D.Kinematic;
        shootTimer = maxShootTimer;
    }
    public void SetAvailability( bool newAvailability ) {
        if( !newAvailability && onStand ) {
            WeaponStand ws = transform.parent.GetComponent<WeaponStand>();
            ws.hasWeapon = false;
            onStand = false;
            gameObject.layer = 11;
        }

        numCollisions = 0;
        Available = newAvailability;
        TogglePhysics( newAvailability );

        if( newAvailability ) {
            transform.SetParent( origParent );
        }
    }

    Rigidbody2D rb;
    Collider2D[] colliders;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        gun = GetComponent<Gun>();
        sr = transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
        removalTimer = maxRemovalTimer;
    }

    private void Start() {
        origParent = transform.parent;
    }

    private void Update() {
        if( Available ) {
            if( gun.currentAmmo <= 0 ) {
                removalTimer -= Time.deltaTime;
            }
            else {
                float distFromCenter = Mathf.Abs( ( transform.position - Vector3.zero ).magnitude );

                if( distFromCenter > maxDistance ) {
                    RemovePickup();
                }
            }

            if( removalTimer <= 0 ) {
                RemovePickup();
            }

            if( shootTimer > 0 ) {
                shootTimer -= Time.deltaTime;
            }
        }
    }

    public void Interact( Transform weaponHeld, Transform newParent ) {
        base.Interact( newParent );

        if( !weaponHeld && gun.currentAmmo > 0 ) {
            // pick it up
            SetAvailability( false );
            transform.SetParent( newParent );
            transform.localPosition = Vector3.zero;
            Vector3 scale = transform.localScale;
            scale.y = 1;
            transform.localScale = scale;
            onStand = false;

            transform.rotation = newParent.rotation;
        }
    }

    void TogglePhysics( bool physics ) {
        rb.bodyType = RigidbodyType2D.Dynamic;
        if( !physics ) {
            rb.velocity = Vector2.zero;
            ResetColor();
        }
        gun.enabled = !physics;
        rb.simulated = physics;

        if( physics ) {
            Vector3 dir = ( transform.parent.position - transform.parent.parent.position ).normalized;
            rb.AddForce( dir * 500.0f );
            gun.muzzleFlash.enabled = false;

            if( gun.currentAmmo <= 0 ) {
                SetEmptyColor();
            }
        }
        foreach( Collider2D col in colliders ) {
            col.enabled = physics;
        }
        shootTimer = 0;
    }

    void RemovePickup() {
        if( transform.parent == null ) {
            Destroy( gameObject );
        }

        gameObject.SetActive( false );
        gun.Reload();
        ResetColor();
        shootTimer = 0;
        removalTimer = maxRemovalTimer;
    }

    void SetEmptyColor() {
        Color temp = sr.color;
        temp.r = 1.0f;
        temp.g = 0.0f;
        temp.b = 0.0f;
        temp.a = 0.6f;
        sr.color = temp;
    }

    void ResetColor() {
        Color temp = sr.color;
        temp.r = 1.0f;
        temp.g = 1.0f;
        temp.b = 1.0f;
        temp.a = 1.0f;
        sr.color = temp;
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if( triggerable ) {
            if( shootTimer <= 0 && numCollisions++ > 1 ) {
                float hit = Mathf.Min( rb.velocity.magnitude / 6.0f, 1.0f );

                if( hit + Random.Range( 0.0f, 0.4f ) >= 0.75f ) {
                    gun.enabled = true;
                    gun.HitSurface();

                    if( gun.currentAmmo <= 0 ) {
                        SetEmptyColor();
                    }

                    gun.muzzleFlash.enabled = false;
                    gun.enabled = false;
                }

                shootTimer = maxShootTimer;
            }
        }
    }

}
