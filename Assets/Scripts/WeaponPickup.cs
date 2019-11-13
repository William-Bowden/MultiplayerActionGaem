using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{
    public float pickupRadius = 1.0f;

    Gun gun;
    [SerializeField]
    Transform origParent;

    float emptyRemovalTimer = 5.0f;
    float droppedRemovalTimer = 15.0f;
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
            if( gun.currentAmmo <= 0 ) {
                StartCoroutine( DropWeapon( emptyRemovalTimer ) );
            }
            else {
                StartCoroutine( DropWeapon( droppedRemovalTimer ) );
            }
        }
        else {
            StopAllCoroutines();
            transform.localScale = Vector3.one;
        }
    }

    Rigidbody2D rb;
    Collider2D[] colliders;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        gun = GetComponent<Gun>();
        removalTimer = maxRemovalTimer;
    }

    private void Start() {
        origParent = transform.parent;
    }

    private void Update() {
        if( Available ) {
            if( gun.currentAmmo <= 0 ) {
                //removalTimer -= Time.deltaTime;
            }
            else {
                float distFromCenter = Mathf.Abs( ( transform.position - Vector3.zero ).magnitude );

                if( distFromCenter > maxDistance ) {
                    RemovePickup();
                }
            }

            //if( removalTimer <= 0 ) {
            //    RemovePickup();
            //}

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
            gun.ResetColor();
        }
        gun.enabled = !physics;
        rb.simulated = physics;

        if( physics ) {
            Vector3 dir = ( transform.parent.position - transform.parent.parent.position ).normalized;
            rb.AddForce( dir * 500.0f );
            gun.muzzleFlash.enabled = false;

            if( gun.currentAmmo <= 0 ) {
                gun.SetEmptyColor();
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
        gun.ResetColor();
        shootTimer = 0;
        removalTimer = maxRemovalTimer;
    }

    IEnumerator DropWeapon( float removalTime ) {
        StartCoroutine( ShrinkWeapon( removalTime / 50.0f, removalTime ) );
        yield return new WaitForSeconds( removalTime );
        RemovePickup();
    }
    IEnumerator ShrinkWeapon( float waitTime, float removalTime ) {
        while( true ) {
            yield return new WaitForSeconds( waitTime );
            //transform.localScale *= 0.975f;
            transform.localScale -= Vector3.one * waitTime / removalTime;
        }
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if( triggerable ) {
            if( shootTimer <= 0 && numCollisions++ > 1 ) {
                float hit = Mathf.Min( rb.velocity.magnitude / 6.0f, 1.0f );

                if( hit + Random.Range( 0.0f, 0.4f ) >= 0.75f ) {
                    gun.enabled = true;
                    gun.HitSurface();

                    if( gun.currentAmmo <= 0 ) {
                        gun.SetEmptyColor();
                    }

                    gun.muzzleFlash.enabled = false;
                    gun.enabled = false;
                }

                shootTimer = maxShootTimer;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere( transform.position, pickupRadius );
    }

}
